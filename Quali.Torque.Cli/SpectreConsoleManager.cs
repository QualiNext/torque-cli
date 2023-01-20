using Newtonsoft.Json;
using Quali.Torque.Cli.Models;
using Spectre.Console;
using Spectre.Console.Json;
using Torque.Cli.Api;
using Quali.Torque.Cli.Utils;

namespace Quali.Torque.Cli;

public interface IConsoleManager
{
    void WriteBlueprintList(ICollection<BlueprintForGetAllResponse> blueprintList);
    void WriteBlueprintDetails(BlueprintDetailsResponse blueprintDetails);
    void WriteBlueprintsErrors(BlueprintValidationResponse result);
    void WriteEmptyList(string title);
    void WriteProfilesList(List<UserProfile> userProfiles);
    void WriteError(Exception ex);
    void DumpJson(object obj);
    T ReadUserInput<T>(string message, bool optional = false, bool masked = false);
    void WriteEnvironmentCreated(string envId, string envUrl);
    Task WaitEnvironment(EnvironmentWaiterData data);
    void WriteSuccessMessage(string message);
    void WriteEnvironmentDetails(EnvironmentDetailsResponse environment);
    void WriteEnvironmentList(ICollection<EnvironmentListItemResponse> envList);
}

public sealed class SpectreConsoleManager : IConsoleManager
{
    public void WriteBlueprintList(ICollection<BlueprintForGetAllResponse> blueprintList)
    {
        var table = new Table
        {
            Title = new TableTitle($"Torque blueprints")
        };
        table.AddColumns("Name", "Repository Name", "Published", "Description");
        foreach (var bp in blueprintList)
        {
            table.AddRow(bp.Name.EscapeMarkup(), bp.Repository_name, bp.Enabled.ToString(), bp.Description ?? "");
        }

        AnsiConsole.Write(table);
    }
    
    public void WriteBlueprintsErrors(BlueprintValidationResponse result)
    {
        if (result.Errors.Count == 0)
        {
            AnsiConsole.MarkupLine("[green bold]Blueprint is valid[/]");
            return;
        }
        var table = new Table
        {
            Title = new TableTitle($"Blueprint Errors")
        };
        table.AddColumns("Code", "Name", "Message");
        table.BorderColor(Color.Red);
        foreach (var error in result.Errors)
        {
            table.AddRow(error.Code, error.Name, error.Message);
        }
        AnsiConsole.Write(table);
    }

    public void WriteBlueprintDetails(BlueprintDetailsResponse blueprintDetails)
    {
        var grid = new Grid();
        var valueColumn = new GridColumn
        {
            Width = 60
        };

        grid.AddColumn();
        grid.AddColumn(valueColumn);

        grid.AddGridDetailsRow("Name", new Text(blueprintDetails.Details.Name));
        grid.AddGridDetailsRow("Description", new Text(blueprintDetails.Details.Description));
        grid.AddGridDetailsRow("Repository", new Text(blueprintDetails.Details.Repository_name));

        if (blueprintDetails.Details.Inputs.Count > 0)
        {
            var inputRows = blueprintDetails.Details.Inputs.Select(input => input.Has_default_value
                    ? $"{input.Name} (default: {input.Default_value})"
                    : input.Name)
                .Select(value => new Text(value))
                .ToList();
            grid.AddGridDetailsRow("Inputs", new Rows(inputRows));
        }

        if (blueprintDetails.Details.Outputs.Count > 0)
        {
            var outputRows = blueprintDetails.Details.Outputs
                .Select(output => new Text($"{output.Name}: {output.Value ?? "none"}")).ToList();
            grid.AddGridDetailsRow("Outputs", new Rows(outputRows));
        }

        AnsiConsole.Write(grid);
    }

    public void WriteEmptyList(string title)
    {
        AnsiConsole.MarkupLine($"[red]{title}[/]");
    }

    public void WriteProfilesList(List<UserProfile> userProfiles)
    {
        var table = new Table();
        table.Border(TableBorder.Minimal);
        table.Title = new TableTitle("Torque user profiles");
        table.AddColumns("Profile Name", "Space", "Repository", "Token");
        foreach (var profile in userProfiles)
        {
            table.AddRow(profile.Name, profile.Space, profile.RepositoryName, profile.Token.MaskToken());
        }
        AnsiConsole.Write(table);
    }

    public void WriteError(Exception ex)
    {
        AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
    }

    public void DumpJson(object obj)
    {
        var json = JsonConvert.SerializeObject(obj);
        AnsiConsole.Write(new JsonText(json));
    }

    public T ReadUserInput<T>(string message, bool optional = false, bool masked = false)
    {
        var textPrompt = new TextPrompt<T>(message.EscapeMarkup());

        if (optional)
            textPrompt.AllowEmpty();

        if (masked)
            textPrompt.Secret();

        return AnsiConsole.Prompt(textPrompt);
    }

    public void WriteEnvironmentCreated(string envId, string envUrl)
    {
        AnsiConsole.Write(new Rows(
            new Text($"ID: {envId}", new Style(Color.Blue)),
            new Text($"Url: {envUrl}", new Style(Color.Green))
        ));

    }

    public async Task WaitEnvironment(EnvironmentWaiterData data)
    {
        var spinnerMsg = $"Starting an environment {data.EnvironmentId}...";
        await AnsiConsole.Status()
            .SpinnerStyle(Style.Parse("green bold"))
            .StartAsync(spinnerMsg, async ctx =>
            {
                var startTime = DateTimeOffset.Now.ToUnixTimeSeconds();
                var env = await data.Client.EnvironmentsGETAsync(data.Space, data.EnvironmentId);
                var lastGrainStates = new Dictionary<string, string>();

                while (DateTimeOffset.Now.ToUnixTimeSeconds() - startTime < data.Timeout * 60)
                {
                    var status = env.Details.Computed_status;
                    if (Constants.EnvFinalStatuses.Contains(status))
                    {
                        if (status == Constants.SuccessStatus)
                        {
                            AnsiConsole.MarkupLine("[green bold]Environment started successfully![/]");
                            return true;
                        }

                        AnsiConsole.MarkupLine(
                            $"[red]Environment failed to start. Current status is [bold]{status}[/][/]");
                        return false;
                    }

                    await Task.Delay(5000);

                    env = await data.Client.EnvironmentsGETAsync(data.Space, data.EnvironmentId);

                    foreach (var grain in env.Details.State.Grains)
                    {
                        lastGrainStates.TryGetValue(grain.Name, out var state);

                        if (state == grain.State.Current_state) continue;
                        lastGrainStates[grain.Name] = grain.State.Current_state;
                        AnsiConsole.MarkupLine(
                            $"[blue]Grain {grain.Name} is now [bold]{grain.State.Current_state}[/][/]");
                    }

                    ctx.Status($"{spinnerMsg} {DateTimeOffset.Now.ToUnixTimeSeconds() - startTime} sec");
                }

                throw new TimeoutException($"Environment was not active after {data.Timeout.ToString()} minutes");

            });
    }

    public void WriteSuccessMessage(string message)
    {
        AnsiConsole.MarkupLine($"[green]{message}[/]");
    }

    public void WriteEnvironmentDetails(EnvironmentDetailsResponse environment)
    {
        var grid = new Grid();
        grid.AddColumn();
        grid.AddColumn();

        grid.AddGridDetailsRow("ID", new Text(environment.Id));
        grid.AddGridDetailsRow("Name", new Text(environment.Definition.Metadata.Name));
        grid.AddGridDetailsRow("Blueprint Name", new Text(environment.Definition.Metadata.Blueprint_name));
        grid.AddGridDetailsRow("State", new Text(environment.Computed_status));

        var grainRows =
            environment.State.Grains.Select(grain => new Markup($"[aqua bold]{grain.Name}[/]: {grain.State.Current_state}"))
                .ToList();
        grid.AddGridDetailsRow("Grains", new Rows(grainRows));

        AnsiConsole.Write(grid);
    }

    public void WriteEnvironmentList(ICollection<EnvironmentListItemResponse> envList)
    {
        var table = new Table
        {
            Title = new TableTitle($"Torque environments")
        };
        table.AddColumns("Id", "Name", "Blueprint Name", "Status");
        foreach (var env in envList)
        {
            table.AddRow(
                env.Id,
                env.Details.Definition.Metadata.Name.EscapeMarkup(),
                env.Details.Definition.Metadata.Blueprint_name.EscapeMarkup(),
                env.Details.Computed_status);
        }
        AnsiConsole.Write(table);
    }
}
