using System.Net.Http.Headers;
using System.Text;
using MatrixToolbox.Core.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MatrixToolbox.Core.Services;

public class AdminService
{
    private readonly ApiOptions _apiOptions;
    private readonly HttpClient _client;
    private readonly JsonSerializerSettings _serializerOptions = new() {ContractResolver = new DefaultContractResolver {NamingStrategy = new SnakeCaseNamingStrategy()}};

    public AdminService(HttpClient client, IOptions<ApiOptions> apiOptions)
    {
        _client = client;
        _apiOptions = apiOptions.Value;

        if (string.IsNullOrEmpty(_apiOptions.Server) || string.IsNullOrEmpty(_apiOptions.AccessToken))
        {
            return;
        }

        _client.BaseAddress = new Uri(_apiOptions.Server);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiOptions.AccessToken);
    }

    private async Task<HttpResponseMessage> GetV1(string requestUri)
    {
        return await _client.GetAsync($"/_synapse/admin/v1/{requestUri}");
    }

    private async Task<T> GetV1<T>(string requestUri) where T : class
    {
        var response = await GetV1(requestUri);
        return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
    }

    private async Task<HttpResponseMessage> PostV1(string requestUri, object content)
    {
        var body = JsonConvert.SerializeObject(content, _serializerOptions);
        return await _client.PostAsync($"/_synapse/admin/v1/{requestUri}", new StringContent(body, Encoding.UTF8, "application/json"
        ));
    }

    public async Task<RoomsModel> GetRooms()
    {
        return await GetV1<RoomsModel>("rooms");
    }

    public async Task<ApiResponse<ServiceNoticeResult>> PostServiceNotice(ServiceNotice serviceNotice)
    {
        var request = await PostV1("send_server_notice", serviceNotice);

        return new ApiResponse<ServiceNoticeResult>
        {
            Raw = await request.Content.ReadAsStringAsync(),
            Result = new ServiceNoticeResult()
        };
    }
}