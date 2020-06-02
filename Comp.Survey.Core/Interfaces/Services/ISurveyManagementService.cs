using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Comp.Survey.Core.Interfaces.DTO;

namespace Comp.Survey.Core.Interfaces.Services
{
    public interface ISurveyManagementService
    {
        Task<ISurvey> CreateNewSurvey(ISurvey survey);
        Task<bool> UpdateExistingSurvey(Guid id, ISurvey survey);
        Task<ISurvey> GetSurveyById(Guid id);
        Task<IList<ISurvey>> GetSurveys();
        Task<IList<ISurvey>> GetSurveysWithNameLike(string name);
        Task<bool> DeleteSurveyById(Guid id);
    }
}
