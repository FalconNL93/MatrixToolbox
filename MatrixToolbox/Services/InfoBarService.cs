using CommunityToolkit.Mvvm.Messaging;
using MatrixToolbox.Contracts.Services;
using MatrixToolbox.Messages;
using MatrixToolbox.Models;
using Microsoft.UI.Xaml.Controls;

namespace MatrixToolbox.Services;

public class InfoBarService : IInfoBarService
{
    private readonly WeakReferenceMessenger _messenger = WeakReferenceMessenger.Default;

    public async void SetStatus(string title, string message, InfoBarSeverity severity = InfoBarSeverity.Informational, int timeout = 0)
    {
        _messenger.Send(new SetUpdateInfoBarMessage(new InfoBarModel
        {
            Title = title,
            Message = message,
            IsOpen = true,
            IsClosable = false,
            Severity = severity,
        }));

        if (timeout <= 0)
        {
            return;
        }

        await Task.Delay(TimeSpan.FromSeconds(timeout));
        ClearStatus();
    }

    public void ClearStatus() => _messenger.Send(new SetUpdateInfoBarMessage(new InfoBarModel {IsOpen = false}));
}