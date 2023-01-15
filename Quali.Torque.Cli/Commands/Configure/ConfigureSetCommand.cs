using Quali.Torque.Cli.Models;
using Quali.Torque.Cli.Models.Settings.Base;
using Quali.Torque.Cli.Utils;
using Spectre.Console.Cli;

namespace Quali.Torque.Cli.Commands.Configure;

internal class ConfigureSetCommand: Command<BaseSettings>
{
    private readonly IConsoleManager _consoleManager;
    private readonly IUserProfilesManager _profilesManager;
    
    public ConfigureSetCommand( IConsoleManager consoleManager, IUserProfilesManager profilesManager)
    {
        _consoleManager = consoleManager;
        _profilesManager = profilesManager;
    }

    public override int Execute(CommandContext context, BaseSettings settings)
    {
        var newProfile = new UserProfile();
        try
        {
            // read profile name
            var profileName = settings.Profile 
                              ?? _consoleManager.ReadUserInput<string>("Profile Name [default]: ", true);
            profileName = string.IsNullOrEmpty(profileName) ? "default" : profileName;
            newProfile.Name = profileName;

            var currentProfile = _profilesManager.ReadUserProfile(profileName);
            
            // read token
            if (!string.IsNullOrEmpty(settings.Token))
            {
                newProfile.Token = settings.Token;
            }
            else
            {
                var isTokenNeeded = string.IsNullOrEmpty(currentProfile.Token);
                var msg = isTokenNeeded ? "Torque Token: " : $"Torque Token [{currentProfile.Token.MaskToken()}]: ";
                var token = _consoleManager.ReadUserInput<string>(msg, !isTokenNeeded, true);
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
                var space = _consoleManager.ReadUserInput<string>(msg, !isSpaceNeeded);
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
                var repo  = _consoleManager.ReadUserInput<string>(msg, !isRepoNeeded); 
                newProfile.RepositoryName = string.IsNullOrEmpty(repo) ? currentProfile.RepositoryName: repo;
            }
            
            _profilesManager.WriteUserProfile(newProfile);
            
            return 0;
        }
        catch (Exception ex)
        {
            _consoleManager.WriteError(ex);
            return 1;
        }
    }
}