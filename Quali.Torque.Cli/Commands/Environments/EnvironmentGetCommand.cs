using System.Runtime.InteropServices;
using System.Text.Json;
using Quali.Torque.Cli.Models.Settings.Environments;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Quali.Torque.Cli.Commands.Environments;

internal class EnvironmentGetCommand : AsyncCommand<EnvironmentGetCommandSettings>
{
    private readonly IClientManager _clientManager;
    private readonly IConsoleManager _consoleManager;

    public EnvironmentGetCommand(IClientManager clientManager, IConsoleManager consoleManager)
    {
        _clientManager = clientManager;
        _consoleManager = consoleManager;
    }

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