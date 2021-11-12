using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace StudyManager.Data.Infrastructure
{
    public interface IRepository<T> where T: class
    {
        IQueryable<T> Query(params Expression<Func<T, object>>[] includes);

        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T> DeleteAsync(Guid id);
    }
}
