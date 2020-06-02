using System;
using System.Collections.Generic;

namespace Comp.Survey.Core.Entities
{
    public class QuestionOption : EntityBase
    {
        public string Text { get; set; }

        public Guid SurveyQuestionId { get; set; }
        public virtual List<CompUserSurveyDetail> CompUserSurveyDetails { get; set; }
    }
}
