using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.DAL.Models;
using TaskManagement.DAL.Repository;

namespace TaskManagement.BLL
{
    [Export(typeof(IUserTaskLogic))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class UserTasksLogic : BusinessLogicBase, IUserTaskLogic
    {
        private const string StatusWrong =
            "Статус \"Завершена\" может быть присвоен только после статусов \"Выполняется\" и \"Приостановлена\"!";
        private const string ChildStatusWrong =
            "Задачи не могут быть завершены, т.к. подзадача {0} имеет статус назначена!";

        private IRepository<UserTask> _taskRepository;

        public void InitializeRepository()
        {
            _taskRepository = RepositoryProvider.GetRepository<UserTask>();
        }

        public void InitializeRepository(IRepository<UserTask> repo)
        {
            _taskRepository = repo;
        }

        public ObservableCollection<UserTask> GetRootTasks()
        {
            var rootTasks =
                new ObservableCollection<UserTask>(_taskRepository.GetAll().ToList().Where(t => t.UserTaskId == null));
            return rootTasks;
        }

        public ObservableCollection<UserTask> GetChildsForTask(int parentId)
        {
            var childTasks =
                new ObservableCollection<UserTask>(_taskRepository.GetAll().Where(t => t.UserTaskId == parentId));
            return childTasks;
        }

        public UserTask GetParentForTask(int parentId)
        {
            var parent = _taskRepository.Find(parentId);
            return parent;
        }

        public UserTask FindTask(int taskId)
        {
            var task = _taskRepository.Find(taskId);
            return task;
        }

        public void AddTask(UserTask userTask)
        {
            _taskRepository.Add(userTask);
            RepositoryProvider.SaveChanges();

            //если у задачи есть родитель,
            //то вычисляем для него актуальное время выполнения и плановую трудоемкость подзадач
            if (userTask.UserTaskId != null)
            {
                CalculateTimeFields(userTask);
            }
        }
        public void UpdateTask(UserTask userTask)
        {
            var item = _taskRepository.Find(userTask.Id);
            //проверяем был ли изменен статус задачи
            if (userTask.Status == Statuses.Completed && item.Status != userTask.Status)
            {
                //если первоначальный статус задачи - "Назначена", то выбрасываем исключение
                if (item.Status == Statuses.Appointed)
                {
                    throw new Exception(StatusWrong);
                }
                //проверяем статусы подзадач
                if (CheckStatusForChilds(item))
                {
                    //Переводим статусы подзадач на "Завершенные"
                    UpdateSubtasksStatus(item);
                }
            }
            //проверяем была ли изменена плановая трудоемкость
            if (userTask.PlannedHours != item.PlannedHours)
            {
                UpdateCalculatedFields(userTask, userTask.PlannedHours - item.PlannedHours);
            }
            //проверяем было ли изменено актуальное время выполнения
            if (userTask.ActualExecutionTime != item.ActualExecutionTime)
            {
                UpdateCalculatedFields(userTask, null, userTask.ActualExecutionTime - item.ActualExecutionTime);
            }
            item.Title = userTask.Title;
            item.Description = userTask.Description;
            item.Executors = userTask.Executors;
            item.PlannedHours = userTask.PlannedHours;
            item.Status = userTask.Status;
            item.ActualExecutionTime = userTask.ActualExecutionTime;
            item.FinishedDate = userTask.FinishedDate;
            item.UserTaskId = userTask.UserTaskId;
            RepositoryProvider.SaveChanges();
        }

        public void DeleteTask(UserTask userTask)
        {
            var childTasks = GetChildsForTask(userTask.Id);
            if (childTasks.Any())
            {
                foreach (var child in childTasks)
                {
                    DeleteTask(child);
                }
                //обновляем задачу после удаления подзадачи
                userTask = _taskRepository.Find(userTask.Id);
            }
            _taskRepository.Remove(userTask);
            RepositoryProvider.SaveChanges();
        }

        /// <summary>
        /// Вычисление временных полей
        /// </summary>
        /// <param name="userTask">задача</param>
        private void CalculateTimeFields(UserTask userTask)
        {
            if (userTask.UserTaskId != null)
            {
                //находим родителя для задачи
                var parentTask = GetParentForTask(userTask.UserTaskId.Value);
                if (parentTask != null)
                {
                    if (parentTask.SubTasksActualExecutionTime == null)
                    {
                        parentTask.SubTasksActualExecutionTime = new TimeSpan();
                        parentTask.SubTasksActualExecutionTime += userTask.ActualExecutionTime;
                    }
                    else if (userTask.SubTasksActualExecutionTime == null)
                    {
                        parentTask.SubTasksActualExecutionTime += userTask.ActualExecutionTime;
                    }
                    else
                    {
                        parentTask.SubTasksActualExecutionTime += userTask.SubTasksActualExecutionTime;
                    }

                    if (parentTask.SubTasksPlannedHours == null)
                    {
                        parentTask.SubTasksPlannedHours = new TimeSpan();
                        parentTask.SubTasksPlannedHours += userTask.PlannedHours;
                    }
                    else if (userTask.SubTasksPlannedHours == null)
                    {
                        parentTask.SubTasksPlannedHours += userTask.PlannedHours;
                    }
                    else
                    {
                        parentTask.SubTasksPlannedHours += userTask.SubTasksPlannedHours;
                    }
                    //сохраняем значения вычисляемых полей
                    SaveCalculatedFieldsForTask(parentTask);
                    CalculateTimeFields(parentTask);
                }
            }
        }

        /// <summary>
        /// Обновление вычисляемых полей
        /// </summary>
        /// <param name="userTask">задача, с измененными значениями временных полей</param>
        /// <param name="newPlannedHours">разность нового значения PlannedHours и старого</param>
        /// <param name="newActualExecutionTime">разность нового значения ActualExecutionTime и старого</param>
        private void UpdateCalculatedFields(UserTask userTask, TimeSpan? newPlannedHours = null,
            TimeSpan? newActualExecutionTime = null)
        {
            //если у задачи нет родителей - выходим
            if (userTask.UserTaskId == null)
                return;
            //находим родительскую задачу
            var parentTask = GetParentForTask(userTask.UserTaskId.Value);
            if (parentTask != null)
            {
                //производим вычисления
                if (newActualExecutionTime != null)
                {
                    parentTask.SubTasksActualExecutionTime += newActualExecutionTime;
                }

                if (newPlannedHours != null)
                {
                    parentTask.SubTasksPlannedHours += newPlannedHours;
                }

                //сохраняем новые значения  для родителя
                SaveCalculatedFieldsForTask(parentTask);
                UpdateCalculatedFields(parentTask, newPlannedHours, newActualExecutionTime);
            }
        }

        /// <summary>
        /// Сохранение вычисляемых полей
        /// </summary>
        /// <param name="userTask">задача</param>
        private void SaveCalculatedFieldsForTask(UserTask userTask)
        {
            var item = _taskRepository.Find(userTask.Id);
            item.SubTasksActualExecutionTime = userTask.SubTasksActualExecutionTime;
            item.SubTasksPlannedHours = userTask.SubTasksPlannedHours;
            RepositoryProvider.SaveChanges();
        }
        
        /// <summary>
        /// Проверяет есть ли у подзадач статус "Назначена"
        /// </summary>
        /// <param name="parentTask">родительская задача</param>
        /// <returns></returns>
        private bool CheckStatusForChilds(UserTask parentTask)
        {
            //получаем подзадачи
            var childTasks = GetChildsForTask(parentTask.Id);
            if (childTasks.Any())
            {
                foreach (var child in childTasks)
                {
                    if (child.Status == Statuses.Appointed)
                    {
                        //если встретился статус "Назначена" - выбрасываем исключение о невозможности завершения подзадачи
                        throw new Exception(String.Format(ChildStatusWrong, child.Title));
                    }
                    CheckStatusForChilds(child);
                }
            }
            return true;
        }

        /// <summary>
        /// Перевод статусов подзадач в Завершенные
        /// </summary>
        /// <param name="parentTask">родительская задача</param>
        private void UpdateSubtasksStatus(UserTask parentTask)
        {
            var childTasks = GetChildsForTask(parentTask.Id);
            if (childTasks.Any())
            {
                foreach (var child in childTasks)
                {
                    child.Status = Statuses.Completed;
                    RepositoryProvider.SaveChanges();
                    UpdateSubtasksStatus(child);
                }
            }
        }
    }
}