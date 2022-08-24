using CommunityToolkit.Mvvm.Messaging;
using MatrixToolbox.Contracts.Services;
using MatrixToolbox.Messages;
using MatrixToolbox.Models;
using MatrixToolbox.Views;
using Microsoft.UI.Xaml.Controls;

namespace MatrixToolbox.Services;

public class InfoBarService : IInfoBarService
{
    private const string MessageToken = nameof(ShellPage);
    private readonly WeakReferenceMessenger _messenger = WeakReferenceMessenger.Default;

    public async void SetStatus(
        string title,
        string message,
        InfoBarSeverity severity = InfoBarSeverity.Informational,
        bool isClosable = false,
        int timeout = 0
    )
    {
        _messenger.Send(new UpdateInfoBarMessage(new InfoBarModel
        {
            Title = title,
            Message = message,
            IsOpen = true,
            IsClosable = isClosable,
            Severity = severity
        }), MessageToken);

        if (timeout <= 0)
        {
            return;
        }

        await Task.Delay(TimeSpan.FromSeconds(timeout));
        ClearStatus();
    }

    public void ClearStatus()
    {
        _messenger.Send(new UpdateInfoBarMessage(new InfoBarModel {IsOpen = false}), MessageToken);
    }
}