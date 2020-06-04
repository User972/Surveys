using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Comp.Survey.App.Models
{
    public class SurveySubmissionRequest
    {
        public string SubmissionTitle { get; set; }
        public Guid CompUserId { get; set; }
        public IList<CompUserSurveyDetail> CompUserSurveyDetails { get; set; }
    }
    public class SurveyCreationRequest
    {
        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "The Survey Name is required.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Please provide a valid Survey Name.")]
        public string Name { get; set; }
        public IList<SurveyQuestionModel> SurveyQuestions { get; set; }
    }

    public class SurveyQuestionModel
    {
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

        public IList<QuestionOption> QuestionOptions { get; set; }
    }
}