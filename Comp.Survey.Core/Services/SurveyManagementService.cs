using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Comp.Survey.Core.Entities;
using Comp.Survey.Core.Interfaces;
using Comp.Survey.Core.Interfaces.DTO;
using Comp.Survey.Core.Interfaces.Services;
using Serilog;

namespace Comp.Survey.Core.Services
{
    public class SurveyManagementService : ISurveyManagementService
    {
        private readonly ISurveyRepository _surveyRepository;
        private readonly ILogger _logger;

        public SurveyManagementService(ISurveyRepository surveyRepository, ILogger logger)
        {
            _surveyRepository = surveyRepository;
            _logger = logger;
        }

        public async Task<ISurvey> CreateNewSurvey(ISurvey surveyDto)
        {
            try
            {
                var survey = Mappings.Mapper.Map<Entities.Survey>(surveyDto);
                survey.Id = Guid.NewGuid();
                var preservedEntity = await _surveyRepository.Create(survey);
                surveyDto.Id = preservedEntity.Id;
                return surveyDto;
            }
            catch (Exception e)
            {
                _logger.Error(e, "SurveyManagementService.CreateNewSurvey");
                throw;
            }
        }

        public async Task<bool> UpdateExistingSurvey(Guid id, ISurvey surveyDto)
        {
            try
            {
                var survey = await _surveyRepository.Get(id);
                if (survey == null)
                    return false;

                Mappings.Mapper.Map(surveyDto, survey);
                await _surveyRepository.Update(survey);
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e, "SurveyManagementService.UpdateExistingSurvey");
                throw;
            }
        }

        public async Task<ISurvey> GetSurveyById(Guid id)
        {
            var survey = await _surveyRepository.GetWithQuestions(id);
            return Mappings.Mapper.Map<ISurvey>(survey);
        }

        public async Task<IList<ISurvey>> GetSurveys()
        {
            var surveys = await _surveyRepository.List();
            return Mappings.Mapper.Map<IList<ISurvey>>(surveys);
        }

        public async Task<IList<ISurvey>> GetSurveysWithNameLike(string name)
        {
            var surveys = await _surveyRepository.List(p => p.Name.Contains(name, StringComparison.InvariantCultureIgnoreCase));
            return Mappings.Mapper.Map<IList<ISurvey>>(surveys);
        }

        public async Task<bool> DeleteSurveyById(Guid id)
        {
            try
            {
                var survey = await _surveyRepository.Get(id);
                if (survey == null)
                    return false;
                await _surveyRepository.Delete(survey);
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e, "SurveyManagementService.DeleteSurveyById");
                throw;
            }
        }
    }
}
