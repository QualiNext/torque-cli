using Quali.Torque.Cli.Models.Settings.Blueprints;
using Spectre.Console.Cli;

namespace Quali.Torque.Cli.Commands.Blueprints;

public class BlueprintUnpublishCommand : TorqueBaseCommand<BlueprintGetUserContextSettings>
{
    public BlueprintUnpublishCommand(IClientManager clientManager, IConsoleManager consoleManager) : base(clientManager, consoleManager)
    {
    }

    public override async Task<int> ExecuteAsync(CommandContext context, BlueprintGetUserContextSettings settings)
    {
        try
        {
            var user = _clientManager.FetchUserProfile(settings);
            var torqueClient = _clientManager.GetClient(user);

            await torqueClient.CatalogDELETEAsync(user.Space, user.RepositoryName, settings.BlueprintName);
            _consoleManager.WriteSuccessMessage($"Blueprint '{settings.BlueprintName}' has been removed from the catalog");
        }
        catch (Exception ex)
        {
            _consoleManager.WriteError(ex);
            return 1;
        }

        return 0;
    }
}