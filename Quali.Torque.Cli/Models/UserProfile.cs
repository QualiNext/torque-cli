using Newtonsoft.Json;

namespace Quali.Torque.Cli.Models;

public class UserProfile
{
    [JsonProperty]
    public string Name { get; set; }
    [JsonProperty]
    public string Token { get; set; }
    [JsonProperty]
    public string Space { get; set; }
    [JsonProperty]
    public string RepositoryName { get; set; }
    [JsonProperty]
    public string BaseUrl { get; set; }
}