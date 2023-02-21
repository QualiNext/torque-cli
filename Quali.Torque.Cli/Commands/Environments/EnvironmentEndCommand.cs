using Quali.Torque.Cli.Models.Settings.Environments;

namespace Quali.Torque.Cli.Commands.Environments;

public class EnvironmentEndCommand: TorqueMemberScopedCommand<EnvironmentEndCommandSettings>
{
    public EnvironmentEndCommand(IClientManager clientManager, IConsoleManager consoleManager) : base(clientManager,
        consoleManager) { }

    protected override async Task RunTorqueCommandAsync(EnvironmentEndCommandSettings settings)
    {
        await Client.EnvironmentsDELETEAsync(User.Space, settings.EnvironmentId);
        ConsoleManager.WriteSuccessMessage($"Request to end environment {settings.EnvironmentId} has been sent");
    }
}