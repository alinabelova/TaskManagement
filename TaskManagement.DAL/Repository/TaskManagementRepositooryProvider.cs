using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.DAL.Repository
{
    [Export(typeof(IRepositoryProvider))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class TaskManagementRepositooryProvider : EntityRepositoryProvider<DataContext>
    {
    }
}
