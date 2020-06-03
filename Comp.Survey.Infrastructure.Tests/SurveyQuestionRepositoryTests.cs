using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Comp.Survey.Infrastructure.Data;
using Comp.Survey.Infrastructure.Tests.MockData;
using TestSupport.EfHelpers;
using Xunit;

namespace Comp.Survey.Infrastructure.Tests
{
    public class SurveyQuestionRepositoryTests
    {
        private readonly DbContextOptions<ApplicationDataContext> _options;

        public SurveyQuestionRepositoryTests()
        {
            _options = EfInMemory.CreateOptions<ApplicationDataContext>();
        }

        [Fact]
        public async Task Create_AddsNewSurveyQuestion_ReturnsSurveyQuestion()
        {
            using (var context = new ApplicationDataContext(_options))
            {
                var repository = new SurveyQuestionRepository(context);
                var newOption = MockSurveys.NewQuestion;
                newOption.SurveyId = MockSurveys.XiomiGuid;
                _ = await repository.Create(newOption);
                var options = context.SurveyQuestions.ToList();
                Assert.NotNull(options);
                Assert.Single(options);
                Assert.Equal(MockSurveys.XiomiGuid, options.First().SurveyId);
            }
        }

        [Fact]
        public async Task Update_UpdatesSurveyQuestion_Returns()
        {
            var name = "modified";
            using (var context = new ApplicationDataContext(_options))
            {
                FeedDataContext(context, MockSurveys.AllSurveys);

                var repository = new SurveyQuestionRepository(context);
                var question = context.SurveyQuestions.First();
                question.Title = name;
                await repository.Update(question);

                Assert.Equal(name, context.SurveyQuestions.First().Title);
            }
        }

        [Fact]
        public async Task Delete_RemovesSurveyQuestion_Returns()
        {
            using (var context = new ApplicationDataContext(_options))
            {
                FeedDataContext(context, MockSurveys.AllSurveys);

                var repository = new SurveyQuestionRepository(context);
                var survey = context.SurveyQuestions.First();
                await repository.Delete(survey);

                var surveys = context.SurveyQuestions.ToList();
                Assert.Equal(5, surveys.Count);
            }
        }

        [Fact]
        public async Task List_ReturnsAllSurveyQuestions()
        {
            using (var context = new ApplicationDataContext(_options))
            {
                FeedDataContext(context, MockSurveys.AllSurveys);

                var repository = new SurveyQuestionRepository(context);
                var surveys = await repository.List();

                Assert.NotNull(surveys);
                Assert.Equal(6, surveys.Count);
            }
        }

        [Fact]
        public async Task List_WhenExpressionPassed_ReturnsFilteredSurveyQuestions()
        {
            using (var context = new ApplicationDataContext(_options))
            {
                FeedDataContext(context, MockSurveys.AllSurveys);

                var repository = new SurveyQuestionRepository(context);
                var surveys = await repository.List(p => p.Title.Contains("white", StringComparison.InvariantCultureIgnoreCase));

                Assert.NotNull(surveys);
                Assert.Equal(4, surveys.Count);
            }
        }

        [Fact]
        public async Task Delete_WhenExpressionPassed_RemovesMatchingSurveyQuestions_Returns()
        {
            using (var context = new ApplicationDataContext(_options))
            {
                FeedDataContext(context, MockSurveys.AllSurveys);

                var repository = new SurveyQuestionRepository(context);
                await repository.Delete(p => !p.Title.Contains("black", StringComparison.InvariantCultureIgnoreCase));

                var surveys = context.SurveyQuestions.ToList();
                Assert.NotNull(surveys);
                Assert.Equal(3, surveys.Count);
            }
        }

        [Fact]
        public async Task ListWithOptions_WhenExpressionPassed_ReturnsFilteredSurveyQuestions()
        {
            using (var context = new ApplicationDataContext(_options))
            {
                FeedDataContext(context, MockSurveys.AllSurveys);

                var repository = new SurveyQuestionRepository(context);
                var surveys = await repository.ListWithOptions(MockSurveys.XiomiGuid);

                Assert.NotNull(surveys);
                Assert.Equal(2, surveys.Count);
            }
        }
        private static void FeedDataContext(ApplicationDataContext context, IEnumerable<Core.Entities.Survey> surveys)
        {
            context.Surveys.AddRange(surveys);
            context.SaveChanges();
        }

    }
}
