using CleanerScheduleManager.Models;
using CleanerScheduleManager.Services;
using CleanerScheduleManager.Services.Interfaces;
using CleanerScheduleManager.Utilities;
using CleanerScheduleManager.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CleanerScheduleManager.ViewModels
{
    public class CleanerViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private Cleaner? _selectedCleaner;

        public ObservableCollection<Cleaner> Cleaners { get; } = new();

        public Cleaner? SelectedCleaner
        {
            get => _selectedCleaner;
            set => SetProperty(ref _selectedCleaner, value);
        }

        public ICommand AddCleanerCommand { get; }
        public ICommand DeleteCleanerCommand { get; }

        public CleanerViewModel(IDataService dataService)
        {
            _dataService = dataService;

            AddCleanerCommand = new RelayCommand(_ => AddCleaner());
            DeleteCleanerCommand = new RelayCommand(_ => DeleteCleaner(), _ => SelectedCleaner != null);
        }

        private void AddCleaner()
        {
            var cleaner = new Cleaner
            {
                Id = Cleaners.Count + 1,
                Name = "New Cleaner",
                SkillLevel = "Basic",
                IsAvailable = true
            };

            Cleaners.Add(cleaner);
        }

        private void DeleteCleaner()
        {
            if (SelectedCleaner != null)
            {
                Cleaners.Remove(SelectedCleaner);
            }
        }
    }
}