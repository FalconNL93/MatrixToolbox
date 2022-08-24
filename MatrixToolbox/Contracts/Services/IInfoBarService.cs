using Microsoft.UI.Xaml.Controls;

namespace MatrixToolbox.Contracts.Services;

public interface IInfoBarService
{
    void SetStatus(string title, string message, InfoBarSeverity severity = InfoBarSeverity.Informational, bool isClosable = false, int timeout = 0);
    void ClearStatus();
}