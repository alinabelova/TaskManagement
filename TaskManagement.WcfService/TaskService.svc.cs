using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Microsoft.Practices.ServiceLocation;
using TaskManagement.BLL;
using TaskManagement.DAL.Models;
using TaskManagement.DAL.Repository;

namespace TaskManagement.WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.

    public class TaskService : ITaskService, IDisposable
    {
        [Import]
        private IUserTaskLogic UserTasksLogic { get; set; }
        public TaskService()
        {
            var container = ServiceLocator.Current.GetInstance<CompositionContainer>();
            container.ComposeParts(this);
            UserTasksLogic.InitializeRepository();
        }
        public TaskService(IRepository<UserTask> repo)
        {
            var container = ServiceLocator.Current.GetInstance<CompositionContainer>();
            container.ComposeParts(this);
            UserTasksLogic.InitializeRepository(repo);
        }
        public ObservableCollection<UserTask> GetRootTasks()
        {
            return UserTasksLogic.GetRootTasks();
        }

        public ObservableCollection<UserTask> GetChildsForTask(int parentId)
        {
            return UserTasksLogic.GetChildsForTask(parentId);
        }

        public void AddTask(UserTask userTask)
        {
            UserTasksLogic.AddTask(userTask);
        }

        public void UpdateTask(UserTask userTask)
        {
            UserTasksLogic.UpdateTask(userTask);
        }

        public void DeleteTask(UserTask userTask)
        {
            UserTasksLogic.DeleteTask(userTask);
        }

        public UserTask FindTask(int taskId)
        {
            return UserTasksLogic.FindTask(taskId);
        }

        public void Dispose()
        {
            UserTasksLogic?.Dispose();
        }
    }
}
