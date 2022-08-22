using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MatrixToolbox.Core.Models;
using MatrixToolbox.Core.Services;
using Microsoft.Extensions.Options;

namespace MatrixToolbox.ViewModels;

public class MainViewModel : ObservableRecipient
{
    private readonly AdminService _adminService;
    private readonly ApiOptions _apiOptions;
    private string _response = string.Empty;
    private ServiceNotice _serviceNotice = new();


    public MainViewModel(AdminService adminService, IOptions<ApiOptions> apiOptions)
    {
        _adminService = adminService;
        RefreshCommand = new AsyncRelayCommand(OnRefresh, () => ApiSettingsValid);
        ClearCommand = new RelayCommand(OnClear, () => ApiSettingsValid);
        PostServiceNotice = new AsyncRelayCommand<ServiceNotice>(OnPostServiceNotice, _ => ApiSettingsValid);
        _apiOptions = apiOptions.Value;

        if (!string.IsNullOrEmpty(_apiOptions.TestUser))
        {
            _serviceNotice.UserId = _apiOptions.TestUser;
        }
    }

    public ObservableCollection<RoomModel> Rooms { get; } = new();
    public AsyncRelayCommand RefreshCommand { get; }
    public RelayCommand ClearCommand { get; }
    public AsyncRelayCommand<ServiceNotice> PostServiceNotice { get; }

    public string Response
    {
        get => _response;
        set => SetProperty(ref _response, value);
    }

    public ServiceNotice ServiceNotice
    {
        get => _serviceNotice;
        set => SetProperty(ref _serviceNotice, value);
    }

    public bool ApiSettingsValid => !string.IsNullOrEmpty(_apiOptions.Server) && !string.IsNullOrEmpty(_apiOptions.AccessToken);

    private async Task OnPostServiceNotice(ServiceNotice? arg)
    {
        var request = await _adminService.PostServiceNotice(arg);

        Response = request.Raw;
    }

    private async Task OnRefresh()
    {
        var rooms = await _adminService.GetRooms();

        try
        {
            Rooms.Clear();
            foreach (var room in rooms.Rooms.Where(room => !string.IsNullOrEmpty(room.Name)))
            {
                Rooms.Add(room);
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            throw;
        }
    }

    private void OnClear()
    {
        Rooms.Clear();
    }
}