using Comp.Survey.Core.Interfaces;

namespace Comp.Survey.Infrastructure.Data
{
    public class SurveyRepository : EntityBaseRepository<Core.Entities.Survey>, ISurveyRepository
    {
        public SurveyRepository(ApplicationDataContext dbContext) : base(dbContext)
        {
        }
    }
}