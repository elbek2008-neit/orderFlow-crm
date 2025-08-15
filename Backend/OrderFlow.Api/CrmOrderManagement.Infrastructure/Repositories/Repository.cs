using CrmOrderManagement.Infrastructure.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrmOrderManagement.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CrmOrderManagement.Core.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly CrmDbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public Repository(CrmDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        // =========================
        // СИНХРОННЫЕ МЕТОДЫ
        // =========================
        public T? GetById(int id)
            => _dbSet.Find(id);

        public IQueryable<T> GetAll()
            => _dbSet.AsQueryable();

        public IQueryable<T> Find(Expression<Func<T, bool>> expression)
            => _dbSet.Where(expression);

        public void Add(T entity)
            => _dbSet.Add(entity);

        public void AddRange(IEnumerable<T> entities)
            => _dbSet.AddRange(entities);

        public void Update(T entity)
            => _dbSet.Update(entity);

        public void Remove(T entity)
            => _dbSet.Remove(entity);

        public void RemoveRange(IEnumerable<T> entities)
            => _dbSet.RemoveRange(entities);

        // =========================
        // АСИНХРОННЫЕ МЕТОДЫ
        // =========================
        public async Task<T?> GetByIdAsync(int id)
            => await _dbSet.FindAsync(id);

        public async Task<IEnumerable<T>> GetAllAsync()
            => await _dbSet.ToListAsync();

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression)
            => await _dbSet.Where(expression).ToListAsync();

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> expression)
            => await _dbSet.FirstOrDefaultAsync(expression);

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
            => await _dbSet.AnyAsync(expression);

        public async Task<int> CountAsync(Expression<Func<T, bool>>? expression = null)
            => expression == null
                ? await _dbSet.CountAsync()
                : await _dbSet.CountAsync(expression);

        // =========================
        // ПАГИНАЦИЯ
        // =========================

        public async Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync<TKey>(
            int pageNumber,
            int pageSize,
            Expression<Func<T, bool>>? filter = null,
            Expression<Func<T, TKey>>? orderBy = null,
            bool ascending = true)
        {
            var query = _dbSet.AsQueryable();

            // Фильтрация
            if (filter != null)
                query = query.Where(filter);

            // Общее количество записей
            var totalCount = await query.CountAsync();

            // Сортировка
            if (orderBy != null)
            {
                query = ascending
                    ? query.OrderBy(orderBy)
                    : query.OrderByDescending(orderBy);
            }

            // Пагинация
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);

        }
    }
}
