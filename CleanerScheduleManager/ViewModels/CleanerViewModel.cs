using CleanerScheduleManager.Models;
using CleanerScheduleManager.Models.Enums;
using CleanerScheduleManager.Services;
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
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using static CleanerScheduleManager.ViewModels.Base.ViewModelBase;

namespace CleanerScheduleManager.ViewModels
{
    public class CleanerViewModel : ViewModelBase, IHasPendingEdits, IPersistable
    {
        private readonly IDataService _dataService;
        private readonly string _dataFilePath = Path.Combine(AppContext.BaseDirectory, "cleaners.json");
        private string _searchText = string.Empty;
        private Cleaner? _selectedCleaner;
        public ObservableCollection<Cleaner> Cleaners { get; } = new();

        public IEnumerable<CleanerSkillLevel> SkillLevels { get; } = Enum.GetValues<CleanerSkillLevel>();
        public ICollectionView CleanersView { get; }

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
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    CleanersView.Refresh();
                }
            }
        }

        public ICommand AddCleanerCommand { get; }
        public ICommand DeleteCleanerCommand { get; }

        public CleanerViewModel(IDataService dataService)
        {
            _dataService = dataService;

            CleanersView = CollectionViewSource.GetDefaultView(Cleaners);
            CleanersView.Filter = FilterCleaners;

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

        public void FinalizeEdits()
        {
            IEditableCollectionView? editable = CollectionViewSource.GetDefaultView(Cleaners) as IEditableCollectionView;
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
            Cleaners.Clear();
            var cleaners = await _dataService.LoadAsync<Cleaner>(_dataFilePath);
            foreach (var cleaner in cleaners)
            {
                Cleaners.Add(cleaner);
            }
        }

        public Task SaveDataAsync()
        {
            FinalizeEdits();
            return _dataService.SaveAsync(Cleaners, _dataFilePath);
        }

        private bool FilterCleaners(object obj)
        {
            if (obj is not Cleaner cleaner)
                return false;

            if (string.IsNullOrWhiteSpace(SearchText))
                return true;

            string search = SearchText.ToLowerInvariant();
            string combined = ($"{cleaner.Id} {cleaner.Name} {cleaner.SkillLevel} {cleaner.IsAvailable}").ToLowerInvariant();
            return combined.Contains(search);
        }
    }
}