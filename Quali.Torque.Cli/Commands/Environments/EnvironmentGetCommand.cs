using Quali.Torque.Cli.Models.Settings.Environments;

namespace Quali.Torque.Cli.Commands.Environments;

public class EnvironmentGetCommand : TorqueMemberScopedCommand<EnvironmentGetUserContextSettings>
{
    public EnvironmentGetCommand(IClientManager clientManager, IConsoleManager consoleManager) : base(clientManager,
        consoleManager) { }

    protected override async Task RunTorqueCommandAsync(EnvironmentGetUserContextSettings settings)
    {
        var envDetails = await Client.EnvironmentsGETAsync(User.Space, settings.EnvironmentId);

        if (settings.Detail)
        {
            ConsoleManager.DumpJson(envDetails);
        }
        else
        {
            ConsoleManager.WriteEnvironmentDetails(envDetails.Details);
        }
    }
}