using System;
using System.Threading.Tasks;
using Comp.Survey.Core.Interfaces.Services;
using Comp.Survey.Core.Utilities;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Comp.Survey.App.Controllers
{
    [Route("api/v2/surveys")]
    [ApiController]
    public class SurveysV2Controller : ControllerBase
    {
        private readonly ISurveyManagementService _surveyManagementService;
        private readonly ILogger _logger;

        public SurveysV2Controller(ISurveyManagementService surveyManagementService, ILogger logger)
        {
            _surveyManagementService = surveyManagementService;
            _logger = logger;
        }

        #region Queries

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

        #endregion Queries

        #region Commands
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody]Models.SurveyCreationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var survey = Mappings.Mappings.Mapper.Map<Models.Survey>(request);
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

        #endregion Commands
    }
}