using Torque.Cli.Api;

namespace Quali.Torque.Cli.Utils;

public static class EnvironmentHelper
{
    public static bool TryGetEnvIdFromUrl(string possibleEnvironmentUrl, IEnumerable<EacResponse> eacList, out string environmentId)
    {
        environmentId = null;
        if (Uri.IsWellFormedUriString(possibleEnvironmentUrl, UriKind.Absolute))
        {
            var eac = eacList.FirstOrDefault(e => string.Equals(e.Url, possibleEnvironmentUrl, StringComparison.OrdinalIgnoreCase));
            if (eac == null)
                throw new ArgumentException("No environment matching the URL found.");

            if (string.IsNullOrEmpty(eac.Environment_id))
                throw new ArgumentException("The environment matching the URL is currently inactive.");

            environmentId = eac.Environment_id;
            return true;
        }

        return false;
    }
}