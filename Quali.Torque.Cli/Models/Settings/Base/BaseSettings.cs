using System.ComponentModel;
using Spectre.Console.Cli;

namespace Quali.Torque.Cli.Models.Settings.Base;

public class BaseSettings: CommandSettings
{
    [CommandOption("-t|--token")]
    [Description("The user long token")]
    public string Token { get; set; }
    
    [CommandOption("--base-url <URL>", IsHidden = true)]
    public string BaseUrl { get; set; }
    
    [CommandOption("-p|--profile")]
    [Description("The user profile name")]
    public string Profile { get; set; }
}

public class DetailedBaseSettings: BaseSettings
{
    [CommandOption("--detail")]
    [Description("Show detailed output in a json format")]
    public bool Detail { get; set; }
}