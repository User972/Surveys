using Comp.Survey.Core.Entities;
using Comp.Survey.Core.Interfaces;

namespace Comp.Survey.Infrastructure.Data
{
    public class SurveyRepository : EntityBaseRepository<Core.Entities.Survey>, ISurveyRepository
    {
        public SurveyRepository(ApplicationDataContext dbContext) : base(dbContext)
        {
        }
    }
    public class CompUserRepository : EntityBaseRepository<CompUser>, ICompUserRepository
    {
        public CompUserRepository(ApplicationDataContext dbContext) : base(dbContext)
        {
        }
    }
    public class CompUserSurveyRepository : EntityBaseRepository<CompUserSurvey>, ICompUserSurveyRepository
    {
        public CompUserSurveyRepository(ApplicationDataContext dbContext) : base(dbContext)
        {
        }
    }

  public class CompUserSurveyDetailRepository : EntityBaseRepository<CompUserSurveyDetail>, ICompUserSurveyDetailRepository
    {
        public CompUserSurveyDetailRepository(ApplicationDataContext dbContext) : base(dbContext)
        {
        }
    }
}