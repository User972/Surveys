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
    public class CompUserManagementServiceTests
    {
        private readonly Guid _guid;
        private readonly Guid _guid2;
        private readonly Guid _nonMatchingGuid;
        private readonly CompUserManagementService _svc;
        private readonly Mock<ICompUserRepository> _userRepository;
        private readonly string _userName = "userName";
        public CompUserManagementServiceTests()
        {
            _guid = Guid.NewGuid();
            _guid2 = Guid.NewGuid();
            _nonMatchingGuid = Guid.NewGuid();
            

            var user1 = new Entities.CompUser
            {
                Id = _guid
            };
            var user2 = new Entities.CompUser
            {
                Id = _guid2
            };

            var allUsers = new List<Entities.CompUser>() { user1, user2 };
            var filteredUsers = new List<Entities.CompUser>() { user1 };


      
            _userRepository = new Mock<ICompUserRepository>();
            _userRepository.Setup(repo =>
                repo.Create(It.IsAny<Entities.CompUser>())).ReturnsAsync(user1);

            _userRepository.Setup(repo =>
                repo.Get(It.Is<Guid>(a => a.Equals(_guid)))).ReturnsAsync(user1);

            _userRepository.Setup(repo =>
                repo.Get(It.Is<Guid>(a => a.Equals(_nonMatchingGuid)))).ReturnsAsync(null as Entities.CompUser);

            _userRepository.Setup(repo => repo.Update(It.IsAny<Entities.CompUser>()));

            _userRepository.Setup(repo => repo.List()).ReturnsAsync(allUsers);

            _userRepository.Setup(repo =>
                repo.List(It.IsAny<Expression<Func<Entities.CompUser, bool>>>())).ReturnsAsync(filteredUsers);

            _userRepository.Setup(repo => repo.Delete(It.IsAny<Entities.CompUser>()));

            var logger = new Mock<ILogger>();
            logger.Setup(l => l.Error(It.IsAny<Exception>(), It.IsAny<string>()));
            logger.Setup(l => l.Warning(It.IsAny<string>()));

            _svc = new CompUserManagementService(_userRepository.Object, logger.Object);
        }

        [Fact]
        public void CreateNewOption_WhenPassedDto__CallsRepository_ReturnsUpdatedDto()
        {
            var dto = Mock.Of<ICompUser>();
            var result = _svc.CreateNewCompUser(dto).Result;

            _userRepository.Verify(repo => repo.Create(It.IsAny<Entities.CompUser>()), Times.Exactly(1));
            Assert.Equal(_guid, result.Id);
        }

        [Fact]
        public void UpdateExistingCompUser_WhenNoUserFound_DoesNotCallRepository_ReturnsFalse()
        {
            var dto = Mock.Of<ICompUser>();
            var result = _svc.UpdateExistingCompUser(_nonMatchingGuid, dto).Result;

            _userRepository.Verify(repo => repo.Update(It.IsAny<Entities.CompUser>()), Times.Never);
            Assert.False(result);
        }


        [Fact]
        public void UpdateExistingCompUser_WhenUserFound_CallsRepository_ReturnsTrue()
        {
            var dto = Mock.Of<ICompUser>();
            var result = _svc.UpdateExistingCompUser(_guid, dto).Result;

            _userRepository.Verify(repo => repo.Update(It.IsAny<Entities.CompUser>()), Times.Exactly(1));
            Assert.True(result);
        }

        [Fact]
        public void GetCompUserById_WhenNotFound_CallsRepository_ReturnsNull()
        {
            var result = _svc.GetCompUserById(_nonMatchingGuid).Result;

            _userRepository.Verify(repo => repo.Get(It.Is<Guid>(a => a.Equals(_nonMatchingGuid))), Times.Exactly(1));
            Assert.Null(result);
        }

        [Fact]
        public void GetCompUserById_WhenFound_CallsRepository_ReturnsDto()
        {
            var result = _svc.GetCompUserById(_guid).Result;

            _userRepository.Verify(repo => repo.Get(It.Is<Guid>(a => a.Equals(_guid))), Times.Exactly(1));
            Assert.IsAssignableFrom<ICompUser>(result);
        }

        [Fact]
        public void GetCompUsers_CallsRepository_ReturnsAllDtoList()
        {
            var result = _svc.GetCompUsers().Result;

            _userRepository.Verify(repo => repo.List(), Times.Exactly(1));
            Assert.NotEmpty(result);
            Assert.True(result.Count == 2);
            Assert.Contains(result, r => r.Id == _guid);
            Assert.Contains(result, r => r.Id == _guid2);
        }

        [Fact]
        public void GetCompUsersWithNameLike_CallsRepository_ReturnsFilteredDtoList()
        {
            var result = _svc.GetCompUsersWithNameLike(_userName).Result;

            _userRepository.Verify(repo => repo.List(It.IsAny<Expression<Func<Entities.CompUser, bool>>>()), Times.Exactly(1));
            Assert.NotEmpty(result);
            Assert.True(result.Count == 1);
            Assert.Contains(result, r => r.Id == _guid);
            Assert.DoesNotContain(result, r => r.Id == _guid2);
        }

        [Fact]
        public void DeleteCompUserById_WhenNotFound_DoesNotCallRepository_ReturnsFalse()
        {
            var result = _svc.DeleteCompUserById(_nonMatchingGuid).Result;

            _userRepository.Verify(repo => repo.Delete(It.IsAny<Entities.CompUser>()), Times.Never);
            Assert.False(result);
        }

        [Fact]
        public void DeleteCompUserById_WhenFound_CallsRepository_ReturnsTrue()
        {
            var result = _svc.DeleteCompUserById(_guid).Result;

            _userRepository.Verify(repo => repo.Delete(It.IsAny<Entities.CompUser>()), Times.Exactly(1));
            Assert.True(result);
        }
    }
}
