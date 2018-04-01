using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SimpleArchitectureDAL.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        //Sync methods
        IEnumerable<T> GetAll(Func<T, bool> predicate = null);
        T Get(Func<T, bool> predicate);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void DeleteRange(List<T> entity);
        void AddRange(List<T> entity);
        IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        IEnumerable<T> GetByPaging(Expression<Func<T, string>> sort, bool desc, int page, int pageSize, out int totalRecords);

        //Async methods
        Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null);
        Task<T> GetAsync(Expression<Func<T, bool>> predicate);
        void AddAsync(T entity);
        void UpdateAsync(T entity);
        void DeleteAsync(T entity);
        void DeleteRangeAsync(List<T> entity);
        void AddRangeAsync(List<T> entity);
        Task<ICollection<T>> AllIncludingAsync(params Expression<Func<T, object>>[] includeProperties);
        Task<ICollection<T>> FindByAsync(Expression<Func<T, bool>> predicate);

    }
}
