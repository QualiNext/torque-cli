using Quali.Torque.Cli.Models.Settings.Environments;
using Spectre.Console.Cli;

namespace Quali.Torque.Cli.Commands.Environments;

public class EnvironmentGetCommand : TorqueBaseCommand<EnvironmentGetCommandSettings>
{
    public EnvironmentGetCommand(IClientManager clientManager, IConsoleManager consoleManager) : base(clientManager,
        consoleManager) { }

    public override async Task<int> ExecuteAsync(CommandContext context, EnvironmentGetCommandSettings settings)
    {
        try
        {
            var user = _clientManager.FetchUserProfile(settings);
            var torqueClient = _clientManager.GetClient(user);

            var envDetails =
                await torqueClient.EnvironmentsGETAsync(user.Space, settings.EnvironmentId);

            if (settings.Detail)
            {
                _consoleManager.DumpJson(envDetails);
            }
            else
            {
                _consoleManager.WriteEnvironmentDetails(envDetails.Details);
            }
                
            return 0;
        }
        catch (Exception ex)
        {
            _consoleManager.WriteError(ex);
            return 1;
        }
    }
}