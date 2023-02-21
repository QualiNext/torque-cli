using Quali.Torque.Cli.Models.Settings.Agents;
using Quali.Torque.Cli.Models.Settings.Base;
using Spectre.Console.Cli;
using Torque.Cli.Api;

namespace Quali.Torque.Cli.Commands.Agents;

public class AgentAssociateWithSpaceCommand: TorqueAdminScopedCommand<AgentAssociateWithSpaceCommandSettings>
{
    public AgentAssociateWithSpaceCommand(IClientManager clientManager, IConsoleManager consoleManager) : base(clientManager, consoleManager)
    {
    }

    protected override async Task RunTorqueCommandAsync(AgentAssociateWithSpaceCommandSettings settings)
    {
        var body = new K8sSpaceAssociationSpec
        {
            Default_namespace = settings.DefaultNamespace,
            Default_service_account = settings.DefaultServiceAccount,
            Type = settings.Type
        };
            
        await Client.SpacesPOSTAsync(settings.AgentName, settings.SpaceName, body);
        ConsoleManager.WriteSuccessMessage("Association has been created");
    }
}