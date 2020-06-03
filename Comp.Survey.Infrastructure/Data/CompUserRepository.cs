using Comp.Survey.Core.Entities;
using Comp.Survey.Core.Interfaces;

namespace Comp.Survey.Infrastructure.Data
{
    public class CompUserRepository : EntityBaseRepository<CompUser>, ICompUserRepository
    {
        public CompUserRepository(ApplicationDataContext dbContext) : base(dbContext)
        {
        }
    }
}