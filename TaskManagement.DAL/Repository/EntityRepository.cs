using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.DAL.Models;

namespace TaskManagement.DAL.Repository
{
    public class EntityRepository<T> : IRepository<T> where T : class, IDomainObject
    {
        private readonly DbContext _dbContext;

        public EntityRepository(DbContext db)
        {
            _dbContext = db;
        }

        public void Add(T item)
        {
            var dbSet = _dbContext.Set<T>();
            dbSet.Add(item);
        }

        public void Remove(T item)
        {
            var dbSet = _dbContext.Set<T>();
            var entry = _dbContext.Entry(item);
            if (entry.State == EntityState.Detached)
                dbSet.Attach(item);
            dbSet.Remove(item);
            _dbContext.Entry(item).State = EntityState.Deleted;
        }

        public T Find(int id)
        {
            var dbSet = _dbContext.Set<T>();
            var item = dbSet.Find(id);
            return item;
        }

        public IQueryable<T> GetAll()
        {
            var dbSet = _dbContext.Set<T>();
            return dbSet;
        }
    }
}
