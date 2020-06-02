using System;

namespace Comp.Survey.Core.Entities
{
    public class CompUserSurveyDetail : EntityBase
    {
        public string CompUserSurvey { get; set; }

        public Guid CompUserSurveyId { get; set; }

        public Guid SurveyQuestionId { get; set; }

        public Guid SelectedOptionId { get; set; }

        public string SelectedOptionValue { get; set; }
    }
}
