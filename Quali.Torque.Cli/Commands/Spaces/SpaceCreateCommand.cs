using Quali.Torque.Cli.Models.Settings.Base;
using Quali.Torque.Cli.Models.Settings.Spaces;
using Spectre.Console.Cli;
using Torque.Cli.Api;

namespace Quali.Torque.Cli.Commands.Spaces;

public class SpaceCreateCommand: TorqueAdminScopedCommand<SpaceCreateCommandSettings>
{
    public SpaceCreateCommand(IClientManager clientManager, IConsoleManager consoleManager) : base(clientManager, consoleManager)
    { 
    }

    protected override async Task RunTorqueCommandAsync(SpaceCreateCommandSettings settings)
    {
        await Client.SpacesPOST2Async(new CreateSpaceRequest {Name = settings.SpaceName});
        ConsoleManager.WriteSuccessMessage($"Space '{settings.SpaceName}' has been created.");
    }
}