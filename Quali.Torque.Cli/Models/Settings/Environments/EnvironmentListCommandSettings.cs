using System.ComponentModel;
using Quali.Torque.Cli.Models.Settings.Blueprints;
using Spectre.Console.Cli;

namespace Quali.Torque.Cli.Models.Settings.Environments;

public class EnvironmentListCommandSettings: DetailedCommandSettings
{
    [CommandOption("--show-ended")]
    [Description("Show ended environments")]
    public bool ShowEnded { get; set; }
    
    [CommandOption("--count <N>")]
    [Description("Amount of environment to be retrieved")]
    [DefaultValue(20)]
    public int Count { get; set; }
}