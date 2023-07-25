using Quali.Torque.Cli.Models;
using Quali.Torque.Cli.Models.Settings.Base;
using Spectre.Console.Cli;

namespace Quali.Torque.Cli.Commands.Config;

public class ConfigUpdateProfileCommand : ConfigBaseCommand<UserContextSettings>
{
    public ConfigUpdateProfileCommand(IConsoleManager consoleManager, IUserProfilesManager profilesManager) : base(
        consoleManager, profilesManager)
    {
    }

    public override int Execute(CommandContext context, UserContextSettings settings)
    {
        var profileName = settings.Profile
                          ?? ConsoleManager.ReadUserInput<string>("Profile Name: ", true);

        UserProfile existingProfile = null;

        try
        {
            existingProfile = ProfilesManager.ReadUserProfile(profileName);
            if (existingProfile == null)
            {
                ConsoleManager.WriteError($"Profile ({profileName}) not found. ");
                return 1;
            }
        }
        catch (Exception ex)
        {
            ConsoleManager.WriteException(ex, "Unable to read configuration file");
            return 1;
        }
        
        if (!string.IsNullOrEmpty(settings.Token))
        {
            existingProfile.Token = settings.Token;
        }
        
        if (!string.IsNullOrEmpty(settings.Space))
        {
            existingProfile.Space = settings.Space;
        }
        
        if (!string.IsNullOrEmpty(settings.RepositoryName))
        {
            existingProfile.RepositoryName = settings.RepositoryName;
        }

        ProfilesManager.WriteUserProfile(existingProfile);

        ConsoleManager.WriteInfo("Profile updated");
        
        return 0;
    }
}