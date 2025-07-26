using CleanerScheduleManager.Models;
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
    public class ClientViewModel : ViewModelBase
    {
        private Client? _selectedClient;

        public ObservableCollection<Client> Clients { get; } = new();

        public Client? SelectedClient
        {
            get => _selectedClient;
            set => SetProperty(ref _selectedClient, value);
        }

        public ICommand AddClientCommand { get; }
        public ICommand DeleteClientCommand { get; }

        public ClientViewModel()
        {
            AddClientCommand = new RelayCommand(_ => AddClient());
            DeleteClientCommand = new RelayCommand(_ => DeleteClient(), _ => SelectedClient != null);
        }

        private void AddClient()
        {
            var client = new Client
            {
                Id = Clients.Count + 1,
                Name = "New Client",
                Address = "123 Default St",
                Phone = "000-000-0000"
            };

            Clients.Add(client);
        }

        private void DeleteClient()
        {
            if (SelectedClient != null)
            {
                Clients.Remove(SelectedClient);
            }
        }
    }
}
