using Newtonsoft.Json;

namespace Quali.Torque.Cli.Models;

public class Error
{
    [JsonProperty("message")]
    public string Message { get; set; }
    [JsonProperty("code")]
    public string Code { get; set; }
    [JsonProperty("name")]
    public string Name { get; set; }
}

public class TorqueApiErrorResponse
{
    [JsonProperty("errors")]
    public List<Error> Errors { get; set; }
}