using Newtonsoft.Json;
using Spectre.Console;
using Spectre.Console.Json;
using Torque.Cli.Api;
using Quali.Torque.Cli.Utils;

namespace Quali.Torque.Cli;

public interface IConsoleWriter
{
    void WriteBlueprintList(ICollection<BlueprintForGetAllResponse> blueprintList);
    void WriteBlueprintDetails(BlueprintDetailsResponse blueprintDetails);
    void WriteEmptyBlueprintList();
    void WriteError(Exception ex);
    void DumpJson(object obj);
}

public sealed class SpectreConsoleWriter : IConsoleWriter
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

    public void WriteError(Exception ex)
    {
        AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
    }

    public void DumpJson(object obj)
    {
        var json = JsonConvert.SerializeObject(obj);
        AnsiConsole.Write(new JsonText(json));
    }
}