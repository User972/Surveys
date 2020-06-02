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
    public class SurveyQuestionManagementServiceTests
    {
        private readonly Mock<ISurveyQuestionRepository> _questionRepository;
        private readonly Guid _surveyId;
        private readonly string _surveyName = "surveyName1";
        private readonly SurveyQuestionManagementService _svc;
        private readonly Guid _nonMatchingGuid;
        private readonly Guid _matchingGuid;
        private List<SurveyQuestion> _allOptions;
        private Expression<Func<SurveyQuestion, bool>> _expr1;

        public SurveyQuestionManagementServiceTests()
        {
            _surveyId = Guid.NewGuid();
            _matchingGuid = Guid.NewGuid();
            _nonMatchingGuid = Guid.NewGuid();
            _expr1 = p => p.Title.Contains(_surveyName, StringComparison.InvariantCultureIgnoreCase);

            var options1 = new SurveyQuestion
            {
                Id = _matchingGuid,
                SurveyId = _surveyId
            };
            var option2 = new SurveyQuestion
            {
                Id = _nonMatchingGuid,
                SurveyId = _surveyId
            };
            _allOptions = new List<SurveyQuestion>() { options1, option2 };
            var filteredOptions = new List<SurveyQuestion>() { options1 };

            _questionRepository = new Mock<ISurveyQuestionRepository>();

            _questionRepository.Setup(repo =>
                repo.Create(It.IsAny<SurveyQuestion>())).ReturnsAsync(options1);

            _questionRepository.Setup(repo =>
                repo.Get(It.Is<Guid>(a => a.Equals(_matchingGuid)))).ReturnsAsync(options1);

            _questionRepository.Setup(repo =>
                repo.Get(It.Is<Guid>(a => a.Equals(_nonMatchingGuid)))).ReturnsAsync(null as SurveyQuestion);

            _questionRepository.Setup(repo =>
                repo.Update(It.IsAny<SurveyQuestion>()));

            _questionRepository.Setup(repo =>
                repo.List(It.IsAny<Expression<Func<SurveyQuestion, bool>>>())).ReturnsAsync(filteredOptions);
           
            _questionRepository.Setup(repo =>
                repo.ListWithOptions(It.Is<Guid>(a=>a.Equals(_surveyId)))).ReturnsAsync(filteredOptions);
            
            _questionRepository.Setup(repo =>
                repo.Delete(It.IsAny<Expression<Func<SurveyQuestion, bool>>>()));
            
            var logger = new Mock<ILogger>();
            logger.Setup(l => l.Error(It.IsAny<Exception>(), It.IsAny<string>()));
            logger.Setup(l => l.Warning(It.IsAny<string>()));

            _svc = new SurveyQuestionManagementService(_questionRepository.Object, logger.Object);
        }

        [Fact]
        public void CreateNewQuestion_WhenPassedSurveyDto__CallsRepository_ReturnsUpdatedDto()
        {
            var dto = Mock.Of<ISurveyQuestion>();
            var result = _svc.CreateNewQuestion(_surveyId, dto).Result;

            _questionRepository.Verify(repo => repo.Create(It.IsAny<SurveyQuestion>()), Times.Exactly(1));
            Assert.Equal(_matchingGuid, result.Id);
        }

        [Fact]
        public void UpdateExistingQuestion_WhenNoSurveyQuestionFound_DoesNotCallRepository_ReturnsFalse()
        {
            var dto = Mock.Of<ISurveyQuestion>();
            var result = _svc.UpdateExistingQuestion(_nonMatchingGuid, dto).Result;

            _questionRepository.Verify(repo => repo.Update(It.IsAny<SurveyQuestion>()), Times.Never);
            Assert.False(result);
        }


        [Fact]
        public void UpdateExistingQuestion_WhenSurveyQuestionFound_CallsRepository_ReturnsTrue()
        {
            var dto = Mock.Of<ISurveyQuestion>();
            var result = _svc.UpdateExistingQuestion(_matchingGuid, dto).Result;

            _questionRepository.Verify(repo => repo.Update(It.IsAny<SurveyQuestion>()), Times.Exactly(1));
            Assert.True(result);
        }

        [Fact]
        public void GetQuestionById_WhenSurveyQuestionNotFound_CallsRepository_ReturnsNull()
        {
            var result = _svc.GetQuestionById(_nonMatchingGuid).Result;

            _questionRepository.Verify(repo => repo.Get(It.Is<Guid>(a => a.Equals(_nonMatchingGuid))), Times.Exactly(1));
            Assert.Null(result);
        }

        [Fact]
        public void GetQuestionById_WhenSurveyQuestionFound_CallsRepository_ReturnsSurveyDto()
        {
            var result = _svc.GetQuestionById(_matchingGuid).Result;

            _questionRepository.Verify(repo => repo.Get(It.Is<Guid>(a => a.Equals(_matchingGuid))), Times.Exactly(1));
            Assert.IsAssignableFrom<ISurveyQuestion>(result);
        }

        [Fact]
        public void GetQuestionsBySurveyId_CallsRepository_ReturnsAllSurveyDtoList()
        {
            var result = _svc.GetQuestionsBySurveyId(_surveyId).Result;

            _questionRepository.Verify(repo => repo.ListWithOptions(It.Is<Guid>(a=>a.Equals(_surveyId))), Times.Exactly(1));
            Assert.NotEmpty(result);
            Assert.True(result.Count == 1);

            Assert.Contains(result, r => r.Id == _matchingGuid);
            Assert.DoesNotContain(result, r => r.Id == _nonMatchingGuid);
        }

        [Fact]
        public void DeleteQuestionsById_WhenSurveyQuestionNotFound_DoesNotCallRepository_ReturnsFalse()
        {
            var result = _svc.DeleteQuestionsById(_nonMatchingGuid).Result;

            _questionRepository.Verify(repo => repo.Delete(It.IsAny<SurveyQuestion>()), Times.Never);
            Assert.False(result);
        }

        [Fact]
        public void DeleteQuestionsById_WhenSurveyQuestionFound_CallsRepository_ReturnsTrue()
        {
            var result = _svc.DeleteQuestionsById(_matchingGuid).Result;

            _questionRepository.Verify(repo => repo.Delete(It.IsAny<SurveyQuestion>()), Times.Exactly(1));
            Assert.True(result);
        }
    }
}
