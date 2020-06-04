using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comp.Survey.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Comp.Survey.Infrastructure.Data
{
    public class SurveyRepository : EntityBaseRepository<Core.Entities.Survey>, ISurveyRepository
    {
        private readonly ApplicationDataContext _dbContext;

        public SurveyRepository(ApplicationDataContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<Core.Entities.Survey>> ListWithQuestions(Guid surveyId)
        {
            var query = _dbContext.Set<Core.Entities.Survey>()
                .Include(s => s.SurveyQuestions)
                .Where(s => s.Id == surveyId);
            return await query.ToListAsync();
        }
        public async Task<Core.Entities.Survey> GetWithQuestions(Guid surveyId)
        {
            var query = _dbContext.Set<Core.Entities.Survey>()
                .Include(s => s.SurveyQuestions)
                .FirstOrDefaultAsync(s => s.Id == surveyId);
            return await query;
        }
    }
}