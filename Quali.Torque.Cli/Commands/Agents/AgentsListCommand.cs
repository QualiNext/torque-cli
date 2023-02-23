using Quali.Torque.Cli.Models.Settings.Agents;

namespace Quali.Torque.Cli.Commands.Agents;

public class AgentsListCommand: TorqueAdminScopedCommand<AgentsListCommandSettings>
{
    public AgentsListCommand(IClientManager clientManager, IConsoleManager consoleManager) : base(clientManager, consoleManager)
    {
    }

    protected override async Task RunTorqueCommandAsync(AgentsListCommandSettings settings)
    {
        var agentsList = await Client.AgentsAllAsync(settings.SpaceName);

        if (agentsList.Count > 0)
        {
            if (settings.Detail)
            {
                ConsoleManager.DumpJson(agentsList);
            }
            else
            {
                ConsoleManager.WriteAgentList(agentsList);
            }
        }
    }
}