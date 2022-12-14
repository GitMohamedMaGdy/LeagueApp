using LeagueApp.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LeagueApp.Infrastructure.Shared
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly AppDbContext _dbContext;
        protected IQueryable<TEntity> cashedEntities;
        protected DateTime lastModifedDate;
        public Repository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Add(TEntity enttity)
        {
            await _dbContext.Set<TEntity>().AddAsync(enttity);
        }

        public async Task AddRange(IEnumerable<TEntity> list)
        {
            _dbContext.Set<TEntity>().AddRange(list);
        }

        public async Task Delete(object id)
        {
            var entity = _dbContext.Set<TEntity>().Find(id);
            _dbContext.Set<TEntity>().Remove(entity);
        }

        public async Task Delete(Expression<Func<TEntity, bool>> where)
        {
            TEntity entity = await _dbContext.Set<TEntity>().FirstOrDefaultAsync(where);
            _dbContext.Set<TEntity>().Remove(entity);
        }

        public async Task Delete(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> where)
        {
            return await _dbContext.Set<TEntity>().FirstOrDefaultAsync(where);
        }
     
        public async Task<IQueryable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            return query;
        }
        public async Task<TEntity> GetById(object id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        public async Task<IQueryable<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            return query.Where(where);
        }
        public IQueryable<TEntity> GetMany(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            return query.Where(where);
        }
        public async Task Update(TEntity entity)
        {
            _dbContext.Attach(entity).State = EntityState.Modified;
        }
        public async Task UpdateRange(IEnumerable<TEntity> list)
        {
            list.ToList().ForEach(item => _dbContext.Attach(item).State = EntityState.Modified);
        }
      
    }
}
