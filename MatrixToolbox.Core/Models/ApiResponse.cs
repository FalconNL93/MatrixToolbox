using Newtonsoft.Json;

namespace MatrixToolbox.Core.Models;

public class ApiResponse<T>
{
    [JsonProperty("errcode")]
    public string Response { get; set; }
    
    [JsonIgnore]
    public string Raw { get; set; }

    [JsonIgnore]
    public T Result { get; set; }
}