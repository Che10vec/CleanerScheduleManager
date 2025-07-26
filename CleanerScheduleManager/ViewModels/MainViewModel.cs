using CleanerScheduleManager.Services;
using CleanerScheduleManager.Services.Interfaces;
using CleanerScheduleManager.Utilities;
using CleanerScheduleManager.ViewModels.Base;
using CleanerScheduleManager.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace CleanerScheduleManager.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public ICommand NavigateToCleanerCommand { get; }
        public ICommand NavigateToClientCommand { get; }
        public ICommand NavigateToTaskCommand { get; }

        public MainViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            NavigateToCleanerCommand = new RelayCommand(_ =>
                _navigationService.NavigateTo<CleanerView>());

            NavigateToClientCommand = new RelayCommand(_ =>
                _navigationService.NavigateTo<ClientView>());

            NavigateToTaskCommand = new RelayCommand(_ =>
                _navigationService.NavigateTo<TaskView>());
        }

        public void SetFrameReference(Frame frame)
        {
            _navigationService.SetFrame(frame);
        }
    }
}