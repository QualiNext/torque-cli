using System.ComponentModel;
using Quali.Torque.Cli.Models.Settings.Blueprints;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Quali.Torque.Cli.Models.Settings.Environments;

public class EnvironmentStartCommandSettings: DetailedCommandSettings
{
    [CommandArgument(0, "<BLUEPRINT-NAME>")]
    [Description("The bluerpint name to run an envrironment from")]
    public string BlueprintName { get; set; }
    
    [CommandOption("-d|--duration <MINUTES>")]
    [Description("The Environment will automatically de-provision at the end of the provided duration (minutes).")]
    [DefaultValue(120)]
    public int Duration { get; set; }
    
    [CommandOption("-n|--name <ENV-NAME>")]
    [Description("Provide a name for the Environment. If not set, the name will be generated automatically " +
                 "using the source branch (or local changes) and current time.")]
    public string Name { get; set; }
    
    [CommandOption("-i|--input <KEY=VALUE>")]
    [Description("The Blueprints inputs can be provided as a key=value pair. " +
                 "For example: -i key1=value1 -i key2=value2.")]
    public IDictionary<string, string> Inputs { get; set; }
    
    [CommandOption("-b|--branch <BRANCH>")]
    [Description("Run the Blueprint version from a remote Git branch")]
    public string Branch { get; set; }
    
    [CommandOption("-c|--commit <COMMIT>")]
    [Description("Specify a specific Commit ID. This is used in order to run a environment from a " +
                 "specific Blueprint historic version. If this parameter is used, the " +
                 "Branch parameter must also be specified.")]
    public string CommitId { get; set; }
    
    [CommandOption("--timeout <MINUTES>")]
    [DefaultValue(30)]
    [Description("Set how long (default timeout is 30 minutes) to block and wait before releasing" +
                 "control back to shell prompt. If timeout is reached before the desired status" +
                 "the wait loop will be interrupted.")]
    public int Timeout { get; set; }
    
    [CommandOption("-w|--wait")]
    [Description("Block shell prompt and wait for the Environment to be Active while the timeout is not reached." +
                 "Default timeout is 30 minutes." +
                 "The default timeout can be changed using the 'timeout' flag.")]
    public bool WaitActive { get; set; }
    
    public override ValidationResult Validate()
    {
        var errors = new List<string>();
        
        if (Branch is null && CommitId is not null)
            errors.Add("Since commit is specified, branch is required!");
        
        if (Timeout < 0)
            errors.Add("Timeout must be positive!");
        
        if (Duration < 0)
            errors.Add("Duration must be positive!");

        return errors.Count > 0
            ? ValidationResult.Error(string.Join(Environment.NewLine, errors))
            : ValidationResult.Success();
    }
}