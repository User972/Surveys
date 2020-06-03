using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Comp.Survey.Core.Interfaces;
using Comp.Survey.Core.Interfaces.DTO;
using Comp.Survey.Core.Services;
using Moq;
using Serilog;
using Xunit;

namespace Comp.Survey.Core.Tests.Services
{
    public class CompUserSurveySurveyManagementServiceTests
    {
        private readonly Guid _guid;
        private readonly Guid _guid2;
        private readonly Guid _nonMatchingGuid;
        private readonly Mock<ICompUserSurveyRepository> _userSurveyRepository;
        private readonly Mock<ICompUserSurveyDetailRepository> _detailRepository;
        private CompUserSurveyManagementService _svc;
        private readonly Guid _compUserId;
        private readonly Guid _surveyId;

        public CompUserSurveySurveyManagementServiceTests()
        {
            _guid = Guid.NewGuid();
            _guid2 = Guid.NewGuid();
            _nonMatchingGuid = Guid.NewGuid();
            _compUserId = Guid.NewGuid();
            _surveyId = Guid.NewGuid();

            var user1 = new Entities.CompUserSurvey
            {
                Id = _guid
            };
            var user2 = new Entities.CompUserSurvey
            {
                Id = _guid2
            };

            var allUsers = new List<Entities.CompUserSurvey>() {user1, user2};
            var filteredUsers = new List<Entities.CompUserSurvey>() {user1};
            
            _userSurveyRepository = new Mock<ICompUserSurveyRepository>();
            _userSurveyRepository.Setup(repo =>
                repo.Create(It.IsAny<Entities.CompUserSurvey>())).ReturnsAsync(user1);

            _userSurveyRepository.Setup(repo =>
                repo.Get(It.Is<Guid>(a => a.Equals(_guid)))).ReturnsAsync(user1);

            _userSurveyRepository.Setup(repo =>
                repo.Get(It.Is<Guid>(a => a.Equals(_nonMatchingGuid)))).ReturnsAsync(null as Entities.CompUserSurvey);

            _userSurveyRepository.Setup(repo => repo.Update(It.IsAny<Entities.CompUserSurvey>()));

            _userSurveyRepository.Setup(repo => repo.List()).ReturnsAsync(allUsers);

            _userSurveyRepository.Setup(repo =>
                repo.List(It.IsAny<Expression<Func<Entities.CompUserSurvey, bool>>>())).ReturnsAsync(filteredUsers);

            _userSurveyRepository.Setup(repo => repo.Delete(It.IsAny<Entities.CompUserSurvey>()));

            _detailRepository = new Mock<ICompUserSurveyDetailRepository>();

            var logger = new Mock<ILogger>();
            logger.Setup(l => l.Error(It.IsAny<Exception>(), It.IsAny<string>()));
            logger.Setup(l => l.Warning(It.IsAny<string>()));

            _svc = new CompUserSurveyManagementService(_userSurveyRepository.Object, _detailRepository.Object, logger.Object);
        }

        [Fact]
        public void CreateNewCompUserSurvey_WhenPassedDto__CallsRepository_ReturnsUpdatedDto()
        {
            var dto = Mock.Of<ICompUserSurvey>();
            var result = _svc.CreateNewCompUserSurvey(dto).Result;

            _userSurveyRepository.Verify(repo => repo.Create(It.IsAny<Entities.CompUserSurvey>()), Times.Exactly(1));
            Assert.Equal(_guid, result.Id);
        }

        [Fact]
        public void UpdateExistingCompUserSurvey_WhenNoUserFound_DoesNotCallRepository_ReturnsFalse()
        {
            var dto = Mock.Of<ICompUserSurvey>();
            var result = _svc.UpdateExistingCompUserSurvey(_nonMatchingGuid, dto).Result;

            _userSurveyRepository.Verify(repo => repo.Update(It.IsAny<Entities.CompUserSurvey>()), Times.Never);
            Assert.False(result);
        }


        [Fact]
        public void UpdateExistingCompUserSurvey_WhenUserFound_CallsRepository_ReturnsTrue()
        {
            var dto = Mock.Of<ICompUserSurvey>();
            var result = _svc.UpdateExistingCompUserSurvey(_guid, dto).Result;

            _userSurveyRepository.Verify(repo => repo.Update(It.IsAny<Entities.CompUserSurvey>()), Times.Exactly(1));
            Assert.True(result);
        }

        [Fact]
        public void GetCompUserSurveyById_WhenNotFound_CallsRepository_ReturnsNull()
        {
            var result = _svc.GetCompUserSurveyById(_nonMatchingGuid).Result;

            _userSurveyRepository.Verify(repo => repo.Get(It.Is<Guid>(a => a.Equals(_nonMatchingGuid))), Times.Exactly(1));
            Assert.Null(result);
        }

        [Fact]
        public void GetCompUserSurveyById_WhenFound_CallsRepository_ReturnsDto()
        {
            var result = _svc.GetCompUserSurveyById(_guid).Result;

            _userSurveyRepository.Verify(repo => repo.Get(It.Is<Guid>(a => a.Equals(_guid))), Times.Exactly(1));
            Assert.IsAssignableFrom<ICompUserSurvey>(result);
        }

        [Fact]
        public void GetCompUserSurveys_CallsRepository_ReturnsAllDtoList()
        {
            var result = _svc.GetCompUserSurveys().Result;

            _userSurveyRepository.Verify(repo => repo.List(), Times.Exactly(1));
            Assert.NotEmpty(result);
            Assert.True(result.Count == 2);
            Assert.Contains(result, r => r.Id == _guid);
            Assert.Contains(result, r => r.Id == _guid2);
        }

        [Fact]
        public void GetCompUserSurveysByCompUserId_CallsRepository_ReturnsDtoList()
        {
            var result = _svc.GetCompUserSurveysByCompUserId(_compUserId).Result;

            _userSurveyRepository.Verify(repo => repo.List(It.IsAny<Expression<Func<Entities.CompUserSurvey, bool>>>()),
                Times.Exactly(1));
            Assert.NotEmpty(result);
            Assert.True(result.Count == 1);
            Assert.Contains(result, r => r.Id == _guid);
            Assert.DoesNotContain(result, r => r.Id == _guid2);
        }
        [Fact]
        public void GetCompUserSurveysBySurveyId_CallsRepository_ReturnsDtoList()
        {
            var result = _svc.GetCompUserSurveysBySurveyId(_surveyId).Result;

            _userSurveyRepository.Verify(repo => repo.List(It.IsAny<Expression<Func<Entities.CompUserSurvey, bool>>>()),
                Times.Exactly(1));
            Assert.NotEmpty(result);
            Assert.True(result.Count == 1);
            Assert.Contains(result, r => r.Id == _guid);
            Assert.DoesNotContain(result, r => r.Id == _guid2);
        }
        
        [Fact]
        public void DeleteCompUserSurveysByCompUserId_WhenFound_CallsRepository_ReturnsTrue()
        {
            var result = _svc.DeleteCompUserSurveysByCompUserId(_guid).Result;

            _userSurveyRepository.Verify(repo => repo.Delete(It.IsAny<Expression<Func<Entities.CompUserSurvey, bool>>>()), Times.Exactly(1));
            Assert.True(result);
        }

    }
}
