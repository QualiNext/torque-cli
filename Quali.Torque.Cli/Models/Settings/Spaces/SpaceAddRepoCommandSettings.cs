using System.ComponentModel;
using System.Linq;
using Quali.Torque.Cli.Models.Settings.Base;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Quali.Torque.Cli.Models.Settings.Spaces;

public class SpaceAddRepoCommandSettings: BaseSettings
{
    [CommandArgument(0, "<SPACE-NAME>")]
    [Description("The name of space to add repo to")]
    public string SpaceName { get; set; }
    
    [CommandOption("--type <REPO-TYPE>")]
    [Description($"The type of repository provider")]
    public string RepositoryType{ get; set; }
    
    [CommandOption("--name|-n <REPO-NAME>")]
    [Description("The name of repository to be added to Torque space")]
    public string RepositoryName{ get; set; }
    
    [CommandOption("--branch|-b <BRANCH-NAME>")]
    [Description("The branch name of repository to be added to Torque space")]
    public string BranchName{ get; set; }
    
    [CommandOption("--url|-u <REPO-URL>")]
    [Description("The url of repository to be added to Torque space")]
    public string RepositoryUrl{ get; set; }
    
    [CommandOption("--access-token|-a <TOKEN>")]
    [Description("The git provider's access token.")]
    public string AccessToken{ get; set; }
    
    public override ValidationResult Validate()
    {
        if (Constants.GitProviders.Contains(RepositoryType))
            return ValidationResult.Error($"--type value must be one of: '{string.Join(", ", Constants.GitProviders)}'");

        return ValidationResult.Success();
    }
}