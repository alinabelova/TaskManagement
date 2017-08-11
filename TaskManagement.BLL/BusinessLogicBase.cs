using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.DAL.Repository;

namespace TaskManagement.BLL
{
    public class BusinessLogicBase : IDisposable
    {
        [Import]
        protected IRepositoryProvider RepositoryProvider { get; private set; }

        public void Dispose()
        {
            if (RepositoryProvider != null)
            {
                RepositoryProvider.Dispose();
                RepositoryProvider = null;
            }
        }
    }
}
