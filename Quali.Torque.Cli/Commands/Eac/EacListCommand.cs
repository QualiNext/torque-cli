using Quali.Torque.Cli.Models.Settings.Base;

namespace Quali.Torque.Cli.Commands.Blueprints;

public class EacListCommand : TorqueMemberScopedCommand<DetailedUserContextSettings>
{
    public EacListCommand(IClientManager clientManager,
        IConsoleManager consoleManager) : base(clientManager, consoleManager)
    {
    }

    protected override async Task RunTorqueCommandAsync(DetailedUserContextSettings settings)
    {
        var eacList = await Client.EacAsync(User.Space);

        if (eacList.Count > 0)
        {
            if (settings.Detail)
            {
                ConsoleManager.DumpJson(eacList);
            }
            else
            {
                ConsoleManager.WriteEacList(eacList);
            }
        }
        else
        {
            ConsoleManager.WriteEmptyList("No eac found");
        }
    }
}