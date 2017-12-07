using Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Services
{
    public class Repository : IRepository
    {
        private DataContext _context;

        public Repository(DataContext context)
        {
            _context = context;
        }

        public IQueryable<T> All<T>() where T : class
        {
            return _context.Set<T>().AsNoTracking();
        }

        public int Count<T>(Expression<Func<T, bool>> conditions) where T : class
        {
            return _context.Set<T>().Count<T>(conditions);
        }

        public Task<int> CountAsync<T>(Expression<Func<T, bool>> conditions) where T : class
        {
            return _context.Set<T>().CountAsync<T>(conditions);
        }

        public void Update<T>(T entity) where T : class
        {
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                _context.Set<T>().Attach(entity);
            }
            entry.State = EntityState.Modified;
        }

        public void Insert<T>(T entity) where T : class
        {
            _context.Set<T>().Add(entity);
        }

        public Task InsertAsync<T>(T entity) where T : class
        {
            return _context.Set<T>().AddAsync(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                _context.Set<T>().Attach(entity);
            }
            entry.State = EntityState.Deleted;
            _context.Set<T>().Remove(entity);
        }

        public void Delete<T>(Expression<Func<T, bool>> conditions) where T : class
        {
            var list = Find<T>(conditions);
            foreach (var item in list)
            {
                Delete<T>(item);
            }
        }

        public T Get<T>(Expression<Func<T, bool>> conditions) where T : class
        {
            return All<T>().FirstOrDefault(conditions);
        }

        public Task<T> GetAsync<T>(Expression<Func<T, bool>> conditions) where T : class
        {
            return All<T>().FirstOrDefaultAsync(conditions);
        }

        public List<T> Find<T>(Expression<Func<T, bool>> conditions = null) where T : class
        {
            if (conditions != null)
            {
                return All<T>().Where(conditions).ToList();
            }
            else
            {
                return All<T>().ToList();
            }
        }

        public Task<List<T>> FindAsync<T>(Expression<Func<T, bool>> conditions = null) where T : class
        {
            if (conditions != null)
            {
                return All<T>().Where(conditions).ToListAsync();
            }
            else
            {
                return All<T>().ToListAsync();
            }
        }

        public List<T> Find<T, S>(Expression<Func<T, bool>> conditions, Expression<Func<T, S>> orderBy, int pageSize, int pageIndex, out int totalCount) where T : class
        {
            var queryList = conditions == null ?
                All<T>() :
                All<T>().Where(conditions);

            totalCount = queryList.Count();

            return queryList.OrderByDescending(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        public Task<List<T>> FindAsync<T, S>(Expression<Func<T, bool>> conditions, Expression<Func<T, S>> orderBy, int pageSize, int pageIndex, out int totalCount) where T : class
        {
            var queryList = conditions == null ?
                All<T>() :
                All<T>().Where(conditions);

            totalCount = queryList.Count();

            return queryList.OrderByDescending(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
