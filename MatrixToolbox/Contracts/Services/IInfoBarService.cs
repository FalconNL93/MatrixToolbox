using Microsoft.UI.Xaml.Controls;

namespace MatrixToolbox.Contracts.Services;

public interface IInfoBarService
{
    void SetStatus(string title, string message, InfoBarSeverity severity = InfoBarSeverity.Informational, int timeout = 10);
    void ClearStatus();
}