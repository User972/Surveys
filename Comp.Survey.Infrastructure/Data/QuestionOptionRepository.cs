using Comp.Survey.Core.Entities;
using Comp.Survey.Core.Interfaces;

namespace Comp.Survey.Infrastructure.Data
{
    public class QuestionOptionRepository : EntityBaseRepository<QuestionOption>, IQuestionOptionRepository
    {
        private readonly ApplicationDataContext _dataContext;

        public QuestionOptionRepository(ApplicationDataContext dataContext) : base(dataContext)
        {
            _dataContext = dataContext;
        }
    }
}
