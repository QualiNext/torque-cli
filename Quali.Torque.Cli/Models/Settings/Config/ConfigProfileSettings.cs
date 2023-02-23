using System.ComponentModel;
using Spectre.Console.Cli;

namespace Quali.Torque.Cli.Models.Settings.Config;

public class ConfigProfileSettings: CommandSettings
{
    [CommandArgument(0, "<PROFILE-NAME>")]
    [Description("Set the profile name to perform action with")]
    public string ProfileName { get; set; }
}