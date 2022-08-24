using CommunityToolkit.Mvvm.Messaging;
using MatrixToolbox.Messages;
using MatrixToolbox.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace MatrixToolbox.Views;

public sealed partial class SettingsPage : Page
{
    public SettingsPage()
    {
        ViewModel = App.GetService<SettingsViewModel>();
        InitializeComponent();
    }

    public SettingsViewModel ViewModel { get; }
}