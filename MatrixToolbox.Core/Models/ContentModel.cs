using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MatrixToolbox.Core.Models;

public static class MessageTypes
{
    public const string Text = "m.text";
}

public class ContentModel : INotifyPropertyChanged
{
    private string _body;
    public string Msgtype { get; set; } = MessageTypes.Text;

    public string Body
    {
        get => _body;
        set => SetField(ref _body, value);
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}