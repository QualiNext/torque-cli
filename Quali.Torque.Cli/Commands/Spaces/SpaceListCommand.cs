using Quali.Torque.Cli.Models.Settings.Base;
using Spectre.Console.Cli;

namespace Quali.Torque.Cli.Commands.Spaces;

public class SpaceListCommand: TorqueBaseCommand<DetailedBaseSettings>
{
    public SpaceListCommand(IClientManager clientManager, IConsoleManager consoleManager) : base(clientManager, consoleManager)
    {
    }

    public override async Task<int> ExecuteAsync(CommandContext context, DetailedBaseSettings settings)
    {
        try
        {
            var user = _clientManager.FetchUserProfile(UserContextSettings.ConvertToUserContextSettings(settings));
            var torqueClient = _clientManager.GetClient(user);

            var spacesList = await torqueClient.SpacesAllAsync();

            if (spacesList.Count > 0)
            {
                if (settings.Detail)
                {
                    _consoleManager.DumpJson(spacesList);
                }
                else
                {
                    _consoleManager.WriteSpaceList(spacesList);
                }
            }
            else
            {
                _consoleManager.WriteEmptyList("No spaces found");
            }

            return 0;
        }
        catch (Exception ex)
        {
            _consoleManager.WriteError(ex);
            return 1;
        }

        return 0;
    }
}