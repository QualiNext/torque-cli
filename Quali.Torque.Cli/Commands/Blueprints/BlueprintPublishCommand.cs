using Quali.Torque.Cli.Models.Settings.Blueprints;
using Spectre.Console.Cli;
using Torque.Cli.Api;

namespace Quali.Torque.Cli.Commands.Blueprints;

public class BlueprintPublishCommand : TorqueBaseCommand<BlueprintGetUserContextSettings>
{
    public BlueprintPublishCommand(IClientManager clientManager, IConsoleManager consoleManager) : base(clientManager,
        consoleManager)
    {
    }

    public override async Task<int> ExecuteAsync(CommandContext context, BlueprintGetUserContextSettings settings)
    {
        try
        {
            var user = _clientManager.FetchUserProfile(settings);
            var torqueClient = _clientManager.GetClient(user);

            var request = new AddCatalogRequest
            {
                Blueprint_name = settings.BlueprintName,
                Repository_name = user.RepositoryName
            };

            await torqueClient.CatalogPOSTAsync(user.Space, request);
            _consoleManager.WriteSuccessMessage($"Blueprint '{settings.BlueprintName}' has been added to the catalog");

        }
        catch (Exception ex)
        {
            _consoleManager.WriteError(ex);
            return 1;
        }

        return 0;
    }
}