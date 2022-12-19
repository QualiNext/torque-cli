using Newtonsoft.Json;

namespace Torque.Cli.Models;

public class UserProfile
{
    [JsonProperty]
    public string? Token { get; set; }
    [JsonProperty]
    public string? Space { get; set; }
    [JsonProperty]
    public string? RepositoryName { get; set; }
}