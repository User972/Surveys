using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Comp.Survey.Core.Interfaces.DTO;

namespace Comp.Survey.Core.Interfaces.Services
{
    public interface ICompUserManagementService
    {
        Task<ICompUser> CreateNewCompUser(ICompUser user);
        Task<bool> UpdateExistingCompUser(Guid id, ICompUser user);
        Task<ICompUser> GetCompUserById(Guid id);
        Task<IList<ICompUser>> GetCompUsers();
        Task<IList<ICompUser>> GetCompUsersWithNameLike(string name);
        Task<bool> DeleteCompUserById(Guid id);
    }
}
