using Quali.Torque.Cli.Models.Settings.Blueprints;
using Spectre.Console.Cli;

namespace Quali.Torque.Cli.Commands.Blueprints;

internal class BlueprintGetCommand : AsyncCommand<BlueprintGetCommandSettings>
{
    private readonly IClientManager _clientManager;
    private readonly IConsoleWriter _consoleWriter;

    public BlueprintGetCommand(IClientManager clientManager,
        IConsoleWriter consoleWriter)
    {
        _clientManager = clientManager;
        _consoleWriter = consoleWriter;
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
                _consoleWriter.DumpJson(blueprintDetails);
            }
            else
            {
                _consoleWriter.WriteBlueprintDetails(blueprintDetails);
            }
                
            return 0;
        }
        catch (Exception ex)
        {
            _consoleWriter.WriteError(ex);
            return 1;
        }
    }
}