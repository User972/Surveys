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
    public class SurveyQuestionManagementService : ISurveyQuestionManagementService
    {
        private readonly ISurveyQuestionRepository _questionRepository;
        private readonly ILogger _logger;

        public SurveyQuestionManagementService(ISurveyQuestionRepository questionRepository, ILogger logger)
        {
            _questionRepository = questionRepository;
            _logger = logger;
        }

        public async Task<ISurveyQuestion> CreateNewQuestion(Guid surveyId, ISurveyQuestion questionDto)
        {
            try
            {
                var question = Mappings.Mapper.Map<SurveyQuestion>(questionDto);
                question.SurveyId = surveyId;
                question.Id = Guid.NewGuid();
                await _questionRepository.Create(question);
                return questionDto;
            }
            catch (Exception e)
            {
                _logger.Error(e, "SurveyQuestionManagementService.CreateNewQuestion");
                throw;
            }
        }

        public async Task<bool> UpdateExistingQuestion(Guid id, ISurveyQuestion questionDto)
        {
            try
            {
                var question = await _questionRepository.Get(id);
                if (question == null)
                    return false;
                Mappings.Mapper.Map<ISurveyQuestion, SurveyQuestion>(questionDto, question);

                await _questionRepository.Update(question);
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e, "SurveyQuestionManagementService.UpdateExistingQuestion");
                throw;
            }
        }

        public async Task<ISurveyQuestion> GetQuestionById(Guid id)
        {
            var option = await _questionRepository.Get(id);
            return Mappings.Mapper.Map<ISurveyQuestion>(option);
        }

        public async Task<IList<ISurveyQuestion>> GetQuestionsBySurveyId(Guid surveyId)
        {
            var option = await _questionRepository.ListWithOptions(surveyId);
            return Mappings.Mapper.Map<IList<ISurveyQuestion>>(option);
        }

        public async Task<bool> DeleteQuestionsById(Guid id)
        {
            try
            {
                var question = await _questionRepository.Get(id);
                if (question == null)
                    return false;

                await _questionRepository.Delete(question);
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e, "SurveyQuestionManagementService.DeleteQuestionsById");
                throw;
            }
        }

        public async Task<bool> DeleteQuestionsBySurveyId(Guid surveyId)
        {
            try
            {
                var questions = await _questionRepository.List(q => q.SurveyId == surveyId);
                if (questions == null)
                    return false;

                await _questionRepository.Delete(q => q.SurveyId == surveyId);
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e, "SurveyQuestionManagementService.DeleteQuestionsById");
                throw;
            }
        }
    }
}