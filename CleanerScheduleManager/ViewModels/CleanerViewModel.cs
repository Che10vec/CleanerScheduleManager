using CleanerScheduleManager.Models;
using CleanerScheduleManager.Models.Enums;
using CleanerScheduleManager.Services;
using CleanerScheduleManager.Services.Interfaces;
using CleanerScheduleManager.Utilities;
using CleanerScheduleManager.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CleanerScheduleManager.ViewModels
{
    public class CleanerViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private Cleaner? _selectedCleaner;

        public ObservableCollection<Cleaner> Cleaners { get; } = new();

        public IEnumerable<CleanerSkillLevel> SkillLevels { get; } = Enum.GetValues<CleanerSkillLevel>();

        public Cleaner? SelectedCleaner
        {
            get => _selectedCleaner;
            set
            {
                if (SetProperty(ref _selectedCleaner, value))
                {
                    ((RelayCommand)DeleteCleanerCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public ICommand AddCleanerCommand { get; }
        public ICommand DeleteCleanerCommand { get; }

        public CleanerViewModel(IDataService dataService)
        {
            _dataService = dataService;

            AddCleanerCommand = new RelayCommand(_ => AddCleaner());
            DeleteCleanerCommand = new RelayCommand(_ => DeleteCleaner(), _ => CanDeleteCleaner());
        }

        private void AddCleaner()
        {
            Cleaner cleaner = new Cleaner
            {
                Id = Cleaners.Count + 1,
                Name = "New Cleaner",
                SkillLevel = CleanerSkillLevel.Beginner,
                IsAvailable = true
            };

            Cleaners.Add(cleaner);
        }

        private void DeleteCleaner()
        {
            Cleaners.Remove(SelectedCleaner);
            SelectedCleaner = null;
        }

        private bool CanDeleteCleaner()
        {
            return SelectedCleaner != null;
        }
    }
}