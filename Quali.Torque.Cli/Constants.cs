namespace Quali.Torque.Cli;

public static class Constants
{
    public const string DefaultUserAgentValue = "Torque-Cli";
    public const string DefaultTorqueUrl = "https://portal.qtorque.io";

    public const string SuccessStatus = "Active";

    public static readonly string[] EnvFinalStatuses =
        {"Active", "Active With Error", "Terminating Failed", "Ended", "Force Ended", "Ended With Error"};

    public static readonly string[] GitProviders =
    {
        "github", "bitbucket", "githubEnterprise", "gitLab", "gitLabEnterprise", "bitbucketServer", "Azure", "AzureEnterprise"
    };
}