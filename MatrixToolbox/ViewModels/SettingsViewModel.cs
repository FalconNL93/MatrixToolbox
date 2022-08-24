using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using MatrixToolbox.Contracts.Services;
using MatrixToolbox.Contracts.ViewModels;
using MatrixToolbox.Core.Models;
using MatrixToolbox.Core.Services;
using MatrixToolbox.Models;
using MatrixToolbox.Services;
using Microsoft.Extensions.Options;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace MatrixToolbox.ViewModels;

public class SettingsViewModel : ViewModelBase, INavigationAware
{
    private readonly AdminService _adminService;
    private readonly IInfoBarService _infoBarService;
    private readonly INavigationService _navigationService;
    private readonly SettingsService _settings;
    private readonly IThemeSelectorService _themeSelectorService;
    private ElementTheme _elementTheme;
    private VersionModel _versionResponse;

    public SettingsViewModel(
        INavigationService navigationService,
        IThemeSelectorService themeSelectorService,
        SettingsService settings,
        IOptionsMonitor<GeneralOptions> generalOptions,
        IOptionsMonitor<ApiOptions> apiOptions,
        AdminService adminService,
        IInfoBarService infoBarService
    )
    {
        GeneralOptions = generalOptions.CurrentValue;
        ApiOptions = apiOptions.CurrentValue;

        _navigationService = navigationService;
        _themeSelectorService = themeSelectorService;
        _settings = settings;
        _adminService = adminService;
        _infoBarService = infoBarService;
        _elementTheme = generalOptions.CurrentValue.Theme;

        SaveSettings = new RelayCommand(() => _settings.Save());
        SwitchThemeCommand = new RelayCommand<ElementTheme>(OnThemeSwitch);
        ReloadCommand = new RelayCommand(OnReloadSettings);
        TestApiSettings = new AsyncRelayCommand(OnTestApiSettings);
    }

    public VersionModel VersionResponse
    {
        get => _versionResponse;
        set => SetProperty(ref _versionResponse, value);
    }

    public RelayCommand ReloadCommand { get; }
    public GeneralOptions GeneralOptions { get; }
    public ApiOptions ApiOptions { get; }

    public RelayCommand SaveSettings { get; }
    public AsyncRelayCommand TestApiSettings { get; }

    public ElementTheme ElementTheme
    {
        get => _elementTheme;
        set => SetProperty(ref _elementTheme, value);
    }

    public ICommand SwitchThemeCommand { get; }


    public async void OnNavigatedTo(object parameter)
    {
        if (parameter is PageParameters.ReloadPage)
        {
            await _themeSelectorService.SetThemeAsync(ElementTheme);
        }
    }

    public void OnNavigatedFrom()
    {
    }

    private async Task OnTestApiSettings()
    {
        _infoBarService.SetStatus("Testing", "Testing API Connection...");
        VersionResponse = await _adminService.GetVersion();

        _infoBarService.SetStatus("OK", "API Connection successful", InfoBarSeverity.Success, 5);
    }

    private void OnReloadSettings()
    {
        _navigationService.NavigateTo(typeof(SettingsViewModel).FullName, PageParameters.ReloadPage);
    }

    private async void OnThemeSwitch(ElementTheme param)
    {
        if (ElementTheme == param)
        {
            return;
        }

        ElementTheme = param;
        await _themeSelectorService.SetThemeAsync(param);
    }
}