using System.ComponentModel;
using Quali.Torque.Cli.Commands.Base;
using Spectre.Console.Cli;

namespace Quali.Torque.Cli.Models.Settings.Blueprints
{
    internal class BlueprintListCommandSettings: BaseSettings
    {
        [CommandArgument(1, "<REPOSITORY-NAME>")]
        [Description("The repository name to find a blueprint from")]
        public string RepositoryName { get; set; }
    }
}

