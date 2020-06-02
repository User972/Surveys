using System;
using System.ComponentModel.DataAnnotations;
using Comp.Survey.Core.Interfaces.DTO;

namespace Comp.Survey.App.Models
{
    [Serializable]
    public class QuestionOption : IQuestionOption 
    {
        public QuestionOption()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "The Option Name is required.")]
        [StringLength(2000, MinimumLength = 1, ErrorMessage = "Please provide a valid Option Description.")]
        public string Text { get; set; }
    }
}
