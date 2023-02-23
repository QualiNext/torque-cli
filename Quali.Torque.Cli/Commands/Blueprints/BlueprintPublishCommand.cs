using Quali.Torque.Cli.Models.Settings.Blueprints;
using Torque.Cli.Api;

namespace Quali.Torque.Cli.Commands.Blueprints;

public class BlueprintPublishCommand : TorqueMemberScopedCommand<BlueprintGetCommandSettings>
{
    public BlueprintPublishCommand(IClientManager clientManager, IConsoleManager consoleManager) : base(clientManager,
        consoleManager)
    {
    }

    protected override async Task RunTorqueCommandAsync(BlueprintGetCommandSettings settings)
    {
        var request = new AddCatalogRequest
        {
            Blueprint_name = settings.BlueprintName,
            Repository_name = User.RepositoryName
        };

        await Client.CatalogPOSTAsync(User.Space, request);
        ConsoleManager.WriteSuccessMessage($"Blueprint '{settings.BlueprintName}' has been added to the catalog");
    }
}