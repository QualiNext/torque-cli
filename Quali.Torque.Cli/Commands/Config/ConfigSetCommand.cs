using Quali.Torque.Cli.Models;
using Quali.Torque.Cli.Models.Settings.Base;
using Quali.Torque.Cli.Utils;
using Spectre.Console.Cli;

namespace Quali.Torque.Cli.Commands.Config;

public class ConfigSetCommand: ConfigBaseCommand<UserContextSettings>
{
    public ConfigSetCommand(IConsoleManager consoleManager, IUserProfilesManager profilesManager) : base(consoleManager, profilesManager)
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

            UserProfile currentProfile;

            try
            {
                currentProfile = ProfilesManager.ReadUserProfile(profileName);
            }
            catch (ProfileNotFoundException)
            {
                currentProfile = new UserProfile();
            }
            catch (DuplicatedProfilesFoundException e)
            {
                throw new Exception("Profiles configuration might be broken. Details: " + e.Message);
            }
            
            // read token
            if (!string.IsNullOrEmpty(settings.Token))
            {
                newProfile.Token = settings.Token;
            }
            else
            {
                var isTokenNeeded = string.IsNullOrEmpty(currentProfile.Token);
                var msg = isTokenNeeded ? "Torque Token: " : $"Torque Token [{currentProfile.Token.MaskToken()}]: ";
                var token = ConsoleManager.ReadUserInput<string>(msg, !isTokenNeeded, true);
                newProfile.Token = string.IsNullOrEmpty(token) ? currentProfile.Token : token;
            }
            
            // read space
            if (!string.IsNullOrEmpty(settings.Space))
            {
                newProfile.Space = settings.Space;
            }
            else
            {
                var isSpaceNeeded = string.IsNullOrEmpty(currentProfile.Space);
                var msg = isSpaceNeeded ? "Torque Space: " : $"Torque Space [{currentProfile.Space}]: ";
                var space = ConsoleManager.ReadUserInput<string>(msg, !isSpaceNeeded);
                newProfile.Space = string.IsNullOrEmpty(space) ? currentProfile.Space : space;
            }
            
            // read repo
            if (!string.IsNullOrEmpty(settings.RepositoryName))
            {
                newProfile.RepositoryName = settings.RepositoryName;
            }
            else
            {
                var isRepoNeeded = string.IsNullOrEmpty(currentProfile.RepositoryName);
                var msg = isRepoNeeded
                    ? "Torque Blueprint Repository: "
                    : $"Torque Blueprints Repository [{currentProfile.RepositoryName}]: ";
                var repo  = ConsoleManager.ReadUserInput<string>(msg, true); // repo might be always optional for now
                newProfile.RepositoryName = string.IsNullOrEmpty(repo) ? currentProfile.RepositoryName: repo;
            }
            
            ProfilesManager.WriteUserProfile(newProfile);
            
            return 0;
        }
        catch (Exception ex)
        {
            ConsoleManager.WriteException(ex);
            return 1;
        }
    }
}