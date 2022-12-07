using Spectre.Console.Cli;
using Spectre.Console;
using System.Net.Http.Headers;
using Torque.Cli.Api;
using System.ComponentModel;

namespace Torque.Cli.Commands.Blueprints;

public sealed class BlueprintListCommand : AsyncCommand<BlueprintListCommand.Settings>
{
    public sealed class Settings : Base.BaseSettings
    {
        [CommandArgument(1, "<REPOSITORY-NAME>")]
        [Description("The repository name to find a blueprint from")]
        public string RepositoryName { get; set; }
    }

    async public override Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var token = settings.Token;
        var HttpClient = new HttpClient();

        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var client = new TorqueApiClient("https://portal.qtorque.io/", HttpClient);
        var BlueprintList = await client.BlueprintsAllAsync(settings.Space);
        
        AnsiConsole.MarkupLine("Blueprint List");

        foreach (var bp in BlueprintList)
        {
            string result = bp.Name;
            AnsiConsole.WriteLine(result);
        }
                
        return 0;
    }
}