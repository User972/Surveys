using Comp.Survey.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Comp.Survey.Core.Entities;
using Comp.Survey.Core.Interfaces;
using Comp.Survey.Core.Interfaces.DTO;
using Serilog;

namespace Comp.Survey.Core.Services
{
    public class CompUserSurveyManagementService : ICompUserSurveyManagementService
    {
        private readonly ICompUserSurveyRepository _compUserSurveyRepository;
        private readonly ICompUserSurveyDetailRepository _detailRepository;
        private readonly ILogger _logger;

        public CompUserSurveyManagementService(ICompUserSurveyRepository compUserSurveyRepository,ICompUserSurveyDetailRepository detailRepository,  ILogger logger)
        {
            _compUserSurveyRepository = compUserSurveyRepository;
            _detailRepository = detailRepository;
            _logger = logger;
        }

        public async Task<ICompUserSurvey> CreateNewCompUserSurvey(ICompUserSurvey surveyDto)
        {
            try
            {
                var survey = Mappings.Mapper.Map<CompUserSurvey>(surveyDto);
                await _compUserSurveyRepository.Create(survey);
                surveyDto.Id = survey.Id;
                return surveyDto;
            }
            catch (Exception e)
            {
                _logger.Error(e, "CompUserSurveyManagementService.CreateNewCompUserSurvey");
                throw;
            }
        }

        public async Task<bool> UpdateExistingCompUserSurvey(Guid id, ICompUserSurvey surveyDto)
        {
            try
            {
                var survey = await _compUserSurveyRepository.Get(id);
                if (survey == null)
                    return false;
                Mappings.Mapper.Map<ICompUserSurvey, CompUserSurvey>(surveyDto, survey);

                await _compUserSurveyRepository.Update(survey);
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e, "CompUserSurveyManagementService.UpdateExistingCompUserSurvey");
                throw;
            }
        }

        public async Task<ICompUserSurvey> GetCompUserSurveyById(Guid id)
        {
            var survey = await _compUserSurveyRepository.Get(id);
            return Mappings.Mapper.Map<ICompUserSurvey>(survey);
        }

        public async Task<IList<ICompUserSurvey>> GetCompUserSurveys()
        {
            var surveys = await _compUserSurveyRepository.List();
            return Mappings.Mapper.Map<IList<ICompUserSurvey>>(surveys);
        }

        public async Task<IList<ICompUserSurvey>> GetCompUserSurveysByCompUserId(Guid userId)
        {
            var surveys = await _compUserSurveyRepository.List(s=>s.CompUserId == userId);
            return Mappings.Mapper.Map<IList<ICompUserSurvey>>(surveys);
        }

        public async Task<IList<ICompUserSurvey>> GetCompUserSurveysBySurveyId(Guid surveyId)
        {
            var surveys = await _compUserSurveyRepository.List(s => s.SurveyId == surveyId);
            return Mappings.Mapper.Map<IList<ICompUserSurvey>>(surveys);
        }

        public async Task<bool> DeleteCompUserSurveysByCompUserId(Guid userId)
        {
            try
            {
                var surveys = await _compUserSurveyRepository.List(o => o.CompUserId == userId);
                if (surveys == null)
                    return false;

                await _compUserSurveyRepository.Delete(o => o.CompUserId == userId);
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e, "CompUserSurveyManagementService.DeleteCompUserSurveysByCompUserId");
                throw;
            }
        }
    }
}
