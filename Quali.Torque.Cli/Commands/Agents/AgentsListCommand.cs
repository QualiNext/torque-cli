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
            var user = _clientManager.FetchUserProfile(UserContextSettings.  settings);
            var torqueClient = _clientManager.GetClient(user);

            var blueprintList = await torqueClient.BlueprintsAllAsync(user.Space);

            if (blueprintList.Count > 0)
            {
                if (settings.Detail)
                {
                    _consoleManager.DumpJson(blueprintList);
                }
                else
                {
                    _consoleManager.WriteBlueprintList(blueprintList);
                }
            }
            else
            {
                _consoleManager.WriteEmptyList("No blueprints found");
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