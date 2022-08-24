using CommunityToolkit.Mvvm.ComponentModel;
using MatrixToolbox.Models;
using Microsoft.UI.Xaml.Controls;

namespace MatrixToolbox.ViewModels;

public abstract class ViewModelBase : ObservableRecipient
{
    public InfoBarModel InfoBarModel { get; set; } = new();

    protected async Task SetStatus(string title, string message, InfoBarSeverity severity = InfoBarSeverity.Informational, int timeout = 10)
    {
        InfoBarModel.Title = title;
        InfoBarModel.Message = message;
        InfoBarModel.IsOpen = true;
        InfoBarModel.IsClosable = false;
        InfoBarModel.Severity = severity;

        if (timeout <= 0)
        {
            return;
        }

        await Task.Delay(TimeSpan.FromSeconds(timeout));
        InfoBarModel.IsOpen = false;
    }
}