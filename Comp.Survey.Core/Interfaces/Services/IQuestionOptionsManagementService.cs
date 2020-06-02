using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Comp.Survey.Core.Interfaces.DTO;

namespace Comp.Survey.Core.Interfaces.Services
{
    public interface IQuestionOptionsManagementService
    {
        Task<IQuestionOption> CreateNewOption(Guid questionId, IQuestionOption option);
        Task<bool> UpdateExistingOption(Guid id, IQuestionOption option);
        Task<IQuestionOption> GetOptionById(Guid id);
        Task<IList<IQuestionOption>> GetOptionsByQuestionId(Guid questionId);
        Task<bool> DeleteOptionById(Guid id);
        Task<bool> DeleteOptionsByQuestionId(Guid questionId);
    }
}
