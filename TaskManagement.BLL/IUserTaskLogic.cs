using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.DAL.Models;
using TaskManagement.DAL.Repository;

namespace TaskManagement.BLL
{
    public interface IUserTaskLogic:IDisposable
    {
        /// <summary>
        /// Инициализация репозитория
        /// </summary>
        void InitializeRepository();
        void InitializeRepository(IRepository<UserTask> repo);

        /// <summary>
        /// Получение корневых задач
        /// </summary>
        /// <returns>корневые задачи</returns>
        ObservableCollection<UserTask> GetRootTasks();

        /// <summary>
        /// Получение родителя для задачи
        /// </summary>
        /// <param name="parentId">ИД родителя</param>
        /// <returns></returns>
        UserTask GetParentForTask(int parentId);

        /// <summary>
        /// Получение подзадач для задачи
        /// </summary>
        /// <param name="parentId">ИД родительской задачи</param>
        /// <returns>подзадачи</returns>
        ObservableCollection<UserTask> GetChildsForTask(int parentId);

        /// <summary>
        /// Добавление новой задачи
        /// </summary>
        /// <param name="userTask">задача</param>
        void AddTask(UserTask userTask);

        /// <summary>
        /// Обновление существующей задачи
        /// </summary>
        /// <param name="userTask">задача</param>
        void UpdateTask(UserTask userTask);

        /// <summary>
        /// Удаление задачи
        /// </summary>
        /// <param name="userTask">задача</param>
        void DeleteTask(UserTask userTask);

        /// <summary>
        /// Поиск задачи
        /// </summary>
        /// <param name="taskId">ИД задачи</param>
        /// <returns>найденная задача</returns>
        UserTask FindTask(int taskId);
    }
}
