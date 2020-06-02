using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Comp.Survey.Core.Entities;
using Comp.Survey.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Comp.Survey.Infrastructure.Data
{
    public class SurveyQuestionRepository : EntityBaseRepository<SurveyQuestion>, ISurveyQuestionRepository
    {
        private readonly ApplicationDataContext _dataContext;
        
        public SurveyQuestionRepository(ApplicationDataContext dataContext) : base(dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IReadOnlyList<SurveyQuestion>> ListWithOptions(Guid surveyId)
        {
            var query = _dataContext.Set<SurveyQuestion>()
                .Include(s=>s.QuestionOptions)
                .Where(s=>s.SurveyId == surveyId);
            return await query.ToListAsync();
        }
    }
}