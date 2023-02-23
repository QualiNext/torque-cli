using Quali.Torque.Cli.Models.Settings.Config;
using Spectre.Console.Cli;

namespace Quali.Torque.Cli.Commands.Config;

public class ConfigSetActiveCommand: ConfigBaseCommand<ConfigProfileSettings>
{
    public ConfigSetActiveCommand( IConsoleManager consoleManager, IUserProfilesManager profilesManager) : base(consoleManager, profilesManager)
    { }
    
    public override int Execute(CommandContext context, ConfigProfileSettings settings)
    {
        var profileName = settings.ProfileName;

        try
        {
            ProfilesManager.SetActiveUserProfile(profileName);
            return 0;
        }
        catch (Exception e)
        {
            ConsoleManager.WriteException(e);
            return 1;
        }
    }
}