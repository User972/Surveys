using System;
using System.Collections.Generic;

namespace Comp.Survey.Core.Entities
{
    public class SurveyQuestion : EntityBase
    {
        public string CreatedBy { get; set; }
        public string CreatedDateTime { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public int QuestionType { get; set; }
  
        public Guid SurveyId { get; set; }
        public virtual List<QuestionOption> QuestionOptions { get; set; }
        public virtual List<CompUserSurveyDetail> CompUserSurveyDetails { get; set; }
    }
}