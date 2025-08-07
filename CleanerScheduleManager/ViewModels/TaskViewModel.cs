using CleanerScheduleManager.Models;
using CleanerScheduleManager.Models.Enums;
using CleanerScheduleManager.Services.Interfaces;
using CleanerScheduleManager.Utilities;
using CleanerScheduleManager.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using static CleanerScheduleManager.ViewModels.Base.ViewModelBase;
using TaskStatusEnum = CleanerScheduleManager.Models.Enums.TaskStatus;

namespace CleanerScheduleManager.ViewModels
{
    public class TaskViewModel : ViewModelBase, IHasPendingEdits
    {
        private readonly IDataService _dataService;
        private readonly ClientViewModel _clientViewModel;
        private readonly CleanerViewModel _cleanerViewModel;
        private CleaningTask? _selectedTask;

        public ObservableCollection<CleaningTask> Tasks { get; } = new();

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

        public ICommand AddTaskCommand { get; }
        public ICommand DeleteTaskCommand { get; }

        public TaskViewModel(IDataService dataService, ClientViewModel clientViewModel, CleanerViewModel cleanerViewModel)
        {
            _dataService = dataService;
            _clientViewModel = clientViewModel;
            _cleanerViewModel = cleanerViewModel;

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
    }
}