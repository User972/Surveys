using Comp.Survey.Core.Entities;
using Comp.Survey.Core.Interfaces;

namespace Comp.Survey.Infrastructure.Data
{
    public class CompUserSurveyDetailRepository : EntityBaseRepository<CompUserSurveyDetail>, ICompUserSurveyDetailRepository
    {
        public CompUserSurveyDetailRepository(ApplicationDataContext dbContext) : base(dbContext)
        {
        }
    }
}