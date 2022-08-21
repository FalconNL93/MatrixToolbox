using System.ComponentModel;
using System.Runtime.CompilerServices;
using MatrixToolbox.Core.Contracts;

namespace MatrixToolbox.Core.Models;

public class RoomsModel : IMatrixResult
{
    public List<RoomModel> Rooms { get; set; }
}

public class RoomModel : IMatrixResult, INotifyPropertyChanged
{
    private string _name;
    private string _roomId;

    public string RoomId
    {
        get => _roomId;
        set => SetField(ref _roomId, value);
    }

    public string Name
    {
        get => _name;
        set => SetField(ref _name, value);
    }

    public string CanonicalAlias { get; set; }
    public int JoinedMembers { get; set; }
    public int JoinedLocalMembers { get; set; }
    public string Version { get; set; }
    public string Creator { get; set; }
    public object Encryption { get; set; }
    public bool Federatable { get; set; }
    public bool Public { get; set; }
    public string JoinRules { get; set; }
    public object GuestAccess { get; set; }
    public string HistoryVisibility { get; set; }
    public int StateEvents { get; set; }
    public string RoomType { get; set; }
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