using MatrixToolbox.Contracts.Services;
using MatrixToolbox.Helpers;
using Microsoft.UI.Xaml;

namespace MatrixToolbox.Services;

public class ThemeSelectorService : IThemeSelectorService
{
    private const string SettingsKey = "AppBackgroundRequestedTheme";

    private readonly SettingsService _settingsService;

    public ThemeSelectorService(SettingsService settingsService)
    {
        _settingsService = settingsService;
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
        var theme = _settingsService.GeneralOptions.Theme;

        return theme;
    }

    private async Task SaveThemeInSettingsAsync(ElementTheme theme)
    {
        _settingsService.GeneralOptions.Theme = theme;
    }
}