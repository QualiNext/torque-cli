using System.ComponentModel;
using Quali.Torque.Cli.Models.Settings.Base;
using Quali.Torque.Cli.Models.Settings.Blueprints;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Quali.Torque.Cli.Models.Settings.Environments;

public class EnvironmentBulkStartUserContextSettings: DetailedUserContextSettings
{
    [CommandArgument(0, "<CSV file path>")]
    [Description("Path to a CSV with the columns [Space, Blueprint, Repository, Duration (Minutes), Owners, Inputs] where Inputs is in the format name:value;name:value")]
    public string CsvPath { get; set; }
    
    public override ValidationResult Validate()
    {
        var errors = new List<string>();
        
        if (!File.Exists(CsvPath))
            errors.Add($"CSV file could not be found at {CsvPath}");
        
        return errors.Count > 0
            ? ValidationResult.Error(string.Join(Environment.NewLine, errors))
            : ValidationResult.Success();
    }
}