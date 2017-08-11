using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.DAL.Models;

namespace TaskManagement.DAL.Repository
{
    public interface IRepository<TItem> where TItem : class, IDomainObject
    {
        void Add(TItem item);
        void Remove(TItem item);
        TItem Find(int id);
        IQueryable<TItem> GetAll();
    }
}
