using System.Reflection;
using System.Windows.Input;
using Windows.ApplicationModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MatrixToolbox.Contracts.Services;
using MatrixToolbox.Helpers;
using MatrixToolbox.Services;
using Microsoft.UI.Xaml;

namespace MatrixToolbox.ViewModels;

public class SettingsViewModel : ObservableRecipient
{
    private readonly IThemeSelectorService _themeSelectorService;
    public SettingsService Settings { get; }
    private ElementTheme _elementTheme;
    private string _versionDescription;

    public SettingsViewModel(
        IThemeSelectorService themeSelectorService,
        SettingsService settings)
    {
        _themeSelectorService = themeSelectorService;
        Settings = settings;
        _elementTheme = _themeSelectorService.Theme;
        _versionDescription = GetVersionDescription();

        SaveSettings = new RelayCommand(OnSaveOptions);
        SwitchThemeCommand = new RelayCommand<ElementTheme>(OnThemeSwitch);
    }

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
        Settings.Save();
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