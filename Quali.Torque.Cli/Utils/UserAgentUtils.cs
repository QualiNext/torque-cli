using System.Diagnostics;
using System.Reflection;

namespace Quali.Torque.Cli.Utils;

public static class UserAgentUtils
{
    public static string GetCurrentVersion()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
        var version = fileVersionInfo.ProductVersion;
        return version;
    }

    public static string[] ParseCustomUserAgent(string value)
    {
        var parts = value.Split('/');
        if (parts.Length != 2)
            throw new ArgumentException("Provided custom user-agent header is not valid");

        return parts;
    }
}