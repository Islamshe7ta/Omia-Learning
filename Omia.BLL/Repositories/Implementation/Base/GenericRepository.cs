using Microsoft.EntityFrameworkCore;
using Omia.DAL.Data;
using Omia.BLL.Repositories.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Omia.DAL.Models.Base;

namespace Omia.BLL.Repositories.Implementation.Base
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly OmiaDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(OmiaDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.Where(expression).ToListAsync();
        }

        public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.FirstOrDefaultAsync(expression);
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? expression = null)
        {
            return expression == null 
                ? await _dbSet.CountAsync() 
                : await _dbSet.CountAsync(expression);
        }

        public virtual async Task<int> MaxAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> selector)
        {
            var query = _dbSet.Where(predicate);
            if (await query.AnyAsync())
            {
                return await query.MaxAsync(selector);
            }
            return 0;
        }

        public virtual async Task AddAsync(T entity)
        {
            if (entity is Omia.DAL.Models.Base.BaseEntity baseEntity)
            {
                baseEntity.CreatedAt = DateTime.UtcNow;
                baseEntity.LastUpdatedAt = DateTime.UtcNow;
                baseEntity.IsDeleted = false;
            }
            await _dbSet.AddAsync(entity);
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                if (entity is BaseEntity baseEntity)
                {
                    baseEntity.CreatedAt = DateTime.UtcNow;
                    baseEntity.LastUpdatedAt = DateTime.UtcNow;
                    baseEntity.IsDeleted = false;
                }
            }
            await _dbSet.AddRangeAsync(entities);
        }

        public virtual void Update(T entity)
        {
            if (entity is BaseEntity baseEntity)
            {
                baseEntity.LastUpdatedAt = DateTime.UtcNow;
            }
            _dbSet.Update(entity);
        }

        public virtual void Delete(T entity)
        {
            if (entity is BaseEntity baseEntity)
            {
                baseEntity.IsDeleted = true;
                baseEntity.LastUpdatedAt = DateTime.UtcNow;
                _dbSet.Update(entity);
            }
            else
            {
                _dbSet.Remove(entity);
            }
        }

        public virtual void RemoveRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }
    }
}
