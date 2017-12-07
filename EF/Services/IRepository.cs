using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Services
{
    public interface IRepository
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();

        IQueryable<T> All<T>() where T : class;
        int Count<T>(Expression<Func<T, bool>> conditions) where T : class;
        Task<int> CountAsync<T>(Expression<Func<T, bool>> conditions) where T : class;
        T Get<T>(Expression<Func<T, bool>> conditions) where T : class;
        void Insert<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        void Delete<T>(Expression<Func<T, bool>> conditions) where T : class;
        List<T> Find<T>(Expression<Func<T, bool>> conditions = null) where T : class;
        List<T> Find<T, S>(Expression<Func<T, bool>> conditions, Expression<Func<T, S>> orderBy, int pageSize, int pageIndex, out int totalCount) where T : class;

        Task<T> GetAsync<T>(Expression<Func<T, bool>> conditions) where T : class;
        Task InsertAsync<T>(T entity) where T : class;
        Task<List<T>> FindAsync<T>(Expression<Func<T, bool>> conditions = null) where T : class;
        Task<List<T>> FindAsync<T, S>(Expression<Func<T, bool>> conditions, Expression<Func<T, S>> orderBy, int pageSize, int pageIndex, out int totalCount) where T : class;
    }
}
