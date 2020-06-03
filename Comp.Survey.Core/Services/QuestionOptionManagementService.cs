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
    public class QuestionOptionManagementService : IQuestionOptionsManagementService
    {
        private readonly IQuestionOptionRepository _optionRepository;
        private readonly ILogger _logger;

        public QuestionOptionManagementService(IQuestionOptionRepository optionRepository, ILogger logger)
        {
            _optionRepository = optionRepository;
            _logger = logger;
        }

        public async Task<IQuestionOption> CreateNewOption(Guid questionId, IQuestionOption optionDto)
        {
            try
            {
                var option = Mappings.Mapper.Map<QuestionOption>(optionDto);
                option.SurveyQuestionId = questionId;
                option.Id = Guid.NewGuid();
                var persistedEntity = await _optionRepository.Create(option);
                optionDto.Id = persistedEntity.Id;
                return optionDto;
            }
            catch (Exception e)
            {
                _logger.Error(e, "QuestionOptionManagementService.CreateNewOption");
                throw;
            }
        }

        public async Task<bool> UpdateExistingOption(Guid id, IQuestionOption optionDto)
        {
            try
            {
                var option = await _optionRepository.Get(id);
                if (option == null)
                    return false;
                Mappings.Mapper.Map<IQuestionOption, QuestionOption>(optionDto, option);

                await _optionRepository.Update(option);
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e, "QuestionOptionManagementService.UpdateExistingOption");
                throw;
            }
        }

        public async Task<IQuestionOption> GetOptionById(Guid id)
        {
            var option = await _optionRepository.Get(id);
            return Mappings.Mapper.Map<IQuestionOption>(option);
        }

        public async Task<IList<IQuestionOption>> GetOptionsByQuestionId(Guid questionId)
        {
            var option = await _optionRepository.List(o => o.SurveyQuestionId == questionId);
            return Mappings.Mapper.Map<IList<IQuestionOption>>(option);
        }

        public async Task<bool> DeleteOptionById(Guid id)
        {
            try
            {
                var option = await _optionRepository.Get(id);
                if (option == null)
                    return false;

                await _optionRepository.Delete(option);
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e, "QuestionOptionManagementService.DeleteOptionById");
                throw;
            }
        }

        public async Task<bool> DeleteOptionsByQuestionId(Guid questionId)
        {
            try
            {
                var options = await _optionRepository.List(o => o.SurveyQuestionId == questionId);
                if (options == null)
                    return false;

                await _optionRepository.Delete(o => o.SurveyQuestionId == questionId);
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e, "QuestionOptionManagementService.DeleteOptionsByQuestionId");
                throw;
            }
        }
    }
}