using System.Diagnostics;
using MatrixToolbox.Activation;
using MatrixToolbox.Contracts.Services;
using MatrixToolbox.Core.Models;
using MatrixToolbox.Core.Services;
using MatrixToolbox.Models;
using MatrixToolbox.Services;
using MatrixToolbox.ViewModels;
using MatrixToolbox.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using UnhandledExceptionEventArgs = Microsoft.UI.Xaml.UnhandledExceptionEventArgs;

namespace MatrixToolbox;

public partial class App : Application
{
    public const string UserConfigurationFile = "config.json";

    public App()
    {
        try
        {
            InitializeComponent();

            Host = Microsoft.Extensions.Hosting.Host
                .CreateDefaultBuilder()
                .UseContentRoot(AppContext.BaseDirectory)
                .ConfigureAppConfiguration((_, config) =>
                {
                    config.AddJsonFile(UserConfigurationFile, true, true);
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

                    services.AddTransient<IActivationHandler, AppNotificationActivationHandler>();

                    services.AddSingleton<IAppNotificationService, AppNotificationService>();
                    services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
                    services.AddSingleton<IActivationService, ActivationService>();
                    services.AddSingleton<IPageService, PageService>();
                    services.AddSingleton<INavigationService, NavigationService>();
                    services.AddTransient<INavigationViewService, NavigationViewService>();

                    services.AddTransient<SettingsViewModel>();
                    services.AddTransient<SettingsPage>();
                    services.AddTransient<MainViewModel>();
                    services.AddTransient<MainPage>();
                    services.AddTransient<ShellPage>();
                    services.AddTransient<ShellViewModel>();

                    services.AddSingleton<SettingsService>();
                    services.AddHttpClient<AdminService>();

                    services.Configure<GeneralOptions>(context.Configuration.GetSection(nameof(GeneralOptions)));
                    services.Configure<ApiOptions>(context.Configuration.GetSection(nameof(ApiOptions)));
                }).Build();

            GetService<IAppNotificationService>().Initialize();

            UnhandledException += App_UnhandledException;
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            throw;
        }
    }

    private IHost Host { get; }

    public static WindowEx MainWindow { get; } = new MainWindow();

    public static T GetService<T>()
        where T : class
    {
        if ((Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    private void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        Debug.WriteLine(e);
    }

    protected override async void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);

        //GetService<IAppNotificationService>().Show(string.Format("AppNotificationSamplePayload".GetLocalized(), AppContext.BaseDirectory));

        await GetService<IActivationService>().ActivateAsync(args);
    }
}