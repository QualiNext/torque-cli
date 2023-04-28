using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.PlatformAbstractions;

namespace Quali.Torque.Cli.Utils;

public static class UserAgentUtils
{
    public static string GetCurrentVersion()
    {
        // Microsoft.Extensions.PlatformAbstractions
        var app = PlatformServices.Default.Application;
        return app.ApplicationVersion;
    }

    public static string[] ParseCustomUserAgent(string value)
    {
        var parts = value.Split('/');
        if (parts.Length != 2)
            throw new ArgumentException("Provided custom user-agent header is not valid");

        return parts;
    }
}