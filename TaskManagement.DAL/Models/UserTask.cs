using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using TaskManagement.DAL.Annotations;

namespace TaskManagement.DAL.Models
{
    public class UserTask : NotifyPropertyChangedBase, IDomainObject
    {
        private DateTime _created = DateTime.Now;
        private string _title;
        private string _description;
        private string _executors;
        private Statuses _status;
        private TimeSpan? _plannedHours;
        private TimeSpan? _actualExecutionTime;
        private DateTime? _finishedDate;
        private ObservableCollection<UserTask> _userTasks;
        private bool _isExpanded;

        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime Updated { get; set; } = DateTime.Now;

        [Required]
        public DateTime Created
        {
            get { return _created; }
            set
            {
                _created = value;
                OnPropertyChanged("Created");
            }
        } 

        [Required]
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }
        
        [Required]
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged("Description");
            }
        }

        [Required]
        public string Executors
        {
            get { return _executors; }
            set
            {
                _executors = value;
                OnPropertyChanged("Executors");
            }
        }

        [Required]
        public Statuses Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged("Status");
            }
        }
        
        [Required]
        public TimeSpan? PlannedHours
        {
            get { return _plannedHours; }
            set
            {
                _plannedHours = value;
                OnPropertyChanged("PlannedHours");
            }
        }
        
        public TimeSpan? ActualExecutionTime
        {
            get { return _actualExecutionTime; }
            set
            {
                _actualExecutionTime = value;
                OnPropertyChanged("ActualExecutionTime");
            }
        }
        
        public DateTime? FinishedDate
        {
            get { return _finishedDate; }
            set
            {
                _finishedDate = value;
                OnPropertyChanged("FinishedDate");
            }
        }
       
        public ObservableCollection<UserTask> UserTasks
        {
            get { return _userTasks; }
            set
            {
                _userTasks = value;
                OnPropertyChanged("UserTasks");
            }
        }

        public virtual int? UserTaskId { get; set; }
        
        [NotMapped]
        public bool IsExpanded
        {
            get
            {
                return _isExpanded;
            }
            set
            {
                _isExpanded = value;
                OnPropertyChanged("IsExpanded");
            }
        }

        //вычисляемые поля для отображения времени выполнения подзадач:
        public long? SubTasksPlannedHoursTicks { get; set; }

        public long? SubTasksActualExecutionTimeTicks { get; set; }

        [NotMapped]
        public TimeSpan? SubTasksActualExecutionTime
        {
            get
            {
                if (SubTasksActualExecutionTimeTicks != null)
                    return TimeSpan.FromTicks(SubTasksActualExecutionTimeTicks.Value);
                return null;
            }
            set { SubTasksActualExecutionTimeTicks = value?.Ticks; }
        }

        [NotMapped]
        public TimeSpan? SubTasksPlannedHours
        {
            get
            {
                if (SubTasksPlannedHoursTicks != null)
                    return TimeSpan.FromTicks(SubTasksPlannedHoursTicks.Value);
                return null;
            }
            set { SubTasksPlannedHoursTicks = value?.Ticks; }
        }
    }
}
