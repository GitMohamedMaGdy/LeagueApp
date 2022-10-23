using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace LeagueApp.Domain.Shared
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> Complete();
        Task BeginTransaction();
        Task CommitTransaction();
        Task RollbackTransaction();
        Task DisposeTransaction();
        TRepository Repository<TEntity, TRepository>() where TEntity : class
            where TRepository : IRepository<TEntity>;
    }
}
