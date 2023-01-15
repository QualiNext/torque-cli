using Spectre.Console.Cli;

namespace Quali.Torque.Cli.Commands.Configure;

internal class ConfigureListCommand: Command
{
    private readonly IConsoleManager _consoleManager;
    private readonly IUserProfilesManager _profilesManager;

    public ConfigureListCommand( IConsoleManager consoleManager, IUserProfilesManager profilesManager)
    {
        _consoleManager = consoleManager;
        _profilesManager = profilesManager;
    }

    public override int Execute(CommandContext context)
    {
        var profiles = _profilesManager.ListUserProfiles();
        _consoleManager.WriteProfilesList(profiles);
        return 0;
    }
}