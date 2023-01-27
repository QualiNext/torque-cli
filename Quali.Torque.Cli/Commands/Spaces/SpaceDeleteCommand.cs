using Quali.Torque.Cli.Models.Settings.Base;
using Quali.Torque.Cli.Models.Settings.Spaces;
using Spectre.Console.Cli;

namespace Quali.Torque.Cli.Commands.Spaces;

public class SpaceDeleteCommand: TorqueBaseCommand<SpaceDeleteCommandSettings>
{
    public SpaceDeleteCommand(IClientManager clientManager, IConsoleManager consoleManager) : base(clientManager, consoleManager)
    {
    }

    public override async Task<int> ExecuteAsync(CommandContext context, SpaceDeleteCommandSettings settings)
    {
        try
        {
            var user = _clientManager.FetchUserProfile(UserContextSettings.ConvertToUserContextSettings(settings));
            var torqueClient = _clientManager.GetClient(user);

            await torqueClient.SpacesDELETEAsync(settings.SpaceName);
            _consoleManager.WriteSuccessMessage($"Space '{settings.SpaceName}' has been deleted.");
        }
        catch (Exception ex)
        {
            _consoleManager.WriteError(ex);
            return 1;
        }

        return 0;
    }
}