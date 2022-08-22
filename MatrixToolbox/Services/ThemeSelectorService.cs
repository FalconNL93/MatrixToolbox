using MatrixToolbox.Helpers;
using MatrixToolbox.Models;
using Microsoft.Extensions.Options;
using Microsoft.UI.Xaml;

namespace MatrixToolbox.Services;

public class ThemeSelectorService
{
    private readonly GeneralOptions _options;

    public ThemeSelectorService(IOptions<GeneralOptions> generalOptions)
    {
        _options = generalOptions.Value;
    }

    public ElementTheme Theme { get; set; } = ElementTheme.Default;

    public async Task InitializeAsync()
    {
        Theme = _options.Theme;
        await Task.CompletedTask;
    }

    public async Task SetThemeAsync(ElementTheme theme)
    {
        Theme = theme;

        await SetRequestedThemeAsync();
        _options.Theme = theme;
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
}