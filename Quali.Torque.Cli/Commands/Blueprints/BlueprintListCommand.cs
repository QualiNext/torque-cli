using Quali.Torque.Cli.Models.Settings.Base;

namespace Quali.Torque.Cli.Commands.Blueprints;

public class BlueprintListCommand : TorqueMemberScopedCommand<DetailedUserContextSettings>
{
    public BlueprintListCommand(IClientManager clientManager,
        IConsoleManager consoleManager) : base(clientManager, consoleManager)
    {
    }

    protected override async Task RunTorqueCommandAsync(DetailedUserContextSettings settings)
    {
        var blueprintList = await Client.BlueprintsAllAsync(User.Space);

        if (blueprintList.Count > 0)
        {
            if (settings.Detail)
            {
                ConsoleManager.DumpJson(blueprintList);
            }
            else
            {
                ConsoleManager.WriteBlueprintList(blueprintList);
            }
        }
        else
        {
            ConsoleManager.WriteEmptyList("No blueprints found");
        }
    }
}