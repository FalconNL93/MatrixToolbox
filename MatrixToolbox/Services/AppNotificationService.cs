using System.Collections.Specialized;
using System.Web;
using MatrixToolbox.Contracts.Services;
using Microsoft.Windows.AppNotifications;

namespace MatrixToolbox.Notifications;

public class AppNotificationService : IAppNotificationService
{
    private readonly INavigationService _navigationService;

    public AppNotificationService(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    public void Initialize()
    {
        AppNotificationManager.Default.NotificationInvoked += OnNotificationInvoked;

        AppNotificationManager.Default.Register();
    }

    public bool Show(string payload)
    {
        var appNotification = new AppNotification(payload);

        AppNotificationManager.Default.Show(appNotification);

        return appNotification.Id != 0;
    }

    public NameValueCollection ParseArguments(string arguments)
    {
        return HttpUtility.ParseQueryString(arguments);
    }

    public void Unregister()
    {
        AppNotificationManager.Default.Unregister();
    }

    ~AppNotificationService()
    {
        Unregister();
    }

    public void OnNotificationInvoked(AppNotificationManager sender, AppNotificationActivatedEventArgs args)
    {
        //// }
        ////    });
        ////        _navigationService.NavigateTo(typeof(SettingsViewModel).FullName!);
        ////    {
        ////    App.MainWindow.DispatcherQueue.TryEnqueue(() =>
        //// {
        //// if (ParseArguments(args.Argument)["action"] == "Settings")
        //// // Navigate to a specific page based on the notification arguments.
        // TODO: Handle notification invocations when your app is already running.
        App.MainWindow.DispatcherQueue.TryEnqueue(() =>
        {
            App.MainWindow.ShowMessageDialogAsync("TODO: Handle notification invocations when your app is already running.", "Notification Invoked");

            App.MainWindow.BringToFront();
        });
    }
}