using Quali.Torque.Cli.Models.Settings.Spaces;
using Spectre.Console.Cli;

namespace Quali.Torque.Cli.Commands.Spaces;

public class SpaceAddRepoCommand: TorqueAdminScopedCommand<SpaceAddRepoCommandSettings>
{
    public SpaceAddRepoCommand(IClientManager clientManager, IConsoleManager consoleManager) : base(clientManager, consoleManager)
    {
    }

    protected override Task RunTorqueCommandAsync(SpaceAddRepoCommandSettings settings)
    {
        throw new NotImplementedException();
    }
}