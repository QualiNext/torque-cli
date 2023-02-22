using Newtonsoft.Json;

namespace Quali.Torque.Cli.Models;

public class UserProfile
{
    [JsonProperty("name")]
    public string Name { get; set; }
    [JsonProperty("token")]
    public string Token { get; set; }
    [JsonProperty("space")]
    public string Space { get; set; }
    [JsonProperty("repository")]
    public string RepositoryName { get; set; }
    [JsonProperty("baseUrl")]
    public string BaseUrl { get; set; }
}