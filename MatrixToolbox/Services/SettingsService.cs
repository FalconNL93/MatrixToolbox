using System.Reflection;
using System.Text;
using MatrixToolbox.Core.Models;
using MatrixToolbox.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace MatrixToolbox.Services;

public class SettingsService
{
    private readonly JsonSerializerSettings _serializerSettings = new();
    public ApiOptions ApiOptions { get; }
    public GeneralOptions GeneralOptions { get; }
    private readonly string? _appDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

    public SettingsService(IOptions<ApiOptions> apiOptions, IOptions<GeneralOptions> generalOptions)
    {
        ApiOptions = apiOptions.Value;
        GeneralOptions = generalOptions.Value;
    }

    public void Save()
    {
        if (_appDirectory == null)
        {
            return;
        }

        var settingsCombined = new SettingsModel
        {
            ApiOptions = ApiOptions,
            GeneralOptions = GeneralOptions
        };
        var fileContent = JsonConvert.SerializeObject(settingsCombined, _serializerSettings);
        File.WriteAllText(Path.Combine(_appDirectory, App.UserConfigurationFile), fileContent, Encoding.UTF8);
    }
}