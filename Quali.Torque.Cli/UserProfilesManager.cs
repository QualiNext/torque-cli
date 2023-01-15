using Quali.Torque.Cli.Models;
using Salaros.Configuration;

namespace Quali.Torque.Cli;

public interface IUserProfilesManager
{
    void WriteUserProfile(UserProfile profile);
    UserProfile ReadUserProfile(string profileName);
    List<UserProfile> ListUserProfiles();
}

public class UserProfilesManager : IUserProfilesManager
{
    private readonly string _configPath;
    private readonly Lazy<ConfigParser> _configParserLazy;

    public UserProfilesManager(IEnvironmentProvider environmentProvider)
    {
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
            throw new InvalidOperationException("Unable to create parser", ex);
        }
    }

    public void WriteUserProfile(UserProfile profile)
    {
        if (!string.IsNullOrEmpty(profile.Name))
        {
            _configParserLazy.Value.SetValue(profile.Name, "token", profile.Token);
            _configParserLazy.Value.SetValue(profile.Name, "space", profile.Space);
            _configParserLazy.Value.SetValue(profile.Name, "repository", profile.RepositoryName);
            
            _configParserLazy.Value.Save();
        }
        else
        {
            throw new ArgumentException("Profile name must be defined");
        }
    }

    public UserProfile ReadUserProfile(string profileName)
    {
        var userProfile = new UserProfile
        {
            Space = _configParserLazy.Value.GetValue(profileName, "space", ""),
            Token = _configParserLazy.Value.GetValue(profileName, "token", ""),
            RepositoryName = _configParserLazy.Value.GetValue(profileName, "repository", ""),
            Name = profileName
        };
        return userProfile;
    }

    public List<UserProfile> ListUserProfiles()
    {
        var userProfiles = new List<UserProfile>();
        foreach (var profileName in _configParserLazy.Value.Sections.Select(section => section.SectionName).ToList())
        {
            userProfiles.Add(new UserProfile
            {
                Name = profileName,
                Space = _configParserLazy.Value.GetValue(profileName, "space", ""),
                Token = _configParserLazy.Value.GetValue(profileName, "token", ""),
                RepositoryName = _configParserLazy.Value.GetValue(profileName, "repository", "")
            });
        }

        return userProfiles;
    }

    // TODO: Looks like there is no way to remove section using this library
    public void RemoveUserProfile(string profileName)
    {
        throw new NotImplementedException();
    }
}