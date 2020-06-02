using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Comp.Survey.Core.Interfaces.DTO;

namespace Comp.Survey.App.Models
{
    public class SurveyQuestion : ISurveyQuestion
    {
        public SurveyQuestion()
        {
            Id = Guid.NewGuid();
            QuestionOptions = new List<IQuestionOption>();
        }

        public Guid Id { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDateTime { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "The Question Title is required.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Please provide a valid Question Title.")]
        public string Title { get; set; }


        [Required(AllowEmptyStrings = true, ErrorMessage = "The Question Sub-Title is required.")]
        [StringLength(100, MinimumLength = 0, ErrorMessage = "Please provide a valid Question Sub-Title.")]
        public string SubTitle { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "The Question type is required.")]
        public int QuestionType { get; set; }
        
        public IList<IQuestionOption> QuestionOptions { get; set; }
    }
}
