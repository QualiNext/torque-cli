using Quali.Torque.Cli.Models.Settings.Blueprints;
using Spectre.Console.Cli;

namespace Quali.Torque.Cli.Commands.Blueprints;

internal class BlueprintGetCommand : AsyncCommand<BlueprintGetCommandSettings>
{
    private readonly IClientManager _clientManager;
    private readonly IConsoleManager _consoleManager;

    public BlueprintGetCommand(IClientManager clientManager, IConsoleManager consoleManager)
    {
        _clientManager = clientManager;
        _consoleManager = consoleManager;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, BlueprintGetCommandSettings settings)
    {
        try
        {
            var user = _clientManager.FetchUserProfile(settings);
            var torqueClient = _clientManager.GetClient(user);

            var blueprintDetails =
                await torqueClient.CatalogGETAsync(user.Space, settings.BlueprintName, user.RepositoryName);

            if (settings.Detail)
            {
                _consoleManager.DumpJson(blueprintDetails);
            }
            else
            {
                _consoleManager.WriteBlueprintDetails(blueprintDetails);
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