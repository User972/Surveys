﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Comp.Survey.Core.Entities;

namespace Comp.Survey.Core.Interfaces
{
    public interface IEntityBaseRepository<T> where T : EntityBase
    {
        Task<T> Create(T entity);
        Task Update(T entity);
        Task Delete(T entity);
        Task<T> Get(Guid id);
        Task<IReadOnlyList<T>> List();
        Task<IReadOnlyList<T>> List(Expression<Func<T, bool>> lambdaExpression);
        Task Delete(Expression<Func<T, bool>> lambdaExpression);
    }

    public interface ISurveyRepository : IEntityBaseRepository<Entities.Survey>
    {
    }

    public interface ISurveyQuestionRepository : IEntityBaseRepository<SurveyQuestion>
    {
        Task<IReadOnlyList<SurveyQuestion>> ListWithOptions(Guid surveyId);
    }
    public interface IQuestionOptionRepository : IEntityBaseRepository<QuestionOption>
    {
    }
    public interface ICompUserRepository : IEntityBaseRepository<CompUser>
    {
    }
    public interface ICompUserSurveyRepository : IEntityBaseRepository<CompUserSurvey>
    {
    }
    public interface ICompUserSurveyDetailRepository : IEntityBaseRepository<CompUserSurveyDetail>
    {
    }
}
