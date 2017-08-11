﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TaskManagement.MVVM.TaskService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="TaskService.ITaskService")]
    public interface ITaskService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITaskService/GetRootTasks", ReplyAction="http://tempuri.org/ITaskService/GetRootTasksResponse")]
        TaskManagement.DAL.Models.UserTask[] GetRootTasks();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITaskService/GetRootTasks", ReplyAction="http://tempuri.org/ITaskService/GetRootTasksResponse")]
        System.Threading.Tasks.Task<TaskManagement.DAL.Models.UserTask[]> GetRootTasksAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITaskService/GetChildsForTask", ReplyAction="http://tempuri.org/ITaskService/GetChildsForTaskResponse")]
        TaskManagement.DAL.Models.UserTask[] GetChildsForTask(int parentId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITaskService/GetChildsForTask", ReplyAction="http://tempuri.org/ITaskService/GetChildsForTaskResponse")]
        System.Threading.Tasks.Task<TaskManagement.DAL.Models.UserTask[]> GetChildsForTaskAsync(int parentId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITaskService/AddTask", ReplyAction="http://tempuri.org/ITaskService/AddTaskResponse")]
        void AddTask(TaskManagement.DAL.Models.UserTask userTask);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITaskService/AddTask", ReplyAction="http://tempuri.org/ITaskService/AddTaskResponse")]
        System.Threading.Tasks.Task AddTaskAsync(TaskManagement.DAL.Models.UserTask userTask);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITaskService/UpdateTask", ReplyAction="http://tempuri.org/ITaskService/UpdateTaskResponse")]
        void UpdateTask(TaskManagement.DAL.Models.UserTask userTask);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITaskService/UpdateTask", ReplyAction="http://tempuri.org/ITaskService/UpdateTaskResponse")]
        System.Threading.Tasks.Task UpdateTaskAsync(TaskManagement.DAL.Models.UserTask userTask);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITaskService/DeleteTask", ReplyAction="http://tempuri.org/ITaskService/DeleteTaskResponse")]
        void DeleteTask(TaskManagement.DAL.Models.UserTask userTask);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITaskService/DeleteTask", ReplyAction="http://tempuri.org/ITaskService/DeleteTaskResponse")]
        System.Threading.Tasks.Task DeleteTaskAsync(TaskManagement.DAL.Models.UserTask userTask);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITaskService/FindTask", ReplyAction="http://tempuri.org/ITaskService/FindTaskResponse")]
        TaskManagement.DAL.Models.UserTask FindTask(int taskId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITaskService/FindTask", ReplyAction="http://tempuri.org/ITaskService/FindTaskResponse")]
        System.Threading.Tasks.Task<TaskManagement.DAL.Models.UserTask> FindTaskAsync(int taskId);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ITaskServiceChannel : TaskManagement.MVVM.TaskService.ITaskService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class TaskServiceClient : System.ServiceModel.ClientBase<TaskManagement.MVVM.TaskService.ITaskService>, TaskManagement.MVVM.TaskService.ITaskService {
        
        public TaskServiceClient() {
        }
        
        public TaskServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public TaskServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public TaskServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public TaskServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public TaskManagement.DAL.Models.UserTask[] GetRootTasks() {
            return base.Channel.GetRootTasks();
        }
        
        public System.Threading.Tasks.Task<TaskManagement.DAL.Models.UserTask[]> GetRootTasksAsync() {
            return base.Channel.GetRootTasksAsync();
        }
        
        public TaskManagement.DAL.Models.UserTask[] GetChildsForTask(int parentId) {
            return base.Channel.GetChildsForTask(parentId);
        }
        
        public System.Threading.Tasks.Task<TaskManagement.DAL.Models.UserTask[]> GetChildsForTaskAsync(int parentId) {
            return base.Channel.GetChildsForTaskAsync(parentId);
        }
        
        public void AddTask(TaskManagement.DAL.Models.UserTask userTask) {
            base.Channel.AddTask(userTask);
        }
        
        public System.Threading.Tasks.Task AddTaskAsync(TaskManagement.DAL.Models.UserTask userTask) {
            return base.Channel.AddTaskAsync(userTask);
        }
        
        public void UpdateTask(TaskManagement.DAL.Models.UserTask userTask) {
            base.Channel.UpdateTask(userTask);
        }
        
        public System.Threading.Tasks.Task UpdateTaskAsync(TaskManagement.DAL.Models.UserTask userTask) {
            return base.Channel.UpdateTaskAsync(userTask);
        }
        
        public void DeleteTask(TaskManagement.DAL.Models.UserTask userTask) {
            base.Channel.DeleteTask(userTask);
        }
        
        public System.Threading.Tasks.Task DeleteTaskAsync(TaskManagement.DAL.Models.UserTask userTask) {
            return base.Channel.DeleteTaskAsync(userTask);
        }
        
        public TaskManagement.DAL.Models.UserTask FindTask(int taskId) {
            return base.Channel.FindTask(taskId);
        }
        
        public System.Threading.Tasks.Task<TaskManagement.DAL.Models.UserTask> FindTaskAsync(int taskId) {
            return base.Channel.FindTaskAsync(taskId);
        }
    }
}
