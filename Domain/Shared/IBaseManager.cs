using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LeagueApp.Domain.Shared
{
    public interface IBaseManager<TEntity> where TEntity : class
    {
        Task<IQueryable<TEntity>> GetAll();
        Task Add(TEntity entity);
        Task Update(TEntity entity);
        Task<int> AddWithComplete(TEntity entity);
        Task<int> UpdateWithComplete(TEntity entity);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> where);
        Task<IQueryable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includeProperties);
        Task<IQueryable<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties);
        Task AddRange(IEnumerable<TEntity> list);
        Task UpdateRange(IEnumerable<TEntity> list);
        Task Remove(int id);
        Task Remove(Expression<Func<TEntity, bool>> where);
        Task Remove(TEntity entity);
        Task<int> RemoveWithComplete(TEntity entity);
        Task<int> Complete();
        Task BeginTransaction();
        Task DisposeTransaction();
        Task RollbackTransaction();
        Task CommitTransaction();


    }
}
