using Quali.Torque.Cli.Models.Settings.Spaces;

namespace Quali.Torque.Cli.Commands.Spaces;

public class SpaceDeleteCommand: TorqueAdminScopedCommand<SpaceDeleteCommandSettings>
{
    public SpaceDeleteCommand(IClientManager clientManager, IConsoleManager consoleManager) : base(clientManager, consoleManager)
    {
    }

    protected override async Task RunTorqueCommandAsync(SpaceDeleteCommandSettings settings)
    {
        await Client.SpacesDELETEAsync(settings.SpaceName);
        ConsoleManager.WriteSuccessMessage($"Space '{settings.SpaceName}' has been deleted.");
    }
}