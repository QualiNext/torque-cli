using Quali.Torque.Cli.Models.Settings.Agents;
using Quali.Torque.Cli.Models.Settings.Base;
using Spectre.Console.Cli;

namespace Quali.Torque.Cli.Commands.Agents;

public class AgentsListCommand: TorqueBaseCommand<AgentsListCommandSettings>
{
    public AgentsListCommand(IClientManager clientManager, IConsoleManager consoleManager) : base(clientManager, consoleManager)
    {
    }

    public override async Task<int> ExecuteAsync(CommandContext context, AgentsListCommandSettings settings)
    {
        try
        {
            var user = _clientManager.FetchUserProfile(UserContextSettings.ConvertToUserContextSettings(settings));
            var torqueClient = _clientManager.GetClient(user);

            var agentsList = await torqueClient.AgentsAllAsync(settings.SpaceName);

            if (agentsList.Count > 0)
            {
                if (settings.Detail)
                {
                    _consoleManager.DumpJson(agentsList);
                }
                else
                {
                    _consoleManager.WriteAgentList(agentsList);
                }
            }
            else
            {
                _consoleManager.WriteEmptyList("No agents found");
            }
        }
        catch (Exception ex)
        {
            _consoleManager.WriteError(ex);
            return 1;
        }


        return 0;
    }
}