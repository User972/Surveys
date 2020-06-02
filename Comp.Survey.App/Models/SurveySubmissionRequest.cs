using System;
using System.Collections.Generic;

namespace Comp.Survey.App.Models
{
    public class SurveySubmissionRequest
    {
        public string SubmissionTitle { get; set; }
        public Guid CompUserId { get; set; }
        public IList<CompUserSurveyDetail> CompUserSurveyDetails { get; set; }
    }
}