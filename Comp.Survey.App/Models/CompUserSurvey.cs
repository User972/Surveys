using System;
using System.Collections.Generic;
using Comp.Survey.Core.Interfaces.DTO;

namespace Comp.Survey.App.Models
{
    public class CompUserSurvey : ICompUserSurvey
    {
        public CompUserSurvey()
        {
            Id = Guid.NewGuid();
            CompUserSurveyDetails = new List<ICompUserSurveyDetail>();
        }
        public CompUserSurvey(IList<ICompUserSurveyDetail> details)
        {
            Id = Guid.NewGuid();
            CompUserSurveyDetails = details;
        }

        public Guid Id { get; set; }
        public string SubmissionTitle { get; set; }
        public Guid CompUserId { get; set; }
        public Guid SurveyId { get; set; }
        public IList<ICompUserSurveyDetail> CompUserSurveyDetails { get; set; }
    }
}