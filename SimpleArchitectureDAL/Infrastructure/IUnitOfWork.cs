using SimpleArchitectureDAL.Repositories;
using System;
using System.Threading.Tasks;

namespace SimpleArchitectureDAL.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : class;

        int SaveChanges();
        Task<int> SaveChangesAsyn();
    }
}
