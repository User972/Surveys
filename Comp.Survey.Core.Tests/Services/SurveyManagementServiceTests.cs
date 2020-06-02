using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Moq;
using Comp.Survey.Core.Entities;
using Comp.Survey.Core.Interfaces;
using Comp.Survey.Core.Interfaces.DTO;
using Comp.Survey.Core.Services;
using Serilog;
using Xunit;

namespace Comp.Survey.Core.Tests.Services
{
    public class SurveyManagementServiceTests
    {
        private readonly Mock<ISurveyRepository> _surveyRepository;
        private readonly Mock<ISurveyQuestionRepository> _optionsRepository;
        private readonly Guid _guid;
        private readonly string _surveyName = "surveyName1";
        private readonly SurveyManagementService _svc;
        private readonly Guid _nonMatchingGuid;
        private readonly Guid _guid2;

        public SurveyManagementServiceTests()
        {
            _guid = Guid.NewGuid();
            _guid2 = Guid.NewGuid();
            _nonMatchingGuid = Guid.NewGuid();

            var survey1 = new Entities.Survey
            {
                Id = _guid
            };
            var survey2 = new Entities.Survey
            {
                Id = _guid2
            };
            var allSurveys = new List<Entities.Survey>() { survey1, survey2 };
            var filteredSurveys = new List<Entities.Survey>() { survey1 };

            _surveyRepository = new Mock<ISurveyRepository>();
            _surveyRepository.Setup(repo =>
                repo.Create(It.IsAny<Entities.Survey>())).ReturnsAsync(survey1);

            _surveyRepository.Setup(repo =>
                repo.Get(It.Is<Guid>(a => a.Equals(_guid)))).ReturnsAsync(new Entities.Survey
                {
                    Id = _guid
                });

            _surveyRepository.Setup(repo =>
                repo.Get(It.Is<Guid>(a => a.Equals(_nonMatchingGuid)))).ReturnsAsync(null as Entities.Survey);

            _surveyRepository.Setup(repo =>
                repo.Update(It.IsAny<Entities.Survey>()));

            _surveyRepository.Setup(repo =>
                repo.List()).ReturnsAsync(allSurveys);

            _surveyRepository.Setup(repo =>
                repo.List(It.IsAny<Expression<Func<Entities.Survey, bool>>>())).ReturnsAsync(filteredSurveys);
            
            _surveyRepository.Setup(repo =>
                repo.Delete(It.IsAny<Entities.Survey>()));

            // Options Repository
            _optionsRepository = new Mock<ISurveyQuestionRepository>();
            _optionsRepository.Setup(repo =>
                repo.Delete(It.IsAny<Expression<Func<SurveyQuestion, bool>>>()));

            var logger = new Mock<ILogger>();
            logger.Setup(l => l.Error(It.IsAny<Exception>(), It.IsAny<string>()));
            logger.Setup(l => l.Warning(It.IsAny<string>()));

            _svc = new SurveyManagementService(_surveyRepository.Object, logger.Object);
        }

        [Fact]
        public void CreateNewSurvey_WhenPassedSurveyDto__CallsRepository_ReturnsUpdatedDto()
        {
            var dto = Mock.Of<ISurvey>();
            var result = _svc.CreateNewSurvey(dto).Result;

            _surveyRepository.Verify(repo => repo.Create(It.IsAny<Entities.Survey>()), Times.Exactly(1));
            Assert.Equal(_guid, result.Id);
        }

        [Fact]
        public void UpdateExistingSurvey_WhenNoSurveyFound_DoesNotCallRepository_ReturnsFalse()
        {
            var dto = Mock.Of<ISurvey>();
            var result = _svc.UpdateExistingSurvey(_nonMatchingGuid, dto).Result;

            _surveyRepository.Verify(repo => repo.Update(It.IsAny<Entities.Survey>()), Times.Never);
            Assert.False(result);
        }


        [Fact]
        public void UpdateExistingSurvey_WhenSurveyFound_CallsRepository_ReturnsTrue()
        {
            var dto = Mock.Of<ISurvey>();
            var result = _svc.UpdateExistingSurvey(_guid, dto).Result;

            _surveyRepository.Verify(repo => repo.Update(It.IsAny<Entities.Survey>()), Times.Exactly(1));
            Assert.True(result);
        }

        [Fact]
        public void GetSurveyById_WhenSurveyNotFound_CallsRepository_ReturnsNull()
        {
            var result = _svc.GetSurveyById(_nonMatchingGuid).Result;

            _surveyRepository.Verify(repo => repo.Get(It.Is<Guid>(a => a.Equals(_nonMatchingGuid))), Times.Exactly(1));
            Assert.Null(result);
        }

        [Fact]
        public void GetSurveyById_WhenSurveyFound_CallsRepository_ReturnsSurveyDto()
        {
            var result = _svc.GetSurveyById(_guid).Result;

            _surveyRepository.Verify(repo => repo.Get(It.Is<Guid>(a => a.Equals(_guid))), Times.Exactly(1));
            Assert.IsAssignableFrom<ISurvey>(result);
        }

        [Fact]
        public void GetSurveys_CallsRepository_ReturnsAllSurveyDtoList()
        {
            var result = _svc.GetSurveys().Result;

            _surveyRepository.Verify(repo => repo.List(), Times.Exactly(1));
            Assert.NotEmpty(result);
            Assert.True(result.Count == 2);
            Assert.Contains(result, r => r.Id == _guid);
            Assert.Contains(result, r => r.Id == _guid2);
        }

        [Fact]
        public void GetSurveysWithNameLike_CallsRepository_ReturnsSurveyDtoList()
        {
            var result = _svc.GetSurveysWithNameLike(_surveyName).Result;

            _surveyRepository.Verify(repo => repo.List(It.IsAny<Expression<Func<Entities.Survey, bool>>>()), Times.Exactly(1));
            Assert.NotEmpty(result);
            Assert.True(result.Count == 1);
            Assert.Contains(result, r => r.Id == _guid);
            Assert.DoesNotContain(result, r => r.Id == _guid2);
        }

        [Fact]
        public void DeleteSurveyById_WhenSurveyNotFound_DoesNotCallRepository_ReturnsFalse()
        {
            var result = _svc.DeleteSurveyById(_nonMatchingGuid).Result;

            _surveyRepository.Verify(repo => repo.Delete(It.IsAny<Entities.Survey>()), Times.Never);
            _optionsRepository.Verify(repo => repo.Delete(It.IsAny<Expression<Func<SurveyQuestion, bool>>>()), Times.Never);
            Assert.False(result);
        }

        [Fact]
        public void DeleteSurveyById_WhenSurveyFound_CallsRepository_ReturnsTrue()
        {
            var result = _svc.DeleteSurveyById(_guid).Result;

            _surveyRepository.Verify(repo => repo.Delete(It.IsAny<Entities.Survey>()), Times.Exactly(1));
            Assert.True(result);
        }
    }
}
