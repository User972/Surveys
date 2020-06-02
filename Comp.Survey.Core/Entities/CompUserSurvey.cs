using System;
using System.Collections.Generic;
using System.Text;

namespace Comp.Survey.Core.Entities
{
    public class CompUserSurvey : EntityBase
    {
        public string SubmissionTitle { get; set; }

        public Guid CompUserId { get; set; }

        public Guid SurveyId { get; set; }
        public virtual List<CompUserSurveyDetail> CompUserSurveyDetails { get; set; }
    }
}
