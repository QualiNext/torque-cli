using Spectre.Console;
using Torque.Cli.Api;

namespace Quali.Torque.Cli;

public interface IConsoleWriter
{
    void WriteBlueprintList(ICollection<BlueprintForGetAllResponse> blueprintList);
    void WriteEmptyBlueprintList();
    void WriteError(string errorMessage);
}

public sealed class ConsoleWriter : IConsoleWriter
{
    
    public void WriteBlueprintList(ICollection<BlueprintForGetAllResponse> blueprintList)
    {
        AnsiConsole.MarkupLine("Blueprint List");

        foreach (var bp in blueprintList)
        {
            var result = bp.Name;
            AnsiConsole.WriteLine(result);
        }
    }

    public void WriteEmptyBlueprintList()
    {
        throw new NotImplementedException();
    }

    public void WriteError(string errorMessage)
    {
        throw new NotImplementedException();
    }
}