using System;
using System.Collections.Generic;
using Comp.Survey.App.Controllers;
using Comp.Survey.App.Models;
using Comp.Survey.Core.Interfaces.DTO;
using Comp.Survey.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Serilog;
using Xunit;

namespace Comp.Survey.API.Tests.Controllers
{
    public class SurveyControllerTests
    {
        private readonly Mock<ISurveyManagementService> _surveyService;
        private readonly Mock<ISurveyQuestionManagementService> _questionService;

        private readonly Guid _nonMatchingGuid;
        private readonly Guid _matchingGuid;
        private readonly Guid _nonMatchingOptionGuid;
        private readonly Guid _matchingOptionGuid;
        private readonly string _surveyName = "survey1";
        private readonly SurveysController _controller;

        public SurveyControllerTests()
        {
            _matchingGuid = Guid.NewGuid();
            _nonMatchingGuid = Guid.NewGuid();
            _matchingOptionGuid = Guid.NewGuid();
            _nonMatchingOptionGuid = Guid.NewGuid();

            #region Surveys
            var survey1 = new App.Models.Survey
            {
                Id = _matchingGuid
            };
            var survey2 = new App.Models.Survey
            {
                Id = _nonMatchingGuid
            };

            _surveyService = new Mock<ISurveyManagementService>();

            _surveyService.Setup(repo =>
                repo.GetSurveys()).ReturnsAsync(new List<ISurvey> { survey1, survey2 });

            _surveyService.Setup(repo =>
                repo.GetSurveysWithNameLike(It.Is<string>(a => a.Equals(_surveyName)))).ReturnsAsync(new List<ISurvey> { survey1 });

            _surveyService.Setup(repo =>
                repo.GetSurveyById(It.Is<Guid>(a => a.Equals(_matchingGuid)))).ReturnsAsync(survey1);

            _surveyService.Setup(repo => repo.CreateNewSurvey(It.IsAny<App.Models.Survey>())).ReturnsAsync(survey1);

            _surveyService.Setup(repo => repo.UpdateExistingSurvey(_matchingGuid, It.IsAny<App.Models.Survey>())).ReturnsAsync(true);
            _surveyService.Setup(repo => repo.UpdateExistingSurvey(_nonMatchingGuid, It.IsAny<App.Models.Survey>())).ReturnsAsync(false);


            #endregion

            #region Options

            var option1 = new SurveyQuestion
            {
                Id = _matchingOptionGuid
            };
            var option2 = new SurveyQuestion
            {
                Id = _nonMatchingOptionGuid
            };

            _questionService = new Mock<ISurveyQuestionManagementService>();
            _questionService.Setup(repo =>
                repo.GetQuestionsBySurveyId(It.Is<Guid>(a => a.Equals(_matchingGuid)))).ReturnsAsync(new List<ISurveyQuestion> { option1, option2 });

            _questionService.Setup(repo =>
                repo.GetQuestionsBySurveyId(It.Is<Guid>(a => a.Equals(_matchingGuid)))).ReturnsAsync(new List<ISurveyQuestion> { option1, option2 });

            _questionService.Setup(repo =>
                repo.GetQuestionById(It.Is<Guid>(a => a.Equals(_matchingOptionGuid)))).ReturnsAsync(option1);

            var logger = new Mock<ILogger>();
            logger.Setup(l => l.Error(It.IsAny<Exception>(), It.IsAny<string>()));
            logger.Setup(l => l.Warning(It.IsAny<string>()));

            var optionService = new Mock<IQuestionOptionsManagementService>();
            var surveySubmissionService = new Mock<ICompUserSurveyManagementService>();

            #endregion

            _controller = new SurveysController(_surveyService.Object, _questionService.Object, optionService.Object, surveySubmissionService.Object, logger.Object);
        }

        [Fact]
        public void GetAllAsync_WhenNoNamePassed_CallsGetSurveys_ReturnsOk()
        {
            var response = _controller.GetAllAsync(string.Empty).Result;

            _surveyService.Verify(repo => repo.GetSurveys(), Times.Exactly(1));
            _surveyService.Verify(repo => repo.GetSurveysWithNameLike(It.IsAny<string>()), Times.Never);

            Assert.IsType<OkObjectResult>(response);

            var surveys = ((OkObjectResult)response).Value as IList<ISurvey>;

            Assert.NotNull(surveys);
            Assert.Equal(2, surveys.Count);
            Assert.Contains(surveys, p => p.Id == _matchingGuid);
            Assert.Contains(surveys, p => p.Id == _nonMatchingGuid);
        }

        [Fact]
        public void GetAllAsync_WhenNamePassed_GetSurveysWithNameLike_ReturnsOk()
        {
            var response = _controller.GetAllAsync(_surveyName).Result;

            _surveyService.Verify(repo => repo.GetSurveys(), Times.Never);
            _surveyService.Verify(repo => repo.GetSurveysWithNameLike(It.Is<string>(a => a.Equals(_surveyName))), Times.Exactly(1));

            Assert.IsType<OkObjectResult>(response);

            var surveys = ((OkObjectResult)response).Value as IList<ISurvey>;

            Assert.NotNull(surveys);
            Assert.Equal(1, surveys.Count);
            Assert.Contains(surveys, p => p.Id == _matchingGuid);
            Assert.DoesNotContain(surveys, p => p.Id == _nonMatchingGuid);
        }

        [Fact]
        public void GetSurveyAsync_WhenSurveyNotFound_ReturnsErrorCode()
        {
            var response = _controller.GetSurveyAsync(_nonMatchingGuid).Result;

            _surveyService.Verify(repo => repo.GetSurveyById(_nonMatchingGuid), Times.Exactly(1));
            Assert.IsType<NotFoundResult>(response);
        }

        [Fact]
        public void GetSurveyAsync_WhenSurveyFound_ReturnsOk()
        {
            var response = _controller.GetSurveyAsync(_matchingGuid).Result;

            _surveyService.Verify(repo => repo.GetSurveyById(_matchingGuid), Times.Exactly(1));
            Assert.IsType<OkObjectResult>(response);

            var survey = ((OkObjectResult)response).Value as ISurvey;
            Assert.NotNull(survey);
            Assert.Equal(survey.Id, _matchingGuid);
        }

        [Fact]
        public void GetAllQuestionsAsync_WhenNoQuestionsForSurveySearchedFor_ReturnsOkNoResults()
        {
            var response = _controller.GetAllQuestionsAsync(_nonMatchingGuid).Result;

            _questionService.Verify(repo => repo.GetQuestionsBySurveyId(_nonMatchingGuid), Times.Exactly(1));
            Assert.IsType<OkObjectResult>(response);

            var options = ((OkObjectResult)response).Value as IList<ISurveyQuestion>;

            Assert.Null(options);
        }

        [Fact]
        public void GetAllQuestionsAsync_WhenNoOptionsForSurveySearchedFor_ReturnsOk()
        {
            var response = _controller.GetAllQuestionsAsync(_matchingGuid).Result;

            _questionService.Verify(repo => repo.GetQuestionsBySurveyId(_matchingGuid), Times.Exactly(1));
            Assert.IsType<OkObjectResult>(response);

            var options = ((OkObjectResult)response).Value as IList<ISurveyQuestion>;

            Assert.NotNull(options);
            Assert.Equal(2, options.Count);
            Assert.Contains(options, p => p.Id == _matchingOptionGuid);
            Assert.Contains(options, p => p.Id == _nonMatchingOptionGuid);
        }

        [Fact]
        public void GetSurveyQuestionAsync_WhenSurveyQuestionNotFound_ReturnsErrorCode()
        {
            var response = _controller.GetSurveyQuestionAsync(_matchingGuid, _nonMatchingOptionGuid).Result;

            _questionService.Verify(repo => repo.GetQuestionById(_nonMatchingOptionGuid), Times.Exactly(1));
            Assert.IsType<NotFoundResult>(response);
        }

        [Fact]
        public void GetSurveyQuestionAsync_WhenSurveyQuestionFound_ReturnsOk()
        {
            var response = _controller.GetSurveyQuestionAsync(_matchingGuid, _matchingOptionGuid).Result;

            _questionService.Verify(repo => repo.GetQuestionById(_matchingOptionGuid), Times.Exactly(1));

            var option = ((OkObjectResult)response).Value as ISurveyQuestion;

            Assert.NotNull(option);
            Assert.Equal(option.Id, _matchingOptionGuid);
        }

        [Fact]
        public void CreateAsync_WhenModelIsValid_ReturnsOk()
        {
            var response = _controller.CreateAsync(new App.Models.Survey
            {
                Name = _surveyName
            }).Result;

            _surveyService.Verify(repo => repo.CreateNewSurvey(It.IsAny<App.Models.Survey>()), Times.Exactly(1));
            Assert.IsType<CreatedAtActionResult>(response);

            Assert.NotNull(response);

            var survey = ((CreatedAtActionResult)response).Value as App.Models.Survey;
            Assert.NotNull(survey);
            Assert.Equal(survey.Id, _matchingGuid);
        }

        [Fact]
        public void UpdateAsync_WhenModelIsValid_And_MatchingSurveyNotFound_ReturnsNotFound()
        {
            var response = _controller.UpdateAsync(_nonMatchingGuid, new App.Models.Survey
            {
                Name = _surveyName
            }).Result;

            _surveyService.Verify(repo => repo.UpdateExistingSurvey(_nonMatchingGuid, It.IsAny<App.Models.Survey>()), Times.Exactly(1));
            Assert.IsType<NotFoundResult>(response);
        }

        [Fact]
        public void UpdateAsync_WhenModelIsValid_And_MatchingSurveyFound_ReturnsOk()
        {
            var response = _controller.UpdateAsync(_matchingGuid, new App.Models.Survey
            {
                Name = _surveyName
            }).Result;

            _surveyService.Verify(repo => repo.UpdateExistingSurvey(_matchingGuid, It.IsAny<App.Models.Survey>()), Times.Exactly(1));
            Assert.IsType<OkResult>(response);
        }

    }
}
