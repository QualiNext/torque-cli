using Quali.Torque.Cli.Models.Settings.Spaces;
using Spectre.Console.Cli;

namespace Quali.Torque.Cli.Commands.Spaces;

public class SpaceAddRepoCommand: TorqueBaseCommand<SpaceAddRepoCommandSettings>
{
    public SpaceAddRepoCommand(IClientManager clientManager, IConsoleManager consoleManager) : base(clientManager, consoleManager)
    {
    }

    public override async Task<int> ExecuteAsync(CommandContext context, SpaceAddRepoCommandSettings settings)
    {
        throw new NotImplementedException();
    }
}