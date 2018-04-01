using Microsoft.EntityFrameworkCore;
using SimpleArchitectureDAL.Infrastructure;
using SimpleArchitectureDAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleArchitectureDAL.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SimpleArchitectureContext _entities;
        public Dictionary<Type, object> Repositories = new Dictionary<Type, object>();

        public UnitOfWork(SimpleArchitectureContext entities)
        {
            _entities = entities;
        }

        public IGenericRepository<T> Repository<T>() where T : class
        {
            if (Repositories.Keys.Contains(typeof(T)))
            {
                return Repositories[typeof(T)] as IGenericRepository<T>;
            }

            IGenericRepository<T> repo = new GenericRepository<T>(_entities);
            Repositories.Add(typeof(T), repo);
            return repo;
        }

        public int SaveChanges()
        {
            return _entities.SaveChanges();
        }

        public async Task<int> SaveChangesAsyn()
        {
            return await _entities.SaveChangesAsync();
        }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _entities.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
