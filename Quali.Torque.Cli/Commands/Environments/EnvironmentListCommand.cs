using Quali.Torque.Cli.Models.Settings.Environments;
using Spectre.Console.Cli;

namespace Quali.Torque.Cli.Commands.Environments;

internal class EnvironmentListCommand: AsyncCommand<EnvironmentListCommandSettings>
{
    private readonly IClientManager _clientManager;
    private readonly IConsoleManager _consoleManager;

    public EnvironmentListCommand(IClientManager clientManager, IConsoleManager consoleManager)
    {
        _clientManager = clientManager;
        _consoleManager = consoleManager;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, EnvironmentListCommandSettings settings)
    {
        try
        {
            var user = _clientManager.FetchUserProfile(settings);
            var torqueClient = _clientManager.GetClient(user);

            var envList =
                await torqueClient.EnvironmentsAllAsync(user.Space, null, null, null, !settings.ShowEnded,
                    settings.Count);

            if (envList.Count > 0)
            {
                if (settings.Detail)
                {
                    _consoleManager.DumpJson(envList);
                }
                else
                {
                    _consoleManager.WriteEnvironmentList(envList);
                }
            }
            else
            {
                _consoleManager.WriteEmptyList("No environments found");
            }

            return 0;
        }
        catch (Exception e)
        {
            _consoleManager.WriteError(e);
            return 1;
        }
    }
}