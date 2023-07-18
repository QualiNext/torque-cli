using Newtonsoft.Json;

namespace Quali.Torque.Cli.Models;

public class TorqueConfiguration
{
    [JsonProperty("active")] public string ActiveProfile { get; set; }

    [JsonProperty("profiles")] public List<UserProfile> Profiles { get; set; }

    public static TorqueConfiguration CreateEmpty() =>
        new TorqueConfiguration
        {
            ActiveProfile = null,
            Profiles = new List<UserProfile>()
        };
}