using System.ComponentModel;
using Quali.Torque.Cli.Models.Settings.Base;
using Spectre.Console.Cli;

namespace Quali.Torque.Cli.Models.Settings.Blueprints;

public class BlueprintGetCommandSettings: DetailedUserContextSettings
{
    [CommandArgument(0, "<BLUEPRINT-NAME>")]
    [Description("The blueprint name to show")]
    public string BlueprintName { get; set; }
}