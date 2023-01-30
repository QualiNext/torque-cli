using Quali.Torque.Cli.Models.Settings.Base;
using Quali.Torque.Cli.Models.Settings.Spaces;
using Spectre.Console.Cli;
using Torque.Cli.Api;

namespace Quali.Torque.Cli.Commands.Spaces;

public class SpaceCreateCommand: TorqueBaseCommand<SpaceCreateCommandSettings>
{
    public SpaceCreateCommand(IClientManager clientManager, IConsoleManager consoleManager) : base(clientManager, consoleManager)
    { 
    }
    
    public override async Task<int> ExecuteAsync(CommandContext context, SpaceCreateCommandSettings settings)
    {
        try
        {
            var user = _clientManager.FetchUserProfile(UserContextSettings.ConvertToUserContextSettings(settings));
            var torqueClient = _clientManager.GetClient(user);

            await torqueClient.SpacesPOST2Async(new CreateSpaceRequest {Name = settings.SpaceName});
            _consoleManager.WriteSuccessMessage($"Space '{settings.SpaceName}' has been created.");
        }
        catch (Exception ex)
        {
            _consoleManager.WriteError(ex);
            return 1;
        }

        return 0;
    }

    
}