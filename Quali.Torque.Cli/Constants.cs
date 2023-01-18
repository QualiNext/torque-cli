namespace Quali.Torque.Cli;

public static class Constants
{
    // Env Vars Names
    public static string BaseUrlEnvVarName = "TORQUE_SERVER_URL";
    public static string ConfigFileEnvVarName = "TORQUE_CONFIG_PATH";
    // TODO(ddovbii): user-agent env var usage is not implemented
    public static string UserAgentEnvVarName = "TORQUE_USERAGENT";
 
    public static string UserAgentValue = "Torque-Cli";
    public static string DefaultTorqueUrl = "https://portal.qtorque.io";

    public static string SuccessStatus = "Active";
    public static string[] EnvFinalStatuses =
        {"Active", "Active With Error", "Terminating Failed", "Ended", "Force Ended", "Ended With Error"};
}