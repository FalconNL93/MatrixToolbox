using System.Reflection;
using System.Text;
using MatrixToolbox.Core.Models;
using MatrixToolbox.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace MatrixToolbox.Services;

public class SettingsService
{
    private readonly ApiOptions _apiOptions;
    private readonly string? _appDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    private readonly GeneralOptions _generalOptions;
    private readonly JsonSerializerSettings _serializerSettings = new();

    public SettingsService(IOptions<ApiOptions> apiOptions, IOptions<GeneralOptions> generalOptions)
    {
        _apiOptions = apiOptions.Value;
        _generalOptions = generalOptions.Value;
    }

    public void Save()
    {
        if (_appDirectory == null)
        {
            return;
        }

        var settingsCombined = new SettingsModel
        {
            ApiOptions = _apiOptions,
            GeneralOptions = _generalOptions
        };
        var fileContent = JsonConvert.SerializeObject(settingsCombined, _serializerSettings);
        File.WriteAllText(Path.Combine(_appDirectory, App.UserConfigurationFile), fileContent, Encoding.UTF8);
    }
}