using System;
using System.Threading.Tasks;
using Comp.Survey.App.Models;
using Comp.Survey.Core.Interfaces.Services;
using Comp.Survey.Core.Utilities;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Comp.Survey.App.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SurveysController : ControllerBase
    {
        private readonly ISurveyManagementService _surveyManagementService;
        private readonly ISurveyQuestionManagementService _surveyQuestionManagementService;
        private readonly IQuestionOptionsManagementService _optionsManagementService;
        private readonly ICompUserSurveyManagementService _surveySubmissionService;
        private readonly ILogger _logger;

        public SurveysController(ISurveyManagementService surveyManagementService, ISurveyQuestionManagementService surveyQuestionManagementService, IQuestionOptionsManagementService optionsManagementService, ICompUserSurveyManagementService surveySubmissionService, ILogger logger)
        {
            Ensure.ArgumentNotNull(surveyManagementService, nameof(surveyManagementService));
            Ensure.ArgumentNotNull(surveyQuestionManagementService, nameof(surveyQuestionManagementService));
            _surveyManagementService = surveyManagementService;
            _surveyQuestionManagementService = surveyQuestionManagementService;
            _optionsManagementService = optionsManagementService;
            _surveySubmissionService = surveySubmissionService;
            _logger = logger;
        }


        #region ACTIONS - Queries

        #region Surveys

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery]string name)
        {
            var response = string.IsNullOrWhiteSpace(name)
             ? await _surveyManagementService.GetSurveys()
             : await _surveyManagementService.GetSurveysWithNameLike(name);

            return Ok(response);
        }

        [Route("{id}")]
        [HttpGet]
        [ActionName("GetSurveyAsync")]
        public async Task<IActionResult> GetSurveyAsync(Guid id)
        {
            Ensure.GuidNotEmpty(id, nameof(id));

            var survey = await _surveyManagementService.GetSurveyById(id);
            if (survey == null)
            {
                return NotFound();
            }

            return Ok(survey);
        }

        #endregion Surveys

        #region Questions

        [HttpGet]
        [Route("{id}/questions")]
        public async Task<IActionResult> GetAllQuestionsAsync(Guid id)
        {
            Ensure.GuidNotEmpty(id, nameof(id));
            var response = await _surveyQuestionManagementService.GetQuestionsBySurveyId(id);
            return Ok(response);
        }

        [Route("{id}/questions/{questionId}")]
        [HttpGet]
        [ActionName("GetSurveyQuestionAsync")]
        public async Task<IActionResult> GetSurveyQuestionAsync(Guid id, Guid questionId)
        {
            Ensure.GuidNotEmpty(id, nameof(id));
            Ensure.GuidNotEmpty(questionId, nameof(questionId));

            var surveyQuestion = await _surveyQuestionManagementService.GetQuestionById(questionId);
            if (surveyQuestion == null)
            {
                return NotFound();
            }

            return Ok(surveyQuestion);
        }

        #endregion Questions

        #region Options

        [HttpGet]
        [Route("{id}/questions/{questionId}/options")]
        public async Task<IActionResult> GetAllQuestionsAsync(Guid id, Guid questionId)
        {
            Ensure.GuidNotEmpty(id, nameof(id));
            Ensure.GuidNotEmpty(questionId, nameof(questionId));
            var response = await _optionsManagementService.GetOptionsByQuestionId(questionId);
            return Ok(response);
        }

        [Route("{id}/questions/{questionId}/options/{optionId}")]
        [HttpGet]
        [ActionName("GetQuestionOptionAsync")]
        public async Task<IActionResult> GetQuestionOptionAsync(Guid id, Guid questionId, Guid optionId)
        {
            Ensure.GuidNotEmpty(id, nameof(id));
            Ensure.GuidNotEmpty(questionId, nameof(questionId));
            Ensure.GuidNotEmpty(optionId, nameof(optionId));

            var option = await _optionsManagementService.GetOptionById(optionId);
            if (option == null)
            {
                return NotFound();
            }

            return Ok(option);
        }

        #endregion Questions
        #endregion  ACTIONS - Queries

        #region ACTIONS - Commands

        #region Surveys
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody]Models.Survey survey)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdSurvey = await _surveyManagementService.CreateNewSurvey(survey);
            return CreatedAtAction(nameof(GetSurveyAsync), new { id = createdSurvey.Id }, createdSurvey);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, Models.Survey survey)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isExistingSurvey = await _surveyManagementService.UpdateExistingSurvey(id, survey);
            if (!isExistingSurvey)
            {
                _logger.Warning("UpdateAsync - Survey id {id} not found", id);
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var isExistingSurvey = await _surveyManagementService.DeleteSurveyById(id);
            if (!isExistingSurvey)
            {
                _logger.Warning("DeleteAsync - Survey id {id} not found", id);
                return NotFound();
            }

            return Accepted();
        }

        #endregion Surveys

        #region Questions
        [HttpPost]
        [Route("{id}/questions")]
        public async Task<IActionResult> CreateQuestionAsync(Guid id, SurveyQuestion question)
        {
            Ensure.GuidNotEmpty(id, nameof(id));
            Ensure.ArgumentNotNull(question, nameof(question));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdQuestion = await _surveyQuestionManagementService.CreateNewQuestion(id, question);
            return CreatedAtAction(nameof(GetSurveyQuestionAsync), new { id, questionId = createdQuestion.Id }, createdQuestion);
        }

        [HttpPut]
        [Route("{id}/questions/{questionId}")]
        public async Task<IActionResult> UpdateQuestionAsync(Guid id, Guid questionId, SurveyQuestion question)
        {
            Ensure.GuidNotEmpty(id, nameof(id));
            Ensure.GuidNotEmpty(questionId, nameof(questionId));
            Ensure.ArgumentNotNull(question, nameof(question));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var isExistingQuestion = await _surveyQuestionManagementService.UpdateExistingQuestion(questionId, question);
            if (!isExistingQuestion)
            {
                _logger.Warning("UpdateQuestionAsync - Survey id {id} and Question Id {questionId} not found", id, questionId);
                return NotFound();
            }
            return Ok();
        }

        [HttpDelete]
        [Route("{id}/questions/{questionId}")]
        public async Task<IActionResult> DeleteQuestionAsync(Guid id, Guid questionId)
        {
            Ensure.GuidNotEmpty(id, nameof(id));
            Ensure.GuidNotEmpty(questionId, nameof(questionId));

            var isExistingQuestion = await _surveyQuestionManagementService.DeleteQuestionsById(questionId);
            if (!isExistingQuestion)
            {
                _logger.Warning($"DeleteQuestionAsync - Survey id {id} and Question Id {questionId} not found");
                return NotFound();
            }

            return Accepted();
        }
        #endregion Questions

        #region Options
        [HttpPost]
        [Route("{id}/questions/{questionId}/options")]
        public async Task<IActionResult> CreateOptionAsync(Guid id, Guid questionId, QuestionOption option)
        {
            Ensure.GuidNotEmpty(id, nameof(id));
            Ensure.GuidNotEmpty(questionId, nameof(questionId));
            Ensure.ArgumentNotNull(option, nameof(option));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdOption = await _optionsManagementService.CreateNewOption(questionId, option);
            return CreatedAtAction(nameof(GetQuestionOptionAsync), new { id, questionId, optionId = createdOption.Id }, createdOption);
        }

        [HttpPut]
        [Route("{id}/questions/{questionId}/options/{optionId}")]
        public async Task<IActionResult> UpdateOptionAsync(Guid id, Guid questionId, Guid optionId, QuestionOption option)
        {
            Ensure.GuidNotEmpty(id, nameof(id));
            Ensure.GuidNotEmpty(questionId, nameof(questionId));
            Ensure.GuidNotEmpty(optionId, nameof(optionId));
            Ensure.ArgumentNotNull(option, nameof(option));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var isExistingOption = await _optionsManagementService.UpdateExistingOption(optionId, option);
            if (!isExistingOption)
            {
                _logger.Warning("UpdateOptionAsync - Survey id {id}, Question Id {questionId} and Option Id {optionId} not found", id, questionId, optionId);
                return NotFound();
            }
            return Ok();
        }

        [HttpDelete]
        [Route("{id}/questions/{questionId}/options/{optionId}")]
        public async Task<IActionResult> DeleteOptionAsync(Guid id, Guid questionId, Guid optionId)
        {
            Ensure.GuidNotEmpty(id, nameof(id));
            Ensure.GuidNotEmpty(questionId, nameof(questionId));
            Ensure.GuidNotEmpty(optionId, nameof(optionId));

            var isExistingQuestion = await _optionsManagementService.DeleteOptionById(optionId);
            if (!isExistingQuestion)
            {
                _logger.Warning($"DeleteOptionAsync - Survey id {id}, Question Id {questionId} and Option Id {optionId} not found");
                return NotFound();
            }

            return Accepted();
        }
        #endregion Options

        #region Survey Submission
        [HttpPost]
        [Route("{id}/submission")]
        public async Task<IActionResult> CreateSubmissionAsync(Guid id, [FromBody]SurveySubmissionRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var s = new CompUserSurvey()
            {
                SurveyId = id,
                CompUserId = request.CompUserId,
                SubmissionTitle = request.SubmissionTitle
            };
            foreach (var detail in request.CompUserSurveyDetails)
            {
                s.CompUserSurveyDetails.Add(detail);
            }
            var createdSurvey = await _surveySubmissionService.CreateNewCompUserSurvey(s);
            return CreatedAtAction(nameof(GetSubmissionAsync), new { id = createdSurvey.Id }, createdSurvey);
        }

        [HttpGet]
        [Route("{id}/submission/{submissionId}")]
        [ActionName("GetSubmissionAsync")]
        public async Task<IActionResult> GetSubmissionAsync(Guid id, Guid submissionId)
        {
            var survey = await _surveySubmissionService.GetCompUserSurveyById(submissionId);
            if (survey == null)
            {
                return NotFound();
            }

            return Ok(survey);
        }

        [HttpGet]
        [Route("{id}/submission/")]
        [ActionName("GetSubmissionAsync")]
        public async Task<IActionResult> GetSubmittedSurveysAsync(Guid id)
        {
            var surveys = await _surveySubmissionService.GetCompUserSurveysBySurveyId(id);
            if (surveys == null)
            {
                return NotFound();
            }

            return Ok(surveys);
        }

        #endregion Survey Submission

        #endregion ACTIONS - Commands
    }
}