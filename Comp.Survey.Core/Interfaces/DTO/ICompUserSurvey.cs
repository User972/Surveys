using System;
using System.Collections.Generic;
using System.Text;

namespace Comp.Survey.Core.Interfaces.DTO
{
    public interface ICompUserSurvey
    {
        Guid Id { get; set; }
        string SubmissionTitle { get; set; }

        Guid CompUserId { get; set; }

        Guid SurveyId { get; set; }

        IList<ICompUserSurveyDetail> CompUserSurveyDetails { get; set; }
    }
}
