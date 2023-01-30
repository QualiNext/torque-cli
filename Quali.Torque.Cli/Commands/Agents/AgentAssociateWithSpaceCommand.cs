using Quali.Torque.Cli.Models.Settings.Agents;
using Quali.Torque.Cli.Models.Settings.Base;
using Spectre.Console.Cli;
using Torque.Cli.Api;

namespace Quali.Torque.Cli.Commands.Agents;

public class AgentAssociateWithSpaceCommand: TorqueBaseCommand<AgentAssociateWithSpaceCommandSettings>
{
    public AgentAssociateWithSpaceCommand(IClientManager clientManager, IConsoleManager consoleManager) : base(clientManager, consoleManager)
    {
    }

    public override async Task<int> ExecuteAsync(CommandContext context, AgentAssociateWithSpaceCommandSettings settings)
    {
        try
        {
            var user = _clientManager.FetchUserProfile(UserContextSettings.ConvertToUserContextSettings(settings));
            var torqueClient = _clientManager.GetClient(user);

            var body = new K8sSpaceAssociationSpec
            {
                Default_namespace = settings.DefaultNamespace,
                Default_service_account = settings.DefaultServiceAccount,
                Type = settings.Type
            };
            
            await torqueClient.SpacesPOSTAsync(settings.AgentName, settings.SpaceName, body);
            _consoleManager.WriteSuccessMessage("Association has been created");
        }
        catch (Exception ex)
        {
            _consoleManager.WriteError(ex);
            return 1;
        }
        return 0;
    }
}