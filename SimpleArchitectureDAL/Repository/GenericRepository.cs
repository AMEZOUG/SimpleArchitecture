using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SimpleArchitectureDAL.Repositories
{
    /// <summary>
    /// Generic Repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DbSet<T> _objectSet;

        public GenericRepository(DbContext entities)
        {
            _objectSet = entities.Set<T>();
        }

        //Sync methods
        public IEnumerable<T> GetAll(Func<T, bool> predicate = null)
        {
            return predicate != null ? _objectSet.Where(predicate) : _objectSet.AsEnumerable();
        }
        public T Get(Func<T, bool> predicate)
        {
            return _objectSet.FirstOrDefault(predicate);
        }
        public void Add(T entity)
        {
            _objectSet.Add(entity);
        }
        public void Update(T entity)
        {
            _objectSet.Update(entity);
            //_objectSet.Attach(entity);
            //_entities.Entry(entity).State = EntityState.Modified;
            //DbEntityEntry dbEntityEntry = DbContext.Entry(entity);
            //dbEntityEntry.State = EntityState.Modified;
        }
        public void Delete(T entity)
        {
            _objectSet.Remove(entity);
        }
        public void DeleteRange(List<T> entity)
        {
            _objectSet.RemoveRange(entity);
        }
        public void AddRange(List<T> entity)
        {
            _objectSet.AddRange(entity);
        }
        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return _objectSet.Where(predicate);
        }
        public IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            return includeProperties.Aggregate<Expression<Func<T, object>>, IQueryable<T>>(_objectSet, (current, includeProperty) => current.Include(includeProperty));
        }
        public IEnumerable<T> GetByPaging(Expression<Func<T, string>> sort, bool desc, int page, int pageSize, out int totalRecords)
        {
            totalRecords = _objectSet.Count();
            if (desc)
            {
                return _objectSet.OrderByDescending(sort).Skip(page).Take(pageSize);
            }
            else
            {
                return _objectSet.OrderBy(sort).Skip(page).Take(pageSize);
            }
        }
        //Async methods
        public async Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate != null)
            {
                return await _objectSet.Where(predicate).ToListAsync();
            }

            return await _objectSet.ToListAsync();
        }
        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _objectSet.FirstOrDefaultAsync(predicate);
        }
        public void AddAsync(T entity)
        {
            _objectSet.Add(entity);
        }
        public void UpdateAsync(T entity)
        {
            _objectSet.Update(entity);
        }
        public void DeleteAsync(T entity)
        {
            _objectSet.Remove(entity);
        }
        public void DeleteRangeAsync(List<T> entity)
        {
            _objectSet.RemoveRange(entity);
        }
        public void AddRangeAsync(List<T> entity)
        {
            _objectSet.AddRange(entity);
        }
        public async Task<ICollection<T>> FindByAsync(Expression<Func<T, bool>> predicate)
        {
            return await _objectSet.Where(predicate).ToListAsync();
        }
        public async Task<ICollection<T>> AllIncludingAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            var query = includeProperties.Aggregate<Expression<Func<T, object>>, IQueryable<T>>(_objectSet, (current, includeProperty) => current.Include(includeProperty));
            return await query.ToListAsync();
        }
    }
}
