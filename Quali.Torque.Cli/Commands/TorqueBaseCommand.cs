using Spectre.Console.Cli;

namespace Quali.Torque.Cli.Commands;

public abstract class TorqueBaseCommand<T> : AsyncCommand<T> where T : CommandSettings
{
    protected readonly IClientManager _clientManager;
    protected readonly IConsoleManager _consoleManager;

    protected TorqueBaseCommand(IClientManager clientManager, IConsoleManager consoleManager)
    {
        _clientManager = clientManager;
        _consoleManager = consoleManager;
    }
}