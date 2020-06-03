using Comp.Survey.Core.Entities;
using Comp.Survey.Core.Interfaces;

namespace Comp.Survey.Infrastructure.Data
{
    public class CompUserSurveyRepository : EntityBaseRepository<CompUserSurvey>, ICompUserSurveyRepository
    {
        public CompUserSurveyRepository(ApplicationDataContext dbContext) : base(dbContext)
        {
        }
    }
}