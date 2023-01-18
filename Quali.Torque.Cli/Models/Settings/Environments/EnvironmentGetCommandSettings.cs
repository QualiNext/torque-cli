using System.ComponentModel;
using Quali.Torque.Cli.Models.Settings.Blueprints;
using Spectre.Console.Cli;

namespace Quali.Torque.Cli.Models.Settings.Environments;

public class EnvironmentGetCommandSettings: DetailedCommandSettings
{
    [CommandArgument(0, "<ENVIRONMENT-ID>")]
    [Description("The environment id to show")]
    public string EnvironmentId { get; set; }
}