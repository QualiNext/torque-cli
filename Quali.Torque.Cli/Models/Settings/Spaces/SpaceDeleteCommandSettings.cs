using System.ComponentModel;
using Quali.Torque.Cli.Models.Settings.Base;
using Spectre.Console.Cli;

namespace Quali.Torque.Cli.Models.Settings.Spaces;

public class SpaceDeleteCommandSettings: BaseSettings
{
    [CommandArgument(0, "<SPACE-NAME>")]
    [Description("The name of space to delete")]
    public string SpaceName { get; set; }
}