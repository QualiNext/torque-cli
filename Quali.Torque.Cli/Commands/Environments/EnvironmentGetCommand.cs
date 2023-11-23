using Quali.Torque.Cli.Models.Settings.Environments;

namespace Quali.Torque.Cli.Commands.Environments;

public class EnvironmentGetCommand : TorqueMemberScopedCommand<EnvironmentGetUserContextSettings>
{
    public EnvironmentGetCommand(IClientManager clientManager, IConsoleManager consoleManager) : base(clientManager,
        consoleManager) { }

    protected override async Task RunTorqueCommandAsync(EnvironmentGetUserContextSettings settings)
    {
        if (await TryGetEnvIdFromUrl(settings)) return;

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

    private async Task<bool> TryGetEnvIdFromUrl(EnvironmentGetUserContextSettings settings)
    {
        if (Uri.IsWellFormedUriString(settings.EnvironmentId, UriKind.Absolute))
        {
            var eacs = await Client.EacAsync(User.Space);
            var eac = eacs.FirstOrDefault(e => string.Equals(e.Url, settings.EnvironmentId));
            if (eac == null)
            {
                ConsoleManager.WriteError("No environment matching the URL found.");
                return true;
            }

            if (string.IsNullOrEmpty(eac.Environment_id))
            {
                ConsoleManager.WriteError("The environment matching the URL is currently inactive.");
                return true;
            }

            settings.EnvironmentId = eac.Environment_id;
        }

        return false;
    }
}