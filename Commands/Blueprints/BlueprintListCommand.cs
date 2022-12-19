using Spectre.Console.Cli;
using Spectre.Console;
using Torque.Cli.Models.Settings.Blueprints;

namespace Torque.Cli.Commands.Blueprints
{
    internal class BlueprintListCommand : AsyncCommand<BlueprintListCommandSettings>
    {
        private readonly ClientManager _clientManager;

        public BlueprintListCommand(ClientManager clientManager)
        {
            _clientManager = clientManager;
        }

        public override async Task<int> ExecuteAsync(CommandContext context, BlueprintListCommandSettings settings)
        {
            var user = _clientManager.FetchUserProfile(settings);
            var torqueClient = _clientManager.GetClient(user);

            var blueprintList = await torqueClient.BlueprintsAllAsync(settings.Space);
            AnsiConsole.MarkupLine("Blueprint List");

            foreach (var bp in blueprintList)
            {
                var result = bp.Name;
                AnsiConsole.WriteLine(result);
            }

            return 0;
        }
    }
}