using MatrixToolbox.Contracts.Services;
using MatrixToolbox.Helpers;
using MatrixToolbox.Models;
using Microsoft.Extensions.Options;
using Microsoft.UI.Xaml;

namespace MatrixToolbox.Services;

public class ThemeSelectorService : IThemeSelectorService
{
    private const string SettingsKey = "AppBackgroundRequestedTheme";

    private readonly GeneralOptions _generalOptions;

    public ThemeSelectorService(IOptions<GeneralOptions> generalOptions)
    {
        _generalOptions = generalOptions.Value;
    }

    public ElementTheme Theme { get; set; } = ElementTheme.Default;

    public async Task InitializeAsync()
    {
        Theme = await LoadThemeFromSettingsAsync();
        await Task.CompletedTask;
    }

    public async Task SetThemeAsync(ElementTheme theme)
    {
        Theme = theme;

        await SetRequestedThemeAsync();
        await SaveThemeInSettingsAsync(Theme);
    }

    public async Task SetRequestedThemeAsync()
    {
        if (App.MainWindow.Content is FrameworkElement rootElement)
        {
            rootElement.RequestedTheme = Theme;

            TitleBarHelper.UpdateTitleBar(Theme);
        }

        await Task.CompletedTask;
    }

    private async Task<ElementTheme> LoadThemeFromSettingsAsync()
    {
        return _generalOptions.Theme;
    }

    private async Task SaveThemeInSettingsAsync(ElementTheme theme)
    {
        _generalOptions.Theme = theme;
    }
}