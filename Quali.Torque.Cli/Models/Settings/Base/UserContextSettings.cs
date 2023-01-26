using System.ComponentModel;
using Spectre.Console.Cli;

namespace Quali.Torque.Cli.Models.Settings.Base;

public class UserContextSettings : BaseSettings
{
    [CommandOption("-s|--space")]
    [Description("The space name")]
    public string Space { get; set; }

    [CommandOption("-p|--profile")]
    [Description("The user profile name")]
    public string Profile { get; set; }
    
    [CommandOption("-r|--repo")]
    [Description("The repository name to use with command")]
    public string RepositoryName { get; set; }
}