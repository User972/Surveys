using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comp.Survey.Core.Entities;
using Comp.Survey.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Comp.Survey.Infrastructure.Data
{
    public class CompUserSurveyRepository : EntityBaseRepository<CompUserSurvey>, ICompUserSurveyRepository
    {
        private readonly ApplicationDataContext _dbContext;

        public CompUserSurveyRepository(ApplicationDataContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<Core.Entities.CompUserSurvey>> ListWithQuestions(Guid compUserSurveyId)
        {
            var query = _dbContext.Set<Core.Entities.CompUserSurvey>()
                .Include(s => s.CompUserSurveyDetails)
                //.ThenInclude(s => s.SelectedOptionId)
                .Where(s => s.Id == compUserSurveyId);
            return await query.ToListAsync();
        }
        public async Task<Core.Entities.CompUserSurvey> GetWithQuestions(Guid compUserSurveyId)
        {
            var query = _dbContext.Set<Core.Entities.CompUserSurvey>()
                .Include(s => s.CompUserSurveyDetails)
                .FirstOrDefaultAsync(s => s.Id == compUserSurveyId);
            return await query;
        }
    }
}