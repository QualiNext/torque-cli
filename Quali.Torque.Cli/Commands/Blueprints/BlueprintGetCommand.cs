using Quali.Torque.Cli.Models.Settings.Blueprints;

namespace Quali.Torque.Cli.Commands.Blueprints;

public class BlueprintGetCommand : TorqueMemberScopedCommand<BlueprintGetCommandSettings>
{
    public BlueprintGetCommand(IClientManager clientManager, IConsoleManager consoleManager) : base(clientManager,
        consoleManager)
    {
    }

    protected override async Task RunTorqueCommandAsync(BlueprintGetCommandSettings settings)
    {
        var blueprintDetails =
            await Client.CatalogGETAsync(User.Space, settings.BlueprintName, User.RepositoryName);

        if (settings.Detail)
        {
            ConsoleManager.DumpJson(blueprintDetails);
        }
        else
        {
            ConsoleManager.WriteBlueprintDetails(blueprintDetails);
        }
    }
}