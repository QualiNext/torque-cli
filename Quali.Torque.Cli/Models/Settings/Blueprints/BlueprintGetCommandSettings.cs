using System.ComponentModel;
using Spectre.Console.Cli;

namespace Quali.Torque.Cli.Models.Settings.Blueprints;

internal class BlueprintGetCommandSettings: DetailedCommandSettings
{
    [CommandArgument(0, "<BLUEPRINT-NAME>")]
    [Description("The blueprint name to show")]
    public string BlueprintName { get; set; }
}