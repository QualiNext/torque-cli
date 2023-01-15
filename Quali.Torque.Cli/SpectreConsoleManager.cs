using System.ComponentModel;
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
    void WriteEmptyBlueprintList();
    void WriteProfilesList(List<UserProfile> userProfiles);
    void WriteError(Exception ex);
    void DumpJson(object obj);
    T ReadUserInput<T>(string message, bool optional = false, bool masked = false);
}

public sealed class SpectreConsoleManager : IConsoleManager
{
    public void WriteBlueprintList(ICollection<BlueprintForGetAllResponse> blueprintList)
    {
        var table = new Table();
        table.AddColumns("Name", "Repository Name", "Published", "Description");
        foreach (var bp in blueprintList)
        {
            table.AddRow(bp.Name.EscapeMarkup(), bp.Repository_name, bp.Enabled.ToString(), bp.Description ?? "");
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

    public void WriteEmptyBlueprintList()
    {
        throw new NotImplementedException();
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
}