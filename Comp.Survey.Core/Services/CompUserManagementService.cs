using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Comp.Survey.Core.Entities;
using Comp.Survey.Core.Interfaces;
using Comp.Survey.Core.Interfaces.DTO;
using Comp.Survey.Core.Interfaces.Services;
using Serilog;

namespace Comp.Survey.Core.Services
{
    public class CompUserManagementService : ICompUserManagementService
    {
        private readonly ICompUserRepository _userRepository;
        private readonly ILogger _logger;

        public CompUserManagementService(ICompUserRepository userRepository, ILogger logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<ICompUser> CreateNewCompUser(ICompUser user)
        {
            try
            {
                var compUser = Mappings.Mapper.Map<CompUser>(user);
                compUser.Id = Guid.NewGuid();
               var persistedEntity = await _userRepository.Create(compUser);
               user.Id = persistedEntity.Id;
                return user;
            }
            catch (Exception e)
            {
                _logger.Error(e, "CompUserManagementService.CreateNewCompUser");
                throw;
            }
        }

        public async Task<bool> UpdateExistingCompUser(Guid id, ICompUser user)
        {
            try
            {
                var compUser = await _userRepository.Get(id);
                if (compUser == null)
                    return false;
                Mappings.Mapper.Map<ICompUser, CompUser>(user, compUser);

                await _userRepository.Update(compUser);
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e, "CompUserManagementService.UpdateExistingCompUser");
                throw;
            }
        }

        public async Task<ICompUser> GetCompUserById(Guid id)
        {
            var user = await _userRepository.Get(id);
            return Mappings.Mapper.Map<ICompUser>(user);
        }

        public async Task<IList<ICompUser>> GetCompUsers()
        {
            var users = await _userRepository.List();
            return Mappings.Mapper.Map<IList<ICompUser>>(users);
        }

        public async Task<IList<ICompUser>> GetCompUsersWithNameLike(string name)
        {
            var users = await _userRepository.List(s => s.Name.Contains(name, StringComparison.CurrentCultureIgnoreCase));
            return Mappings.Mapper.Map<IList<ICompUser>>(users);
        }

        public async Task<bool> DeleteCompUserById(Guid id)
        {
            try
            {
                var user = await _userRepository.Get(id);
                if (user == null)
                    return false;

                await _userRepository.Delete(user);
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e, "CompUserManagementService.DeleteCompUserById");
                throw;
            }
        }
    }
}
