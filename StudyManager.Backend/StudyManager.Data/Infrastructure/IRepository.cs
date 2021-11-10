using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace StudyManager.Data.Infrastructure
{
    public interface IRepository<T> where T: class
    {
        Task<List<T>> GetAll();
        Task<List<T>> GetAll(Expression<Func<T, bool>> expression);
        Task<List<T>> GetWithLimit(int skip, int take, Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes);
        Task<List<T>> GetWithLimit(int skip, int take);

        Task<T> Get(Guid id, params Expression<Func<T, object>>[] includes);
        Task<T> GetFirstOrDefault(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes);

        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(Guid id);
    }
}
