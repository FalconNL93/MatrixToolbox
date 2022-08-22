using System.Diagnostics;
using System.Reflection;
using System.Windows.Input;
using Windows.ApplicationModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MatrixToolbox.Contracts.Services;
using MatrixToolbox.Core.Contracts.Services;
using MatrixToolbox.Core.Models;
using MatrixToolbox.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.UI.Xaml;

namespace MatrixToolbox.ViewModels;

public class SettingsViewModel : ObservableRecipient
{
    private readonly IFileService _fileService;
    private readonly IThemeSelectorService _themeSelectorService;
    private ElementTheme _elementTheme;
    private string _versionDescription;

    public SettingsViewModel(IThemeSelectorService themeSelectorService, IOptions<MatrixApiOptions> apiOptions, IFileService fileService)
    {
        _themeSelectorService = themeSelectorService;
        _fileService = fileService;
        MatrixApiOptions = apiOptions.Value;
        _elementTheme = _themeSelectorService.Theme;
        _versionDescription = GetVersionDescription();

        SaveMatrixOptions = new RelayCommand(OnSaveOptions);
        SwitchThemeCommand = new RelayCommand<ElementTheme>(OnThemeSwitch);
    }

    public MatrixApiOptions MatrixApiOptions { get; }
    public RelayCommand SaveMatrixOptions { get; }

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
        try
        {
            _fileService.Save(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "appsettings.user.json", new {MatrixApiOptions});
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            throw;
        }
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