using Quali.Torque.Cli.Models;
using Salaros.Configuration;

namespace Quali.Torque.Cli;

public class UserProfilesManager
{
    private string _configPath;
    private readonly ConfigParser _configParser;

    public UserProfilesManager(string configPath)
    {
        if (!String.IsNullOrEmpty(configPath))
            _configPath = configPath;
        else
            _configPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                ".torque",
                "config");

        _configParser = new ConfigParser(_configPath);
    }

    public void WriteUserProfile(string profileName, UserProfile profile)
    {
        _configParser.SetValue(profileName, "token", profile.Token);
        _configParser.SetValue(profileName, "space", profile.Space);
        _configParser.SetValue(profileName, "repository", profile.RepositoryName);

        _configParser.Save();
    }

    public UserProfile ReadUserProfile(string profileName)
    {
        var userProfile = new UserProfile
        {
            Space = _configParser.GetValue(profileName, "space", ""),
            Token = _configParser.GetValue(profileName, "token", ""),
            RepositoryName = _configParser.GetValue(profileName, "repository", "")
        };
        return userProfile;
    }
}