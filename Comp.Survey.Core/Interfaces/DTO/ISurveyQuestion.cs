using System;
using System.Collections.Generic;

namespace Comp.Survey.Core.Interfaces.DTO
{
    public interface ISurveyQuestion
    {
        Guid Id { get; set; }
        string CreatedBy { get; set; }
        string CreatedDateTime { get; set; }
        string Title { get; set; }
        string SubTitle { get; set; }
        int QuestionType { get; set; }
        IList<IQuestionOption> QuestionOptions { get; set; }
    }
}
