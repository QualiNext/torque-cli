using Quali.Torque.Cli.Models;
using Salaros.Configuration;

namespace Quali.Torque.Cli;

public interface IUserProfilesManager
{
    void WriteUserProfile(string profileName, UserProfile profile);
    UserProfile ReadUserProfile(string profileName);
}

public class UserProfilesManager : IUserProfilesManager
{
    private readonly string _configPath;
    private readonly Lazy<ConfigParser> _configParserLazy;

    public UserProfilesManager(IEnvironmentProvider environmentProvider)
    {
        //TODO: Move to constant file
        var configPath = environmentProvider.GetEnvironmentVariable(Constants.ConfigFileEnvVarName); 
        
        if (!String.IsNullOrEmpty(configPath))
        {
            _configPath = configPath;
        }
        else
        {
            _configPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                ".torque",
                "config");
        }

        _configParserLazy = new Lazy<ConfigParser>(() => CreateParser());
    }

    private ConfigParser CreateParser()
    {
        try
        {
            return new ConfigParser(_configPath);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Some error message", ex);
        }
    }

    public void WriteUserProfile(string profileName, UserProfile profile)
    {
        _configParserLazy.Value.SetValue(profileName, "token", profile.Token);
        _configParserLazy.Value.SetValue(profileName, "space", profile.Space);
        _configParserLazy.Value.SetValue(profileName, "repository", profile.RepositoryName);

        _configParserLazy.Value.Save();
    }

    public UserProfile ReadUserProfile(string profileName)
    {
        var userProfile = new UserProfile
        {
            Space = _configParserLazy.Value.GetValue(profileName, "space", ""),
            Token = _configParserLazy.Value.GetValue(profileName, "token", ""),
            RepositoryName = _configParserLazy.Value.GetValue(profileName, "repository", "")
        };
        return userProfile;
    }
}