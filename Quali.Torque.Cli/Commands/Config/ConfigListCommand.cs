using Spectre.Console.Cli;

namespace Quali.Torque.Cli.Commands.Config;

public class ConfigListCommand: Command
{
    private readonly IConsoleManager _consoleManager;
    private readonly IUserProfilesManager _profilesManager;

    public ConfigListCommand( IConsoleManager consoleManager, IUserProfilesManager profilesManager)
    {
        _consoleManager = consoleManager;
        _profilesManager = profilesManager;
    }

    public override int Execute(CommandContext context)
    {
        var profiles = _profilesManager.ListUserProfiles();
        var activeProfileName = _profilesManager.ReadActiveUserProfile().Name;
        _consoleManager.WriteProfilesList(profiles, activeProfileName);
        return 0;
    }
}