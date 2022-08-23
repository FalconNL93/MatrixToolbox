using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MatrixToolbox.Contracts.Services;
using MatrixToolbox.Contracts.ViewModels;
using MatrixToolbox.Core.Models;
using MatrixToolbox.Models;
using MatrixToolbox.Services;
using Microsoft.Extensions.Options;
using Microsoft.UI.Xaml;

namespace MatrixToolbox.ViewModels;

public class SettingsViewModel : ObservableRecipient, INavigationAware
{
    private readonly INavigationService _navigationService;
    private readonly SettingsService _settings;
    private readonly IThemeSelectorService _themeSelectorService;
    private ElementTheme _elementTheme;


    public SettingsViewModel(
        INavigationService navigationService,
        IThemeSelectorService themeSelectorService,
        SettingsService settings,
        IOptionsMonitor<GeneralOptions> generalOptions,
        IOptionsMonitor<ApiOptions> apiOptions
    )
    {
        GeneralOptions = generalOptions.CurrentValue;
        ApiOptions = apiOptions.CurrentValue;

        _navigationService = navigationService;
        _themeSelectorService = themeSelectorService;
        _settings = settings;
        _elementTheme = generalOptions.CurrentValue.Theme;

        SaveSettings = new RelayCommand(() => _settings.Save());
        SwitchThemeCommand = new RelayCommand<ElementTheme>(OnThemeSwitch);
        ReloadCommand = new RelayCommand(() => _navigationService.NavigateTo(GetType().FullName, PageParameters.ReloadPage));
    }

    public RelayCommand ReloadCommand { get; }
    public GeneralOptions GeneralOptions { get; }
    public ApiOptions ApiOptions { get; }

    public RelayCommand SaveSettings { get; }

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