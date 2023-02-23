using Quali.Torque.Cli.Models.Settings.Environments;

namespace Quali.Torque.Cli.Commands.Environments;

public class EnvironmentListCommand: TorqueMemberScopedCommand<EnvironmentListUserContextSettings>
{
    public EnvironmentListCommand(IClientManager clientManager, IConsoleManager consoleManager) : base(clientManager,
        consoleManager) { }

    protected override async Task RunTorqueCommandAsync(EnvironmentListUserContextSettings settings)
    {
        var filter = settings.Filter == "auto" ? "automation" : settings.Filter;

        var envList =
            await Client.EnvironmentsAllAsync(User.Space, filter, null, null, !settings.ShowEnded, settings.Count);

        if (envList.Count > 0)
        {
            if (settings.Detail)
            {
                ConsoleManager.DumpJson(envList);
            }
            else
            {
                ConsoleManager.WriteEnvironmentList(envList);
            }
        }
        else
        {
            ConsoleManager.WriteEmptyList("No environments found");
        }
    }
}