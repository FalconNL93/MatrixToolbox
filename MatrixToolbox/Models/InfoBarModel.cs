using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.UI.Xaml.Controls;

namespace MatrixToolbox.Models;

public class InfoBarModel
{
    private bool _isClosable;
    private bool _isOpen;
    private string _message;
    private InfoBarSeverity _severity = InfoBarSeverity.Informational;
    private string _title;


    public string Title
    {
        get => _title;
        set => SetField(ref _title, value);
    }

    public bool IsOpen
    {
        get => _isOpen;
        set => SetField(ref _isOpen, value);
    }

    public string Message
    {
        get => _message;
        set => SetField(ref _message, value);
    }

    public InfoBarSeverity Severity
    {
        get => _severity;
        set => SetField(ref _severity, value);
    }

    public bool IsClosable
    {
        get => _isClosable;
        set => SetField(ref _isClosable, value);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return false;
        }

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}