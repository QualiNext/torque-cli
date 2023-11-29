using System.ComponentModel;
using Quali.Torque.Cli.Models.Settings.Base;
using Spectre.Console.Cli;

namespace Quali.Torque.Cli.Models.Settings.Eac;

public class ExportEacCommandSettings : UserContextSettings
{
    [CommandArgument(0, "<ENVIRONMENT-ID>")]
    [Description("The environment id for plan")]
    public string EnvironmentId { get; set; }
    
    [CommandOption("-o|--output")]
    [Description("The file name to generate, defaults to the environment's name")]
    public string FileName { get; set; }
}