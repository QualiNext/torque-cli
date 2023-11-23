using System.ComponentModel;
using Quali.Torque.Cli.Models.Settings.Base;
using Spectre.Console.Cli;

namespace Quali.Torque.Cli.Models.Settings.Eac;

public class PlanCommandSettings: UserContextSettings
{
    [CommandArgument(0, "<ENVIRONMENT-ID>")]
    [Description("The environment id for plan")]
    public string EnvironmentId { get; set; }
    
    [CommandOption("-g|--grain")]
    [Description("Return the plan for the specified grain")]
    public string GrainPath { get; set; }
}