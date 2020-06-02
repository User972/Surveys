using System;

namespace Comp.Survey.Core.Interfaces.DTO
{
    public interface ICompUserSurveyDetail
    {
        Guid Id { get; set; }
        
        string CompUserSurvey { get; set; }

        Guid CompUserSurveyId { get; set; }

        Guid SurveyQuestionId { get; set; }

        Guid SelectedOptionId { get; set; }

        // In case question type has different option style i.e. radio, textbox or anything
        // By default, we will store option text
        string SelectedOptionValue { get; set; }
    }
}