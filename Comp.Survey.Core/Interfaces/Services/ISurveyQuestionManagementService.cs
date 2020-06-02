using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Comp.Survey.Core.Interfaces.DTO;

namespace Comp.Survey.Core.Interfaces.Services
{
    public interface ISurveyQuestionManagementService
    {
        Task<ISurveyQuestion> CreateNewQuestion(Guid surveyId, ISurveyQuestion question);
        Task<bool> UpdateExistingQuestion(Guid id, ISurveyQuestion option);
        Task<ISurveyQuestion> GetQuestionById(Guid id);
        Task<IList<ISurveyQuestion>> GetQuestionsBySurveyId(Guid surveyId);
        Task<bool> DeleteQuestionsById(Guid id);
        Task<bool> DeleteQuestionsBySurveyId(Guid surveyId);
    }
}
