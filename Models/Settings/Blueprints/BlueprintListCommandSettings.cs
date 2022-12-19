using System.ComponentModel;
using Spectre.Console.Cli;
using Torque.Cli.Commands.Base;

namespace Torque.Cli.Models.Settings.Blueprints
{
    internal class BlueprintListCommandSettings: BaseSettings
    {
        [CommandArgument(1, "<REPOSITORY-NAME>")]
        [Description("The repository name to find a blueprint from")]
        public string? RepositoryName { get; set; }
    }
}

