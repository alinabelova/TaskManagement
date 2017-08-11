using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using TaskManagement.DAL.Models;

namespace TaskManagement.WcfService
{
    [ServiceContract]
    public interface ITaskService
    {
        [OperationContract]
        ObservableCollection<UserTask> GetRootTasks();

        [OperationContract]
        ObservableCollection<UserTask> GetChildsForTask(int parentId);

        [OperationContract]
        void AddTask(UserTask userTask);

        [OperationContract]
        void UpdateTask(UserTask userTask);

        [OperationContract]
        void DeleteTask(UserTask userTask);

        [OperationContract]
        UserTask FindTask(int taskId);
    }
}
