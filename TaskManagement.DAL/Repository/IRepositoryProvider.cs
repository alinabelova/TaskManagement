using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.DAL.Models;

namespace TaskManagement.DAL.Repository
{
    public interface IRepositoryProvider : IDisposable
    {
        IRepository<T> GetRepository<T>() where T : class, IDomainObject;
        void SaveChanges();
        void RevertChanges();
    }
}
