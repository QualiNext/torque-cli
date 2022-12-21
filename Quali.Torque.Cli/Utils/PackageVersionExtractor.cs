using System.Diagnostics;
using System.Reflection;

namespace Quali.Torque.Cli.Utils;

public static class PackageVersionExtractor
{
    public static string GetVersion()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
        var version = fileVersionInfo.ProductVersion;
        return version;
    }
}