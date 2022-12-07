using Spectre.Console.Cli;
using Spectre.Console;
using System.ComponentModel;
using Torque.Cli.Api;
using System.Net.Http.Headers;

namespace Torque.Cli.Commands.Blueprints;

public sealed class BlueprintGetCommand : AsyncCommand<BlueprintGetCommand.Settings>
{
    // public class Settings : Base.BaseSettings
    public sealed class Settings : Base.BaseSettings
    {
        [CommandArgument(0, "<BLUEPRINT-NAME>")]
        [Description("The blueprint name to show")]
        public string BlueprintName { get; set; }

        [CommandArgument(1, "<REPOSITORY-NAME>")]
        [Description("The repository name to find a blueprint from")]
        public string RepositoryName { get; set; }
    }

    async public override Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var token = settings.Token;
        var space = settings.Space;
        var HttpClient = new HttpClient();

        // HttpClient.BaseAddress = new Uri("https://portal.qtorque.io/api");
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var client = new TorqueApiClient("https://portal.qtorque.io/api/", HttpClient);
        var BlueprintDetails = await client.CatalogGETAsync(space, settings.BlueprintName, settings.RepositoryName);

        AnsiConsole.MarkupLine($"[bold]Get Blueprint =>[/] name[[{settings.BlueprintName}]]");
        AnsiConsole.WriteLine($"Details: {BlueprintDetails.Details.ToString()}");
        return 0;
    }
}
