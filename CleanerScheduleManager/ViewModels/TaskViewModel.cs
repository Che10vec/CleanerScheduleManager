using CleanerScheduleManager.Models;
using CleanerScheduleManager.Models.Enums;
using CleanerScheduleManager.Services.Interfaces;
using CleanerScheduleManager.Utilities;
using CleanerScheduleManager.ViewModels.Base;
using CleanerScheduleManager.ViewModels.Interfaces;
using CleanerScheduleManager.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Data;
using System.Windows.Input;
using static CleanerScheduleManager.ViewModels.Base.ViewModelBase;
using TaskStatusEnum = CleanerScheduleManager.Models.Enums.TaskStatus;

namespace CleanerScheduleManager.ViewModels
{
    public class TaskViewModel : ViewModelBase, IHasPendingEdits, IPersistable
    {
        private readonly IDataService _dataService;
        private readonly string _dataFilePath = Path.Combine(AppContext.BaseDirectory, "tasks.json");
        private readonly ClientViewModel _clientViewModel;
        private readonly CleanerViewModel _cleanerViewModel;
        private string _searchText = string.Empty;
        private CleaningTask? _selectedTask;

        public ObservableCollection<CleaningTask> Tasks { get; } = new();
        public ICollectionView TasksView { get; }
        public IEnumerable<TaskStatusEnum> TaskStatuses { get => Enum.GetValues<TaskStatusEnum>(); }
        public IEnumerable<Client> Clients => _clientViewModel.Clients;
        public IEnumerable<Cleaner> Cleaners => _cleanerViewModel.Cleaners;

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
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    TasksView.Refresh();
                }
            }
        }

        public ICommand AddTaskCommand { get; }
        public ICommand DeleteTaskCommand { get; }

        public TaskViewModel(IDataService dataService, ClientViewModel clientViewModel, CleanerViewModel cleanerViewModel)
        {
            _dataService = dataService;
            _clientViewModel = clientViewModel;
            _cleanerViewModel = cleanerViewModel;
            TasksView = CollectionViewSource.GetDefaultView(Tasks);
            TasksView.Filter = FilterTasks;
            AddTaskCommand = new RelayCommand(_ => AddTask());
            DeleteTaskCommand = new RelayCommand(_ => DeleteTask(), _ => CanDeleteTask());
        }

        private void AddTask()
        {
            var task = new CleaningTask
            {
                TaskId = Tasks.Count + 1,
                ScheduledDate = DateTime.Today,
                Duration = TimeSpan.FromHours(2),
                Status = TaskStatusEnum.Scheduled,
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

        public void FinalizeEdits()
        {
            IEditableCollectionView? editable = CollectionViewSource.GetDefaultView(Tasks) as IEditableCollectionView;
            if (editable != null)
            {
                if (editable.IsAddingNew)
                    editable.CommitNew();
                if (editable.IsEditingItem)
                    editable.CommitEdit();
            }
        }
        public async Task LoadDataAsync()
        {
            var tasks = await _dataService.LoadAsync<CleaningTask>(_dataFilePath);
            Tasks.Clear();
            foreach (var task in tasks)
            {
                if (task.Client != null)
                    task.Client = _clientViewModel.Clients.FirstOrDefault(c => c.Id == task.Client.Id);
                if (task.Cleaner != null)
                    task.Cleaner = _cleanerViewModel.Cleaners.FirstOrDefault(c => c.Id == task.Cleaner.Id);
                Tasks.Add(task);
            }
        }

        public Task SaveDataAsync()
        {
            FinalizeEdits();
            return _dataService.SaveAsync(Tasks, _dataFilePath);
        }
        private bool FilterTasks(object obj)
        {
            if (obj is not CleaningTask task)
                return false;

            if (string.IsNullOrWhiteSpace(SearchText))
                return true;

            string search = SearchText.ToLowerInvariant();
            string client = task.Client == null ? string.Empty : $"{task.Client.Id} {task.Client.Name} {task.Client.Address} {task.Client.Phone} ";
            string cleaner = task.Cleaner == null ? string.Empty : $"{task.Cleaner.Id} {task.Cleaner.Name} {task.Cleaner.SkillLevel} {task.Cleaner.IsAvailable} ";
            string combined = ($"{task.TaskId} {task.ScheduledDate} {task.Duration} {task.Status} {client}{cleaner}").ToLowerInvariant();
            return combined.Contains(search);
        }
    }
}