using Quali.Torque.Cli.Models.Settings.Blueprints;
using Spectre.Console.Cli;

namespace Quali.Torque.Cli.Commands.Blueprints;

public class BlueprintListCommand : TorqueBaseCommand<DetailedCommandSettings>
{
    public BlueprintListCommand(IClientManager clientManager,
        IConsoleManager consoleManager) : base(clientManager, consoleManager) { }

    public override async Task<int> ExecuteAsync(CommandContext context, DetailedCommandSettings settings)
    {
        try
        {
            var user = _clientManager.FetchUserProfile(settings);
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