
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using LeagueApp.Domain.Shared;

namespace LeagueApp.Infrastructure.Shared
{
    public class BaseManager<TEntity> : IBaseManager<TEntity> where TEntity : class
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;
        private readonly IRepository<TEntity> _repository;
        public BaseManager(IUnitOfWork unitOfWork, IMapper mapper, IRepository<TEntity> repository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repository = repository;
        }
        public virtual async Task<IQueryable<TEntity>> GetAll()
        {
            return await _repository.GetAllAsync();
        }

        public virtual async Task Add(TEntity entity)
        {
            await _repository.Add(entity);
        }


        public virtual async Task Update(TEntity entity)
        {
            await _repository.Update(entity);
        }

        public virtual async Task<TEntity> GetById(object id)
        {
            return await _repository.GetById(id);
        }

        public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> @where)
        {
            return await _repository.GetAsync(@where);
        }
      
        public virtual async Task<IQueryable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return await _repository.GetAllAsync(includeProperties);
        }
     
        public virtual IQueryable<TEntity> GetMany(Expression<Func<TEntity, bool>> @where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return  _repository.GetMany(@where, includeProperties);
        }
        public virtual Task <IQueryable<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> @where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return  _repository.GetManyAsync(@where, includeProperties);
        }
       
        public virtual async Task AddRange(IEnumerable<TEntity> list)
        {
            await _repository.AddRange(list);
        }
        public virtual async Task UpdateRange(IEnumerable<TEntity> list)
        {
            await _repository.UpdateRange(list);
        }
        public virtual async Task Remove(int id)
        {
            await _repository.Delete(id);

        }

        public virtual async Task Remove(TEntity entity)
        {
            await _repository.Delete(entity);
        }

        public virtual async Task Remove(Expression<Func<TEntity, bool>> @where)
        {
            await _repository.Delete(@where);
        }

        public async Task<int> Complete()
        {
            return await _unitOfWork.Complete();
        }

        public async Task BeginTransaction()
        {
            await _unitOfWork.BeginTransaction();
        }

        public async Task DisposeTransaction()
        {
            await _unitOfWork.DisposeTransaction();
        }

        public async Task RollbackTransaction()
        {
            await _unitOfWork.RollbackTransaction();
        }
        public async Task CommitTransaction()
        {
            await _unitOfWork.CommitTransaction();
        }
        public async Task<int> AddWithComplete(TEntity entity)
        {
            await Add(entity);
            return await Complete();
        }

        public async Task<int> UpdateWithComplete(TEntity entity)
        {
            await Update(entity);
            return await Complete();
        }

        public async Task<int> RemoveWithComplete(TEntity entity)
        {
            await Remove(entity);
            return await Complete();
        }
       
    }
}
