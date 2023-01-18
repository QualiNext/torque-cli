using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Quali.Torque.Cli.Models.Settings.Blueprints;

internal class BlueprintValidateCommandSettings : DetailedCommandSettings
{
    [CommandArgument(0, "<BLUEPRINT-FILE>")]
    [Description("The path to a file with a blueprint ")]
    public string BlueprintFile { get; set; }
    
    public override ValidationResult Validate()
    {
        return File.Exists(BlueprintFile)
            ? ValidationResult.Success()
            : ValidationResult.Error("Provided file does not exist");
    }
}