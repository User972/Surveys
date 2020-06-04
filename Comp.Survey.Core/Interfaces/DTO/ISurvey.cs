using System;
using System.Collections.Generic;

namespace Comp.Survey.Core.Interfaces.DTO
{
    public interface ISurvey
    {
        Guid Id { get; set; }

        string Name { get; set; }

        IList<ISurveyQuestion> SurveyQuestions { get; set; }
    }
}
