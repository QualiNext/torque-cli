using System.ComponentModel;
using Spectre.Console.Cli;

namespace Quali.Torque.Cli.Models.Settings.Base;

public class UserContextSettings : BaseSettings
{
    [CommandOption("-s|--space")]
    [Description("The space name")]
    public string Space { get; set; }

    [CommandOption("-r|--repo")]
    [Description("The repository name to use with command")]
    public string RepositoryName { get; set; }
    
    public static UserContextSettings ConvertToUserContextSettings(BaseSettings baseSettings)
    {
        return new UserContextSettings 
        {
            Token = baseSettings.Token,
            BaseUrl = baseSettings.BaseUrl,
            Profile = baseSettings.Profile
        };
    }
}

public class DetailedUserContextSettings: UserContextSettings
{
    [CommandOption("--detail")]
    [Description("Show detailed output in a json format")]
    public bool Detail { get; set; }
}