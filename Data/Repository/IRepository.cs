using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AddressBookApi.Data.Repository
{
    public interface IRepository<T>
    {
        IQueryable<T> AllIncludingAsQueryable(params Expression<Func<T, object>>[] includeProperties);
        IEnumerable<T> GetAll();
        Task<int> CountAsync();
        T GetSingle(Expression<Func<T, bool>> predicate);
        IQueryable<T> FindByAsQueryable(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void DeleteWhere(Expression<Func<T, bool>> predicate);
        void AddAll(IEnumerable<T> list);
        IQueryable<T> AsQueryable();
        IQueryable<T> GetSet();
    }
}
