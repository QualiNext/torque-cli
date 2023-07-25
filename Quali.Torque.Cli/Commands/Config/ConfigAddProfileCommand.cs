using Quali.Torque.Cli.Models;
using Quali.Torque.Cli.Models.Settings.Base;
using Quali.Torque.Cli.Utils;
using Spectre.Console.Cli;

namespace Quali.Torque.Cli.Commands.Config;

public class ConfigAddProfileCommand: ConfigBaseCommand<UserContextSettings>
{
    public ConfigAddProfileCommand(IConsoleManager consoleManager, IUserProfilesManager profilesManager) : base(consoleManager, profilesManager)
    {
    }

    public override int Execute(CommandContext context, UserContextSettings settings)
    {
        var newProfile = new UserProfile();
        try
        {
            // read profile name
            var profileName = settings.Profile 
                              ?? ConsoleManager.ReadUserInput<string>("Profile Name [default]: ", true);
            profileName = string.IsNullOrEmpty(profileName) ? "default" : profileName;
            
            newProfile.Name = profileName;

            try
            {
                var exisitingProfile = ProfilesManager.ReadUserProfile(profileName);
                if (exisitingProfile != null)
                {
                    ConsoleManager.WriteError($"Unable to add profile {profileName}. Profile with the same name already exists.");
                    return 1; 
                }
                
            }
            catch (Exception ex)
            {
                ConsoleManager.WriteException(ex, "Unable to read configuration file");
                return 1; 

            }
            
            // read token
            
            if (!string.IsNullOrEmpty(settings.Token))
            {
                newProfile.Token = settings.Token;
            }
            else
            {
                var isTokenNeeded = string.IsNullOrEmpty(newProfile.Token);
                var msg = isTokenNeeded ? "Torque Token: " : $"Torque Token [{newProfile.Token.MaskToken()}]: ";
                var token = ConsoleManager.ReadUserInput<string>(msg, !isTokenNeeded, true);
                newProfile.Token = string.IsNullOrEmpty(token) ? newProfile.Token : token;
            }
            
            // read space
            if (!string.IsNullOrEmpty(settings.Space))
            {
                newProfile.Space = settings.Space;
            }
            else
            {
                var isSpaceNeeded = string.IsNullOrEmpty(newProfile.Space);
                var msg = isSpaceNeeded ? "Torque Space: " : $"Torque Space [{newProfile.Space}]: ";
                var space = ConsoleManager.ReadUserInput<string>(msg, !isSpaceNeeded);
                newProfile.Space = string.IsNullOrEmpty(space) ? newProfile.Space : space;
            }
            
            // read repo
            if (!string.IsNullOrEmpty(settings.RepositoryName))
            {
                newProfile.RepositoryName = settings.RepositoryName;
            }
            else
            {
                var isRepoNeeded = string.IsNullOrEmpty(newProfile.RepositoryName);
                var msg = isRepoNeeded
                    ? "Torque Blueprint Repository: "
                    : $"Torque Blueprints Repository [{newProfile.RepositoryName}]: ";
                var repo  = ConsoleManager.ReadUserInput<string>(msg, true); // repo might be always optional for now
                newProfile.RepositoryName = string.IsNullOrEmpty(repo) ? newProfile.RepositoryName: repo;
            }
            
            ProfilesManager.WriteUserProfile(newProfile);

            ConsoleManager.WriteInfo("Profile added"); 
            
            return 0;
        }
        catch (Exception ex)
        {
            ConsoleManager.WriteException(ex);
            return 1;
        }
    }
}