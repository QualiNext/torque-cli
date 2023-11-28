using Quali.Torque.Cli.Models.Settings.Environments;
using Quali.Torque.Cli.Utils;
using Torque.Cli.Api;

namespace Quali.Torque.Cli.Commands.Environments;

public class EnvironmentGetCommand : TorqueMemberScopedCommand<EnvironmentGetUserContextSettings>
{
    public EnvironmentGetCommand(IClientManager clientManager, IConsoleManager consoleManager) : base(clientManager,
        consoleManager) { }

    protected override async Task RunTorqueCommandAsync(EnvironmentGetUserContextSettings settings)
    {
        IEnumerable<EacResponse> eacs = await Client.EacAsync(User.Space);
        if (EnvironmentHelper.TryGetEnvIdFromUrl(settings.EnvironmentId, eacs, out string environmentId))
            settings.EnvironmentId = environmentId;

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