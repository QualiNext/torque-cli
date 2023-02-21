using Quali.Torque.Cli.Models.Settings.Base;
namespace Quali.Torque.Cli.Commands.Spaces;

public class SpaceListCommand: TorqueAdminScopedCommand<DetailedBaseSettings>
{
    public SpaceListCommand(IClientManager clientManager, IConsoleManager consoleManager) : base(clientManager, consoleManager)
    {
    }

    protected override async Task RunTorqueCommandAsync(DetailedBaseSettings settings)
    {
        var spacesList = await Client.SpacesAllAsync();

        if (spacesList.Count > 0)
        {
            if (settings.Detail)
            {
                ConsoleManager.DumpJson(spacesList);
            }
            else
            {
                ConsoleManager.WriteSpaceList(spacesList);
            }
        }
        else
        {
            ConsoleManager.WriteEmptyList("No spaces found");
        }
    }
}