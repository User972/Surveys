using System;
using Comp.Survey.Core.Interfaces.DTO;

namespace Comp.Survey.App.Models
{
    public class CompUserSurveyDetail : ICompUserSurveyDetail
    {
        public CompUserSurveyDetail()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public string CompUserSurvey { get; set; }
        public Guid CompUserSurveyId { get; set; }
        public Guid SurveyQuestionId { get; set; }
        public Guid SelectedOptionId { get; set; }
        public string SelectedOptionValue { get; set; }
    }
}