using CommunityToolkit.Mvvm.ComponentModel;
using MatrixToolbox.Contracts.Services;
using MatrixToolbox.Contracts.ViewModels;

namespace MatrixToolbox.ViewModels;

public abstract class ViewModelBase : ObservableRecipient, INavigationAware
{
    public void OnNavigatedTo(object parameter)
    {
        var infoBar = App.GetService<IInfoBarService>();
        infoBar.ClearStatus();
    }

    public void OnNavigatedFrom()
    {
    }
}