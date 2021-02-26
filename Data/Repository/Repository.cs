using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AddressBookApi.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace AddressBookApi.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext _context;

        public Repository(AppDbContext context)
        {
            _context ??= context;
        }

        public IQueryable<T> AllIncludingAsQueryable(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> queryable = _context.Set<T>();

            return includeProperties.Aggregate(queryable,
                (current, includeProperty) => current.Include(includeProperty));
        }

        public IEnumerable<T> GetAll() => _context.Set<T>().AsEnumerable();

        public async Task<int> CountAsync() => await _context.Set<T>().CountAsync();

        public T GetSingle(Expression<Func<T, bool>> predicate) => _context.Set<T>().FirstOrDefault(predicate);

        public IQueryable<T> FindByAsQueryable(Expression<Func<T, bool>> predicate) => _context.Set<T>().Where(predicate);

        public async Task AddAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Added;
            await _context.Set<T>().AddAsync(entity);
        }

        public void Add(T entity)
        {
            var dbEntityEntry = _context.Entry(entity);
            dbEntityEntry.State = EntityState.Added;
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }

        public void Update(T entity) => _context.Entry(entity).State = EntityState.Modified;

        public void Delete(T entity) => _context.Entry(entity).State = EntityState.Deleted;

        public void DeleteWhere(Expression<Func<T, bool>> predicate)
        {
            var entities = _context.Set<T>().Where(predicate);

            foreach (var entity in entities)
                _context.Entry(entity).State = EntityState.Deleted;

        }

        public void AddAll(IEnumerable<T> list) => _context.Set<T>().AddRange(list);

        public IQueryable<T> AsQueryable() => _context.Set<T>().AsQueryable();

        public IQueryable<T> GetSet() => _context.Set<T>();
    }
}
