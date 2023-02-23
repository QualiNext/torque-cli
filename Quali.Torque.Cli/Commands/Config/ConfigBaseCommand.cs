using Spectre.Console.Cli;

namespace Quali.Torque.Cli.Commands.Config;

public abstract class ConfigBaseCommand<T> : Command<T> where T : CommandSettings
{
    protected readonly IConsoleManager ConsoleManager;
    protected readonly IUserProfilesManager ProfilesManager;

    protected ConfigBaseCommand( IConsoleManager consoleManager, IUserProfilesManager profilesManager)
    {
        ConsoleManager = consoleManager;
        ProfilesManager = profilesManager;
    }
}