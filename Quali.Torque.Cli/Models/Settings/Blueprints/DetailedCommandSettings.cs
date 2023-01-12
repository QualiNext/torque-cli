using System.ComponentModel;
using Quali.Torque.Cli.Models.Settings.Base;
using Spectre.Console.Cli;

namespace Quali.Torque.Cli.Models.Settings.Blueprints;

internal class DetailedCommandSettings: BaseSettings
{
    [CommandOption("--detail")]
    [Description("Show detailed output in a json format")]
    public bool Detail { get; set; }
}