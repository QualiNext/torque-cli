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
    void WriteProfilesList(List<UserProfile> userProfiles, string activeProfile);
    void WriteException(Exception ex, string errorMessage =null );
    void WriteError(string errorMessage);
    void DumpJson(object obj);
    T ReadUserInput<T>(string message, bool optional = false, bool masked = false);
    void WriteEnvironmentCreated(string envId, string envUrl);
    Task WaitEnvironment(EnvironmentWaiterData data);
    void WriteSuccessMessage(string message);
    void WriteEnvironmentDetails(EnvironmentDetailsResponse environment);
    void WriteEnvironmentList(ICollection<EnvironmentListItemResponse> envList);
    void WriteAgentList(ICollection<SpaceComputeServiceResponse> agentsList);
    void WriteSpaceList(ICollection<SpaceListItemResponse> spacesList);
    void WriteInfo(string message);
    void WriteEacList(ICollection<EacResponse> eacList);
    void WritePlan(GetEnvironmentPlanResultResponse plan, string settingsGrainPath);
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

    public void WriteProfilesList(List<UserProfile> userProfiles, string activeProfile)
    {
        var table = new Table();
        table.Border(TableBorder.Minimal);
        table.Title = new TableTitle("Torque user profiles");

        var headerNames = new List<string> {"Active", "Profile Name", "Space", "Repository", "Token"};
        table.AddStyledColumns(headerNames, Color.Blue, Justify.Center);

        foreach (var profile in userProfiles)
        {
            var active = profile.Name == activeProfile ? "+" : "";
            table.AddRow($"[green]{active}[/]", profile.Name, profile.Space, profile.RepositoryName ?? "",
                profile.Token.MaskToken());
        }

        AnsiConsole.Write(table);
    }

    public void WriteException(Exception ex, string errorMessage = null)
    {
#if DEBUG
        if(!string.IsNullOrEmpty(errorMessage))
        {
            WriteError(errorMessage);
        }
        
        AnsiConsole.WriteException(ex);
#else
        if (string.IsNullOrEmpty(errorMessage))
        {
            WriteError(ex.Message);
        }
        else
        {
            WriteError(errorMessage+ ". Exception Message: " + ex.Message);
        }

         
#endif
    }

    public void WriteError(string errorMessage)
    {
        AnsiConsole.Write(new Text(errorMessage, new Style(Color.Red, decoration: Decoration.Bold)));
        AnsiConsole.WriteLine();
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
            environment.State.Grains
                .Select(grain => new Markup($"[aqua bold]{grain.Name}[/]: {grain.State.Current_state}"))
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

    public void WriteAgentList(ICollection<SpaceComputeServiceResponse> agentsList)
    {
        var table = new Table
        {
            Title = new TableTitle($"Torque agents")
        };
        table.AddColumns("Name", "Status");
        foreach (var agent in agentsList)
        {
            table.AddRow(
                agent.Name,
                agent.Status ?? "Unknown");
        }

        AnsiConsole.Write(table);
    }

    public void WriteSpaceList(ICollection<SpaceListItemResponse> spacesList)
    {
        var spaceRows = spacesList.Select(space => new Text(space.Name, new Style(decoration: Decoration.Bold)))
            .ToList();

        AnsiConsole.Write(new Rows(spaceRows));
    }

    public void WriteInfo(string message)
    {
        AnsiConsole.Write(message);
    }

    public void WriteEacList(ICollection<EacResponse> eacList)
    {
        var table = new Table
        {
            Title = new TableTitle($"Torque EaC")
        };
        table.AddColumns("URL", "Environment Name", "Blueprint name", "Owner", "Status", "Environment ID");
        foreach (var eac in eacList)
        {
            table.AddRow(eac.Url, eac.Environment_name.EscapeMarkup(), eac.Blueprint_name.EscapeMarkup(),
                eac.Owner_email.EscapeMarkup(), eac.Status, eac.Environment_id ?? "");
        }

        AnsiConsole.Write(table);
    }

    public void WritePlan(GetEnvironmentPlanResultResponse plan, string grainPath)
    {
        if (grainPath != null)
        {
            var grainInfo = plan.Plan.Environment.Grains.FirstOrDefault(g =>
                string.Equals(g.Path, grainPath, StringComparison.OrdinalIgnoreCase));
            if (grainInfo == null)
            {
                WriteError($"No grain found with path {grainPath}");
                return;
            }

            AnsiConsole.WriteLine(grainInfo.Content);
            return;
        }
        
        var grid = new Grid();
        grid.AddColumn();
        grid.AddColumn();

        grid.AddGridDetailsRow("Status", new Text(plan.Status));
        grid.AddGridDetailsRow("Errors", new Text(plan.Errors != null ? string.Join(", ", plan.Errors): "No Errors found"));

        AnsiConsole.Write(grid);

        if (plan?.Plan?.Environment?.Grains != null)
        {
            var table = new Table
            {
                Title = new TableTitle($"Grains")
            };
            table.AddColumns("Grain path", "Plan");
            foreach (var grain in plan.Plan.Environment.Grains)
            {
                string planString = $"Source commit: {grain.Source_commit.EscapeMarkup()}" + Environment.NewLine +
                                    $"Target commit: {grain.Target_commit.EscapeMarkup()}" + Environment.NewLine + Environment.NewLine +
                                    $"{grain.Content.EscapeMarkup()}";
                table.AddRow(grain.Path.EscapeMarkup(), planString);
            }

            AnsiConsole.Write(table);
        }
    }
}