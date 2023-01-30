using System.ComponentModel;
using Quali.Torque.Cli.Models.Settings.Base;
using Spectre.Console.Cli;

namespace Quali.Torque.Cli.Models.Settings.Environments;

public class EnvironmentExtendCommandSettings: UserContextSettings
{
    [CommandArgument(0, "<ENVIRONMENT-ID>")]
    [Description("The environment id to extend")]
    public string EnvironmentId { get; set; }
    
    [CommandOption("-d|--duration <MINUTES>")]
    [Description("The Environment will automatically de-provision at the end of the provided duration (minutes).")]
    [DefaultValue(120)]
    public int Duration { get; set; }
}