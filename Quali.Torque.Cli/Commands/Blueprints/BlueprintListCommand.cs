using Quali.Torque.Cli.Models.Settings.Blueprints;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Quali.Torque.Cli.Commands.Blueprints;

internal class BlueprintListCommand : AsyncCommand<DetailedCommandSettings>
{
    private readonly IClientManager _clientManager;
    private readonly IConsoleWriter _consoleWriter;

    public BlueprintListCommand(IClientManager clientManager,
        IConsoleWriter consoleWriter)
    {
        _clientManager = clientManager;
        _consoleWriter = consoleWriter;
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
                    _consoleWriter.DumpJson(blueprintList);
                }
                else
                {
                    _consoleWriter.WriteBlueprintList(blueprintList);
                }
            }
            else
            {
                _consoleWriter.WriteEmptyBlueprintList();
            }
        }
        catch (Exception ex)
        {
            _consoleWriter.WriteError(ex);
            return 1;
        }
            

        return 0;
    }
}