using System.ComponentModel;
using Quali.Torque.Cli.Models.Settings.Blueprints;
using Spectre.Console.Cli;
using ValidationResult = Spectre.Console.ValidationResult;

namespace Quali.Torque.Cli.Models.Settings.Environments;

public class EnvironmentListCommandSettings: DetailedCommandSettings
{
    [CommandOption("--show-ended")]
    [Description("Show ended environments")]
    public bool ShowEnded { get; set; }
    
    [CommandOption("--count <N>")]
    [Description("Amount of environment to be retrieved")]
    [DefaultValue(20)]
    public int Count { get; set; }
    
    [CommandOption("--filter <VALUE>")]
    [Description("{all|my|auto} Show all environments, only user's environments or environments launched by any automation")]
    [DefaultValue("my")]
    public string Filter{ get; set; }
    
    public override ValidationResult Validate()
    {
        var possibleFilters = new List<string> {"my", "all", "auto"};
        if (!possibleFilters.Contains(Filter))
            return ValidationResult.Error($"--filter value must be one of: '{string.Join(", ", possibleFilters)}'");

        if (Count <= 0)
            return ValidationResult.Error("--count value must be positive");
        
        return ValidationResult.Success();
    }
}