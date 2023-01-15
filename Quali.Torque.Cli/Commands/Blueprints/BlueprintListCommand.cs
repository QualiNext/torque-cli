using Quali.Torque.Cli.Models.Settings.Blueprints;
using Spectre.Console.Cli;

namespace Quali.Torque.Cli.Commands.Blueprints;

internal class BlueprintListCommand : AsyncCommand<DetailedCommandSettings>
{
    private readonly IClientManager _clientManager;
    private readonly IConsoleManager _consoleManager;

    public BlueprintListCommand(IClientManager clientManager,
        IConsoleManager consoleManager)
    {
        _clientManager = clientManager;
        _consoleManager = consoleManager;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, DetailedCommandSettings settings)
    {
        try
        {
            var user = _clientManager.FetchUserProfile(settings);
            var torqueClient = _clientManager.GetClient(user);

            var blueprintList = await torqueClient.BlueprintsAllAsync(user.Space);

            if (blueprintList.Count > 0)
            {
                if (settings.Detail)
                {
                    _consoleManager.DumpJson(blueprintList);
                }
                else
                {
                    _consoleManager.WriteBlueprintList(blueprintList);
                }
            }
            else
            {
                _consoleManager.WriteEmptyBlueprintList();
            }
        }
        catch (Exception ex)
        {
            _consoleManager.WriteError(ex);
            return 1;
        }
            

        return 0;
    }
}