using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MatrixToolbox.Core.Models;

public class ServiceNotice : INotifyPropertyChanged
{
    private ContentModel _content = new();
    private string _userId;

    public string UserId
    {
        get => _userId;
        set => SetField(ref _userId, value);
    }

    public ContentModel Content
    {
        get => _content;
        set => SetField(ref _content, value);
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return false;
        }

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class ServiceNoticeResult
{
    public string EventId { get; set; }
}