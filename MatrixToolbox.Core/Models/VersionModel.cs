using Newtonsoft.Json;

namespace MatrixToolbox.Core.Models;

public class VersionModel
{
    [JsonProperty("server_version")]
    public string ServerVersion { get; set; }

    public string PythonVersion { get; set; }
}