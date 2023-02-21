using Quali.Torque.Cli.Models.Settings.Blueprints;
using Spectre.Console.Cli;

namespace Quali.Torque.Cli.Commands.Blueprints;

public class  BlueprintUnpublishCommand : TorqueMemberScopedCommand<BlueprintGetCommandSettings>
{
    public BlueprintUnpublishCommand(IClientManager clientManager, IConsoleManager consoleManager) : base(clientManager, consoleManager)
    {
    }

    protected override async Task RunTorqueCommandAsync(BlueprintGetCommandSettings settings)
    {
        await Client.CatalogDELETEAsync(User.Space, User.RepositoryName, settings.BlueprintName);
        ConsoleManager.WriteSuccessMessage($"Blueprint '{settings.BlueprintName}' has been removed from the catalog");
    }
}