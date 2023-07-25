using System.Net;
using Quali.Torque.Cli.Models.Settings.Blueprints;
using Torque.Cli.Api;

namespace Quali.Torque.Cli.Commands.Blueprints;

public class BlueprintGetCommand : TorqueMemberScopedCommand<BlueprintGetCommandSettings>
{
    public BlueprintGetCommand(IClientManager clientManager, IConsoleManager consoleManager) : base(clientManager,
        consoleManager)
    {
    }

    protected override async Task RunTorqueCommandAsync(BlueprintGetCommandSettings settings)
    {
        try
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
        catch (ApiException<ErrorResponse> ex)
        {
            if (ex.StatusCode == (int) HttpStatusCode.NotFound)
            {
                ConsoleManager.WriteInfo($"Blueprint {settings.BlueprintName} not found in {User.RepositoryName}");
            }
            else
            {
                ConsoleManager.WriteException(ex);   
            }
        }
    }
}