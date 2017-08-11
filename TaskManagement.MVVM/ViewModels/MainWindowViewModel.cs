using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using TaskManagement.DAL.Models;
using TaskManagement.MVVM.Resources;
using TaskManagement.MVVM.TaskService;
using TaskManagement.MVVM.Utils;
using Telerik.Windows.Controls;

namespace TaskManagement.MVVM.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private UserTask _selectedTask;
        private ObservableCollection<UserTask> _userTasks;
        private Visibility _editModeControlsVisibility = Visibility.Collapsed;
        private Visibility _detailsModeControlsVisibility = Visibility.Visible;
        private Visibility _calculatedFieldsVisibility = Visibility.Collapsed;
        private bool _isTextBoxReadOnly = true;

        //выделенная задача
        public UserTask SelectedTask
        {
            get { return _selectedTask; }
            set
            {
                _selectedTask = value;
                //устанавливаем режим просмотра
                SetControlsDisplayMode(true);
                //если у задачи есть подзадачи, делаем вычисляемые поля видимыми, иначе скрываем их
                if (SelectedTask.UserTasks != null && SelectedTask.UserTasks.Any())
                    CalculatedFieldsVisibility = Visibility.Visible;
                else
                    CalculatedFieldsVisibility = Visibility.Collapsed;
                OnPropertyChanged("SelectedTask");
            }
        }

        //коллекция задач
        public ObservableCollection<UserTask> UserTasks
        {
            get { return _userTasks; }
            set
            {
                _userTasks = value;
                OnPropertyChanged("UserTasks");
            }
        }

        //видимость полей в режиме редактирования
        public Visibility EditModeControlsVisibility
        {
            get { return _editModeControlsVisibility; }
            set
            {
                _editModeControlsVisibility = value;
                OnPropertyChanged("EditModeControlsVisibility");
            }
        }

        //видимость полей в режиме просмотра
        public Visibility DetailsModeControlsVisibility
        {
            get { return _detailsModeControlsVisibility; }
            set
            {
                _detailsModeControlsVisibility = value;
                OnPropertyChanged("DetailsModeControlsVisibility");
            }
        }

        //видимость вычисляемых полей
        public Visibility CalculatedFieldsVisibility
        {
            get { return _calculatedFieldsVisibility; }
            set
            {
                _calculatedFieldsVisibility = value;
                OnPropertyChanged("CalculatedFieldsVisibility");
            }
        }
        
        public bool IsTextBoxReadOnly
        {
            get { return _isTextBoxReadOnly; }
            set
            {
                _isTextBoxReadOnly = value;
                OnPropertyChanged("IsTextBoxReadOnly");
            }
        }

        public IEnumerable<KeyValuePair<string, string>> TaskStatuses
            => EnumHelper.GetAllValuesAndDisplayAttributes<Statuses>();

        public MainWindowViewModel()
        {
            DeleteCommand = new DelegateCommand(DeleteTask);
            SaveCommand = new DelegateCommand(SaveTask);
            EditCommand = new DelegateCommand(EditTask);
            AddCommand = new DelegateCommand(AddTask);
            AddChildCommand = new DelegateCommand(AddChildTask);
            UpdateCommand = new DelegateCommand(LoadTasks);
            LoadTasks(null);
        }

        public DelegateCommand DeleteCommand { get; private set; }
        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand EditCommand { get; private set; }
        public DelegateCommand AddCommand { get; private set; }
        public DelegateCommand AddChildCommand { get; private set; }
        public DelegateCommand UpdateCommand { get; private set; }

        /// <summary>
        /// Удаление задачи
        /// </summary>
        /// <param name="parameter"></param>
        private void DeleteTask(object parameter)
        {
            //получаем элемент дерева
            var clickedItem = (parameter as RadContextMenu)?.GetClickedElement<RadTreeViewItem>();
            if (clickedItem != null)
            {
                //преобразуем элемент дерева в задачу
                var taskForDelete = clickedItem.DataContext as UserTask;
                var client = new TaskServiceClient();
                try
                {
                    client.DeleteTask(taskForDelete);
                    //обновляем список задач
                    UserTasks = new ObservableCollection<UserTask>(client.GetRootTasks());
                    client.Close();
                }
                catch (TimeoutException)
                {
                    client.Abort();
                    throw;
                }
                catch (CommunicationException)
                {
                    client.Abort();
                    throw;
                }
            }
        }

        /// <summary>
        /// Проверка валидности задачи
        /// </summary>
        private void ValidationTask()
        {
            //используем файл ресурсов для хранения ошибок валидации
            var rm = ExceptionResources.ResourceManager;
            if (SelectedTask == null)
            {
                throw new Exception(rm.GetString("TaskIsNull"));
            }
            if (SelectedTask.FinishedDate.HasValue &&
                SelectedTask.FinishedDate.Value.CompareTo(SelectedTask.Created) < 0)
            {
                throw new Exception(rm.GetString("FinishedDateError"));
            }
            if (String.IsNullOrEmpty(SelectedTask.Executors))
            {
                throw new Exception(rm.GetString("ExecutorsIsNull"));
            }
            if (String.IsNullOrEmpty(SelectedTask.Title))
            {
                throw new Exception(rm.GetString("TitleIsNull"));
            }
            if (SelectedTask.PlannedHours == null)
            {
                throw new Exception(rm.GetString("PlannedHoursIsNull"));
            }
            if (String.IsNullOrEmpty(SelectedTask.Description))
            {
                throw new Exception(rm.GetString("DescriptionIsNull"));
            }
        }

        /// <summary>
        /// Сохранение задачи
        /// </summary>
        /// <param name="parameter"></param>
        private void SaveTask(object parameter)
        {
            ValidationTask();
            var client = new TaskServiceClient();
            try
            {
                if (SelectedTask.Id > 0)
                {
                    client.UpdateTask(SelectedTask);
                }
                else
                {
                    client.AddTask(SelectedTask);
                }
                client.Close();
            }
            catch (TimeoutException)
            {
                client.Abort();
                throw;
            }
            catch (CommunicationException)
            {
                client.Abort();
                throw;
            }
            //устанавливаем режим просмотра
            SetControlsDisplayMode(true);
            //обновляем дерево задач
            LoadTasks(null);
        }

        /// <summary>
        /// Устанавливает SelectedTask в режим редактирования
        /// </summary>
        /// <param name="parameter"></param>
        private void EditTask(object parameter)
        {
            //получаем элемент дерева
            var clickedItem = (parameter as RadContextMenu)?.GetClickedElement<RadTreeViewItem>();
            if (clickedItem != null)
            {
                //преобразуем элемент дерева в задачу и обновляем SelectedTask
                SelectedTask = clickedItem.DataContext as UserTask;
                //устанавливаем режим редактирования
                SetControlsDisplayMode(false);
            }
        }

        /// <summary>
        /// Открывает режим добавления новой задачи
        /// </summary>
        /// <param name="parameter"></param>
        private void AddTask(object parameter)
        {
            var newTask = new UserTask() {Title = "Новая задача"};
            UserTasks.Add(newTask);
            SelectedTask = newTask;
            //устанавливаем режим редактирования
            SetControlsDisplayMode(false);
        }

        /// <summary>
        /// Открывает режим добавления подазадачи
        /// </summary>
        /// <param name="parameter"></param>
        private void AddChildTask(object parameter)
        {
            //получаем элемент дерева
            var clickedItem = (parameter as RadContextMenu)?.GetClickedElement<RadTreeViewItem>();
            //получеам родителя для нового элемента
            var parentTask = clickedItem?.DataContext as UserTask;
            if (parentTask != null)
            {
                var newTask = new UserTask() {Title = "Новая подзадача", UserTaskId = parentTask?.Id};
                if (parentTask.UserTasks == null)
                {
                    parentTask.UserTasks = new ObservableCollection<UserTask>();
                }
                parentTask.UserTasks.Add(newTask);
                SelectedTask = newTask;
                parentTask.IsExpanded = true;
                //устанавливаем режим редактирования
                SetControlsDisplayMode(false);
            }
        }

        /// <summary>
        /// Загрузка задач
        /// </summary>
        /// <param name="o"></param>
        private void LoadTasks(object o)
        {
            var client = new TaskServiceClient();
            try
            {
                UserTasks = new ObservableCollection<UserTask>(client.GetRootTasks());
                client.Close();
            }
            catch (TimeoutException)
            {
                client.Abort();
                throw;
            }
            catch (CommunicationException)
            {
                client.Abort();
                throw;
            }
        }

        /// <summary>
        /// Устанавливает режим просмотра/редактирования
        /// </summary>
        /// <param name="isReadOnly"></param>
        private void SetControlsDisplayMode(bool isReadOnly)
        {
            IsTextBoxReadOnly = isReadOnly;
            //режим просмотра
            if (isReadOnly)
            {
                EditModeControlsVisibility = Visibility.Collapsed;
                DetailsModeControlsVisibility = Visibility.Visible;

            }
            //режим редактирования
            else
            {
                DetailsModeControlsVisibility = Visibility.Collapsed;
                EditModeControlsVisibility = Visibility.Visible;
            }
        }
    }
}
