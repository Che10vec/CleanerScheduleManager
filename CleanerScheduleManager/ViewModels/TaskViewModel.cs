using CleanerScheduleManager.Models;
using CleanerScheduleManager.Models.Enums;
using CleanerScheduleManager.Services.Interfaces;
using CleanerScheduleManager.Utilities;
using CleanerScheduleManager.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace CleanerScheduleManager.ViewModels
{
    public class TaskViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private CleaningTask? _selectedTask;

        public ObservableCollection<CleaningTask> Tasks { get; } = new();

        public CleaningTask? SelectedTask
        {
            get => _selectedTask;
            set
            {
                if (SetProperty(ref _selectedTask, value))
                {
                    ((RelayCommand)DeleteTaskCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public ICommand AddTaskCommand { get; }
        public ICommand DeleteTaskCommand { get; }

        public TaskViewModel(IDataService dataService)
        {
            _dataService = dataService;

            AddTaskCommand = new RelayCommand(_ => AddTask());
            DeleteTaskCommand = new RelayCommand(_ => DeleteTask(), _ => CanDeleteTask());
        }

        private void AddTask()
        {
            var task = new CleaningTask
            {
                TaskId = Tasks.Count + 1,
                ClientId = 1,
                CleanerId = 1,
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
                SelectedTask = null;
            }
        }

        private bool CanDeleteTask()
        {
            return SelectedTask != null;
        }
    }
}