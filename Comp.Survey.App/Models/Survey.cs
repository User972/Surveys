using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Comp.Survey.Core.Interfaces.DTO;

namespace Comp.Survey.App.Models
{
    public class Survey : ISurvey
    {
        public Survey()
        {
            Id = Guid.NewGuid();
            SurveyQuestions = new List<ISurveyQuestion>();
        }

        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "The Survey Name is required.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Please provide a valid Survey Name.")]
        public string Name { get; set; }

        public IList<ISurveyQuestion> SurveyQuestions { get; set; }
    }
}
