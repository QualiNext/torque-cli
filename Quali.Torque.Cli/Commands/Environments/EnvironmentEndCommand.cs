using Quali.Torque.Cli.Models.Settings.Environments;
using Spectre.Console.Cli;

namespace Quali.Torque.Cli.Commands.Environments;

public class EnvironmentEndCommand: TorqueBaseCommand<EnvironmentEndCommandSettings>
{
    public EnvironmentEndCommand(IClientManager clientManager, IConsoleManager consoleManager) : base(clientManager,
        consoleManager) { }

    public override async Task<int> ExecuteAsync(CommandContext context, EnvironmentEndCommandSettings settings)
    {
        var user = _clientManager.FetchUserProfile(settings);
        try
        {
            await _clientManager.GetClient(user).EnvironmentsDELETEAsync(user.Space, settings.EnvironmentId);
            _consoleManager.WriteEnvironmentEnded(settings.EnvironmentId);
            return 0;
        }
        catch (Exception e)
        {
            _consoleManager.WriteError(e);
            return 1;
        }
    }
}