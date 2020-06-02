using System.Collections.Generic;

namespace Comp.Survey.Core.Entities
{
    public class Survey : EntityBase
    {
        public string Name { get; set; }
        public virtual List<SurveyQuestion> SurveyQuestions { get; set; }
        public virtual List<CompUserSurvey> CompUserSurveys { get; set; }
    }
}