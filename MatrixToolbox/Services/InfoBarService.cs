using CommunityToolkit.Mvvm.Messaging;
using MatrixToolbox.Contracts.Services;
using MatrixToolbox.Messages;
using MatrixToolbox.Models;
using Microsoft.UI.Xaml.Controls;

namespace MatrixToolbox.Services;

public class InfoBarService : IInfoBarService
{
    public void SetStatus(string title, string message, InfoBarSeverity severity = InfoBarSeverity.Informational, int timeout = 10)
    {
        var infoBarModel = new InfoBarModel
        {
            Title = title,
            Message = message,
            IsOpen = true,
            IsClosable = false,
            Severity = severity,
        };

        WeakReferenceMessenger.Default.Send(new SetUpdateInfoBarMessage(infoBarModel));
    }
    
    public void ClearStatus()
    {
        WeakReferenceMessenger.Default.Send(new SetUpdateInfoBarMessage(new InfoBarModel
        {
            IsOpen = false
        }));
    }
}