using Torque.Cli.Api;

namespace Quali.Torque.Cli.Models;

public sealed class EnvironmentWaiterData
{
    public string Space { get; set; }
    public string EnvironmentId { get; set; }
    public TorqueApiClient Client { get; set; }
    public int Timeout { get; set; }
}