using System.ComponentModel;
using Quali.Torque.Cli.Models.Settings.Base;
using Spectre.Console.Cli;

namespace Quali.Torque.Cli.Models.Settings.Agents;

public class AgentsListCommandSettings: BaseSettings
{
    [CommandArgument(0, "<SPACE-NAME>")]
    [Description("The space name to list agents from")]
    public string SpaceName { get; set; }
}