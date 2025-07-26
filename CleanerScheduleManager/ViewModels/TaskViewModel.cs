using CleanerScheduleManager.Models;
using CleanerScheduleManager.Utilities;
using CleanerScheduleManager.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using CleanerScheduleManager.Models.Enums;

namespace CleanerScheduleManager.ViewModels
{
    public class TaskViewModel : ViewModelBase
    {
        private CleaningTask? _selectedTask;

        public ObservableCollection<CleaningTask> Tasks { get; } = new();

        public CleaningTask? SelectedTask
        {
            get => _selectedTask;
            set => SetProperty(ref _selectedTask, value);
        }

        public ICommand AddTaskCommand { get; }
        public ICommand DeleteTaskCommand { get; }

        public TaskViewModel()
        {
            AddTaskCommand = new RelayCommand(_ => AddTask());
            DeleteTaskCommand = new RelayCommand(_ => DeleteTask(), _ => SelectedTask != null);
        }

        private void AddTask()
        {
            var task = new CleaningTask
            {
                TaskId = Tasks.Count + 1,
                ClientId = 1, // placeholder
                CleanerId = 1, // placeholder
                ScheduledDate = DateTime.Today,
                Duration = TimeSpan.FromHours(2),
                Status = Models.Enums.TaskStatus.Scheduled,
            };

            Tasks.Add(task);
        }

        private void DeleteTask()
        {
            if (SelectedTask != null)
            {
                Tasks.Remove(SelectedTask);
            }
        }
    }
}
