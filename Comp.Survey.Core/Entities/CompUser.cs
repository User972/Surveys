using System.Collections.Generic;

namespace Comp.Survey.Core.Entities
{
    public class CompUser : EntityBase
    {
        public string Name { get; set; }
        public virtual List<CompUserSurvey> CompUserSurveys { get; set; }
    }
}