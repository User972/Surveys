using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Comp.Survey.Core.Entities;
using Comp.Survey.Core.Interfaces;
using Comp.Survey.Core.Interfaces.DTO;
using Comp.Survey.Core.Services;
using Moq;
using Serilog;
using Xunit;

namespace Comp.Survey.Core.Tests.Services
{
    public class QuestionOptionManagementServiceTests
    {
        private readonly Mock<IQuestionOptionRepository> _optionRepository;
        private readonly QuestionOptionManagementService _svc;
        private readonly Guid _guid;
        private readonly Guid _guid2;
        private readonly Guid _nonMatchingGuid;
        private readonly Guid _questionId;

        public QuestionOptionManagementServiceTests()
        {
            _guid = Guid.NewGuid();
            _guid2 = Guid.NewGuid();
            _nonMatchingGuid = Guid.NewGuid();
            _questionId = Guid.NewGuid();

            var option1 = new Entities.QuestionOption
            {
                Id = _guid
            };
            var option2 = new Entities.QuestionOption
            {
                Id = _guid2
            };
            Expression<Func<Entities.QuestionOption, bool>> ex = o => o.SurveyQuestionId == _questionId;
            var allOptions = new List<Entities.QuestionOption>() { option1, option2 };
            var filteredOptions = new List<Entities.QuestionOption>() { option1 };

            _optionRepository = new Mock<IQuestionOptionRepository>();
            _optionRepository.Setup(repo =>
                repo.Create(It.IsAny<Entities.QuestionOption>())).ReturnsAsync(option1);

            _optionRepository.Setup(repo =>
                repo.Get(It.Is<Guid>(a => a.Equals(_guid)))).ReturnsAsync(option1);

            _optionRepository.Setup(repo =>
                repo.Get(It.Is<Guid>(a => a.Equals(_nonMatchingGuid)))).ReturnsAsync(null as Entities.QuestionOption);

            _optionRepository.Setup(repo => repo.Update(It.IsAny<Entities.QuestionOption>()));

            _optionRepository.Setup(repo => repo.List()).ReturnsAsync(allOptions);

            _optionRepository.Setup(repo =>
                repo.List(It.IsAny<Expression<Func<Entities.QuestionOption, bool>>>())).ReturnsAsync(filteredOptions);

            _optionRepository.Setup(repo => repo.Delete(It.IsAny<Entities.QuestionOption>()));

            var logger = new Mock<ILogger>();
            logger.Setup(l => l.Error(It.IsAny<Exception>(), It.IsAny<string>()));
            logger.Setup(l => l.Warning(It.IsAny<string>()));

            _svc = new QuestionOptionManagementService(_optionRepository.Object, logger.Object);

        }
      
        [Fact]
        public void CreateNewOption_WhenPassedOptionDto__CallsRepository_ReturnsUpdatedDto()
        {
            var dto = Mock.Of<IQuestionOption>();
            var result = _svc.CreateNewOption(_questionId, dto).Result;

            _optionRepository.Verify(repo => repo.Create(It.IsAny<Entities.QuestionOption>()), Times.Exactly(1));
            Assert.Equal(_guid, result.Id);
        }

        [Fact]
        public void UpdateExistingOption_WhenNoOptionFound_DoesNotCallRepository_ReturnsFalse()
        {
            var dto = Mock.Of<IQuestionOption>();
            var result = _svc.UpdateExistingOption(_nonMatchingGuid, dto).Result;

            _optionRepository.Verify(repo => repo.Update(It.IsAny<Entities.QuestionOption>()), Times.Never);
            Assert.False(result);
        }


        [Fact]
        public void UpdateExistingOption_WhenOptionFound_CallsRepository_ReturnsTrue()
        {
            var dto = Mock.Of<IQuestionOption>();
            var result = _svc.UpdateExistingOption(_guid, dto).Result;

            _optionRepository.Verify(repo => repo.Update(It.IsAny<Entities.QuestionOption>()), Times.Exactly(1));
            Assert.True(result);
        }

        [Fact]
        public void GetOptionById_WhenOptionNotFound_CallsRepository_ReturnsNull()
        {
            var result = _svc.GetOptionById(_nonMatchingGuid).Result;

            _optionRepository.Verify(repo => repo.Get(It.Is<Guid>(a => a.Equals(_nonMatchingGuid))), Times.Exactly(1));
            Assert.Null(result);
        }

        [Fact]
        public void GetOptionById_WhenFound_CallsRepository_ReturnsDto()
        {
            var result = _svc.GetOptionById(_guid).Result;

            _optionRepository.Verify(repo => repo.Get(It.Is<Guid>(a => a.Equals(_guid))), Times.Exactly(1));
            Assert.IsAssignableFrom<IQuestionOption>(result);
        }

        [Fact]
        public void GetOptionsByQuestionId_CallsRepository_ReturnsFilteredSurveyDtoList()
        {
            var result = _svc.GetOptionsByQuestionId(_questionId).Result;

            _optionRepository.Verify(repo => repo.List(It.IsAny<Expression<Func<Entities.QuestionOption, bool>>>()), Times.Exactly(1));
            Assert.NotEmpty(result);
            Assert.True(result.Count == 1);
            Assert.Contains(result, r => r.Id == _guid);
            Assert.DoesNotContain(result, r => r.Id == _guid2);
        }

        [Fact]
        public void DeleteOptionById_WhenNotFound_DoesNotCallRepository_ReturnsFalse()
        {
            var result = _svc.DeleteOptionById(_nonMatchingGuid).Result;

            _optionRepository.Verify(repo => repo.Delete(It.IsAny<Entities.QuestionOption>()), Times.Never);
            Assert.False(result);
        }

        [Fact]
        public void DeleteOptionById_WhenFound_CallsRepository_ReturnsTrue()
        {
            var result = _svc.DeleteOptionById(_guid).Result;

            _optionRepository.Verify(repo => repo.Delete(It.IsAny<Entities.QuestionOption>()), Times.Exactly(1));
            Assert.True(result);
        }

        [Fact]
        public void DeleteOptionsByQuestionId_CallsRepository_ReturnsTrue()
        {
            var result = _svc.DeleteOptionsByQuestionId(_questionId).Result;

            _optionRepository.Verify(repo => repo.Delete(It.IsAny<Expression<Func<Entities.QuestionOption, bool>>>()), Times.Exactly(1));
            Assert.True(result);
        }
    }
}
