using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Comp.Survey.Core.Entities;
using Comp.Survey.Infrastructure.Data;
using Comp.Survey.Infrastructure.Tests.MockData;
using TestSupport.EfHelpers;
using Xunit;

namespace Comp.Survey.Infrastructure.Tests
{
    public class SurveyRepositoryTests
    {
        private readonly DbContextOptions<ApplicationDataContext> _options;

        public SurveyRepositoryTests()
        {
            _options = EfInMemory.CreateOptions<ApplicationDataContext>();
        }

        [Fact]
        public async Task Create_AddsNewSurvey_ReturnsSurvey()
        {
            using (var context = new ApplicationDataContext(_options))
            {
                var repository = new SurveyRepository(context);
                _ = await repository.Create(MockSurveys.iPhonePro);
                var surveys = context.Surveys.ToList();
                Assert.NotNull(surveys);
                Assert.Single(surveys);
                Assert.Equal(MockSurveys.iPhonePro.Name, surveys.First().Name);
            }
        }

        [Fact]
        public async Task Update_UpdatesSurvey_Returns()
        {
            var name = "modified";
            using (var context = new ApplicationDataContext(_options))
            {
                FeedDataContext(context, MockSurveys.AllSurveys);
            
                var repository = new SurveyRepository(context);
                var survey = context.Surveys.First();
                survey.Name = name;
                await repository.Update(survey);

                Assert.Equal(name, context.Surveys.First().Name);
            }

        }

        [Fact]
        public async Task Delete_RemovesSurvey_Returns()
        {
            using (var context = new ApplicationDataContext(_options))
            {
                FeedDataContext(context, MockSurveys.AllSurveys);

                var repository = new SurveyRepository(context);
                var survey = context.Surveys.First();
                await repository.Delete(survey);

                var surveys = context.Surveys.ToList();
                Assert.Equal(2, surveys.Count);
            }
        }
        
        [Fact]
        public async Task List_ReturnsAllSurveys()
        {
            using (var context = new ApplicationDataContext(_options))
            {
                FeedDataContext(context, MockSurveys.AllSurveys);

                var repository = new SurveyRepository(context);
                var surveys = await repository.List();

                Assert.NotNull(surveys);
                Assert.Equal(3, surveys.Count);
            }
        }

        [Fact]
        public async Task List_WhenExpressionPassed_ReturnsFilteredSurveys()
        {
            using (var context = new ApplicationDataContext(_options))
            {
                FeedDataContext(context, MockSurveys.AllSurveys);

                var repository = new SurveyRepository(context);
                var surveys = await repository.List(p => p.Name.Contains("iphone", StringComparison.InvariantCultureIgnoreCase));

                Assert.NotNull(surveys);
                Assert.Equal(1, surveys.Count);
                Assert.Equal(MockSurveys.iPhonePro.Id, surveys.First().Id);
            }
        }

        [Fact]
        public async Task Delete_WhenExpressionPassed_RemovesMatchingSurveys_Returns()
        {
            using (var context = new ApplicationDataContext(_options))
            {
                FeedDataContext(context, MockSurveys.AllSurveys);

                var repository = new SurveyRepository(context);
                await repository.Delete(p => !p.Name.Contains("iphone", StringComparison.InvariantCultureIgnoreCase));

                var surveys = context.Surveys.ToList();
                Assert.Single(surveys);
                Assert.Equal(MockSurveys.iPhonePro.Id, surveys.First().Id);
            }
        }

        private static void FeedDataContext(ApplicationDataContext context, IEnumerable<Core.Entities.Survey> surveys)
        {
            context.Surveys.AddRange(surveys);
            context.SaveChanges();
        }

    }
}
