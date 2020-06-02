using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Comp.Survey.Core.Entities;
using Comp.Survey.Core.Interfaces;

namespace Comp.Survey.Infrastructure.Data
{
    public class EntityBaseRepository<T> : IEntityBaseRepository<T> where T : EntityBase
    {
        private readonly ApplicationDataContext _dataContext;

        public EntityBaseRepository(ApplicationDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<T> Create(T entity)
        {
            _dataContext.Set<T>().Add(entity);
            await _dataContext.SaveChangesAsync();
            return entity;
        }

        public async Task Update(T entity)
        {
            _dataContext.Entry(entity).State = EntityState.Modified;
            await _dataContext.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            _dataContext.Set<T>().Remove(entity);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<T> Get(Guid id)
        {
            return await _dataContext.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> List()
        {
            return await _dataContext.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> List(Expression<Func<T, bool>> lambdaExpression)
        {
            var query = _dataContext.Set<T>().AsQueryable();
            if (lambdaExpression != null)
            {
                query = query.Where(lambdaExpression);
            }
           
            return await query.ToListAsync();
        }

        public async Task Delete(Expression<Func<T, bool>> lambdaExpression)
        {
            var records = await _dataContext.Set<T>().Where(lambdaExpression).ToListAsync();
            if (records.Count > 0)
            {
                _dataContext.Set<T>().RemoveRange(records);
                await _dataContext.SaveChangesAsync();
            }
        }
    }
}
