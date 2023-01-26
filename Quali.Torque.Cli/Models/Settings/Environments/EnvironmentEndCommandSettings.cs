using System.ComponentModel;
using Quali.Torque.Cli.Models.Settings.Base;
using Spectre.Console.Cli;

namespace Quali.Torque.Cli.Models.Settings.Environments;

public class EnvironmentEndCommandSettings: UserContextSettings
{
    [CommandArgument(0, "<ENVIRONMENT-ID>")]
    [Description("The environment id to stop")]
    public string EnvironmentId { get; set; }
}