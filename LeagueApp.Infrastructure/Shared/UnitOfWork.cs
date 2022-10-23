
using LeagueApp.Domain.Shared;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LeagueApp.Infrastructure.Shared
{
    public class UnitOfWork : IUnitOfWork
    {
        private Hashtable _repositories;
        public AppDbContext _db { get; set; }
        private IDbContextTransaction _transaction;
        public UnitOfWork(AppDbContext context)
        {
            _db = context;
        }
        public async Task BeginTransaction()
        {
            _transaction = await _db.Database.BeginTransactionAsync();
        }
        public async Task<int> Complete()
        {
            return await _db.SaveChangesAsync();
        }
        public async Task CommitTransaction()
        {
            await _transaction.CommitAsync();
        }
        public async Task RollbackTransaction()
        {
            await _transaction.RollbackAsync();
        }

        public async Task DisposeTransaction()
        {
            await _transaction.DisposeAsync();
        }
        public bool IsTransactionStarted()
        {
            return _transaction != null;
        }
        public TRepository Repository<TEntity, TRepository>()
            where TEntity : class
            where TRepository : IRepository<TEntity>
        {
            if (_repositories == null)
                _repositories = new Hashtable();

            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                object repositoryInstance;
                var repositoryType = typeof(TRepository);
                if (repositoryType.IsGenericType)
                {
                    repositoryInstance = Activator.CreateInstance(repositoryType
                        .MakeGenericType(typeof(TEntity)), _db);
                }
                else
                {
                    repositoryInstance = Activator.CreateInstance(repositoryType, _db);
                }
                _repositories.Add(type, repositoryInstance);
            }

            return (TRepository)_repositories[type];
        }
        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
