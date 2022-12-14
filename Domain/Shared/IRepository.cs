using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace LeagueApp.Domain.Shared
{
    public interface IRepository<T> where T : class
    {
        Task Add(T enttity);
        Task Update(T entity);
        Task Delete(object id);
        Task Delete(Expression<Func<T, bool>> where);
        Task Delete(T entity);
        Task<T> GetById(object id);
        Task<T> GetAsync(Expression<Func<T, bool>> where);
        Task<IQueryable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties);
        Task<IQueryable<T>> GetManyAsync(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includeProperties);
        IQueryable<T> GetMany(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includeProperties);
        Task AddRange(IEnumerable<T> list);
        Task UpdateRange(IEnumerable<T> list);
    }
}
