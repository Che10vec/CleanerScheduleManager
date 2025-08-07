using CleanerScheduleManager.Models;
using CleanerScheduleManager.Services.Interfaces;
using CleanerScheduleManager.Utilities;
using CleanerScheduleManager.ViewModels.Base;
using CleanerScheduleManager.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using static CleanerScheduleManager.ViewModels.Base.ViewModelBase;

namespace CleanerScheduleManager.ViewModels
{
    public class ClientViewModel : ViewModelBase, IHasPendingEdits, IPersistable
    {
        private readonly IDataService _dataService;
        private readonly string _dataFilePath = Path.Combine(AppContext.BaseDirectory, "clients.json");
        private Client? _selectedClient;

        public ObservableCollection<Client> Clients { get; } = new();

        public Client? SelectedClient
        {
            get => _selectedClient;
            set
            {
                if (SetProperty(ref _selectedClient, value))
                {
                    ((RelayCommand)DeleteClientCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public ICommand AddClientCommand { get; }
        public ICommand DeleteClientCommand { get; }

        public ClientViewModel(IDataService dataService)
        {
            _dataService = dataService;

            AddClientCommand = new RelayCommand(_ => AddClient());
            DeleteClientCommand = new RelayCommand(_ => DeleteClient(), _ => CanDeleteClient());
        }

        private void AddClient()
        {
            var client = new Client
            {
                Id = Clients.Count + 1,
                Name = "New Client",
                Address = "123 Default st.",
                Phone = "000-000-0000"
            };

            Clients.Add(client);
        }

        private void DeleteClient()
        {
            if (SelectedClient != null)
            {
                Clients.Remove(SelectedClient);
                SelectedClient = null;
            }
        }

        private bool CanDeleteClient()
        {
            return SelectedClient != null;
        }

        public void FinalizeEdits()
        {
            IEditableCollectionView? editable = CollectionViewSource.GetDefaultView(Clients) as IEditableCollectionView;
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
            Clients.Clear();
            var clients = await _dataService.LoadAsync<Client>(_dataFilePath);
            foreach (var client in clients)
            {
                Clients.Add(client);
            }
        }

        public Task SaveDataAsync()
        {
            FinalizeEdits();
            return _dataService.SaveAsync(Clients, _dataFilePath);
        }
    }
}