using CleanerScheduleManager.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Threading;
using Xunit;
using IHasPendingEdits = CleanerScheduleManager.ViewModels.Base.ViewModelBase.IHasPendingEdits;

namespace CleanerScheduleManager.Tests;

public class NavigationServiceIntegrationTests
{
    [Theory]
    [InlineData(typeof(DestinationPage), typeof(AlternateDestinationPage))]
    [InlineData(typeof(AlternateDestinationPage), typeof(DestinationPage))]
    public void NavigateTo_FinalizesEditsAndNavigates(Type startType, Type destinationType)
    {
        RunOnStaThread(() =>
        {
            var provider = new ServiceCollection()
                .AddSingleton<NavigationService>()
                .AddTransient<DestinationPage>()
                .AddTransient<AlternateDestinationPage>()
                .BuildServiceProvider();

            var navigationService = provider.GetRequiredService<NavigationService>();
            var frame = new Frame();
            navigationService.SetFrame(frame);

            Navigate(navigationService, startType);
            PumpDispatcher();

            var vm = new PendingViewModel();
            ((Page)frame.Content!).DataContext = vm;

            Navigate(navigationService, destinationType);
            PumpDispatcher();

            Assert.True(vm.Finalized);
            Assert.IsType(destinationType, frame.Content);
        });
    }

    private static void RunOnStaThread(ThreadStart action)
    {
        Exception? threadException = null;
        var thread = new Thread(() =>
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                threadException = ex;
            }
        });

        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join();

        if (threadException != null)
        {
            throw threadException;
        }
    }

    private static void Navigate(NavigationService navigationService, Type pageType) =>
        typeof(NavigationService)
            .GetMethod(nameof(NavigationService.NavigateTo))!
            .MakeGenericMethod(pageType)
            .Invoke(navigationService, null);

    private static void PumpDispatcher()
    {
        var frame = new DispatcherFrame();
        Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() => frame.Continue = false), DispatcherPriority.ApplicationIdle);
        Dispatcher.PushFrame(frame);
    }

    private sealed class PendingViewModel : IHasPendingEdits
    {
        public bool Finalized { get; private set; }
        public void FinalizeEdits() => Finalized = true;
    }

    public sealed class DestinationPage : Page { }
    public sealed class AlternateDestinationPage : Page { }
}