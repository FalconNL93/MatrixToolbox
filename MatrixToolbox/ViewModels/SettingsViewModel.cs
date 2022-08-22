using System.Reflection;
using System.Windows.Input;
using Windows.ApplicationModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MatrixToolbox.Core.Models;
using MatrixToolbox.Helpers;
using MatrixToolbox.Models;
using MatrixToolbox.Services;
using Microsoft.Extensions.Options;
using Microsoft.UI.Xaml;

namespace MatrixToolbox.ViewModels;

public class SettingsViewModel : ObservableRecipient
{
    private readonly SettingsService _settings;
    private readonly ThemeSelectorService _themeSelectorService;
    private ElementTheme _elementTheme;
    private string _versionDescription;

    public SettingsViewModel(
        ThemeSelectorService themeSelectorService,
        SettingsService settings,
        IOptions<GeneralOptions> generalOptions,
        IOptions<ApiOptions> apiOptions
    )
    {
        _themeSelectorService = themeSelectorService;
        _settings = settings;
        GeneralOptions = generalOptions.Value;
        ApiOptions = apiOptions.Value;
        _elementTheme = _themeSelectorService.Theme;
        _versionDescription = GetVersionDescription();

        SaveSettings = new RelayCommand(OnSaveOptions);
        SwitchThemeCommand = new RelayCommand<ElementTheme>(OnThemeSwitch);
    }

    public GeneralOptions GeneralOptions { get; }
    public ApiOptions ApiOptions { get; }

    public RelayCommand SaveSettings { get; }

    public ElementTheme ElementTheme
    {
        get => _elementTheme;
        set => SetProperty(ref _elementTheme, value);
    }

    public string VersionDescription
    {
        get => _versionDescription;
        set => SetProperty(ref _versionDescription, value);
    }

    public ICommand SwitchThemeCommand { get; }

    private void OnSaveOptions()
    {
        _settings.Save();
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

    private static string GetVersionDescription()
    {
        Version version;

        if (RuntimeHelper.IsMSIX)
        {
            var packageVersion = Package.Current.Id.Version;

            version = new Version(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
        }
        else
        {
            version = Assembly.GetExecutingAssembly().GetName().Version!;
        }

        return $"{"AppDisplayName".GetLocalized()} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }
}