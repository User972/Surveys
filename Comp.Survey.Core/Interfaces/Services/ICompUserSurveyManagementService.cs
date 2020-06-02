using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Comp.Survey.Core.Interfaces.DTO;

namespace Comp.Survey.Core.Interfaces.Services
{
    public interface ICompUserSurveyManagementService
    {
        Task<ICompUserSurvey> CreateNewCompUserSurvey(ICompUserSurvey survey);
        Task<bool> UpdateExistingCompUserSurvey(Guid id, ICompUserSurvey survey);
        Task<ICompUserSurvey> GetCompUserSurveyById(Guid id);
        Task<IList<ICompUserSurvey>> GetCompUserSurveys();
        Task<IList<ICompUserSurvey>> GetCompUserSurveysByCompUserId(Guid userId);
        Task<IList<ICompUserSurvey>> GetCompUserSurveysBySurveyId(Guid surveyId);
        Task<bool> DeleteCompUserSurveysByCompUserId(Guid userId);
    }
}
