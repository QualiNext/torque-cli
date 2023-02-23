using Quali.Torque.Cli.Models;

namespace Quali.Torque.Cli;

public interface IUserProfilesManager
{
    void WriteUserProfile(UserProfile profile);
    UserProfile ReadUserProfile(string profileName);
    List<UserProfile> ListUserProfiles();
    UserProfile ReadActiveUserProfile();
    void SetActiveUserProfile(string profileName);
    void RemoveUserProfile(string profileName);
}

public class ProfileNotFoundException : Exception
{
    public ProfileNotFoundException(string message)
        : base(message) { }
}

public class DuplicatedProfilesFoundException : Exception
{
    public DuplicatedProfilesFoundException(string message)
        : base(message) { }
}

public class UserProfilesManager : IUserProfilesManager
{
    private readonly string _configPath;
    private readonly ITorqueConfigurationProvider _torqueConfigurationProvider;
    private readonly TorqueConfiguration _torqueConfiguration;

    public UserProfilesManager(IEnvironmentProvider environmentProvider, ITorqueConfigurationProvider torqueConfigurationProvider)
    {
        _torqueConfigurationProvider = torqueConfigurationProvider;
        var configPath = environmentProvider.GetEnvironmentVariable(Constants.ConfigFileEnvVarName);
        
        if (!string.IsNullOrEmpty(configPath))
        {
            _configPath = configPath;
        }
        else
        {
            _configPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                ".torque",
                "config.yml");
        }

        try
        {
            _torqueConfiguration = _torqueConfigurationProvider.LoadConfiguration(_configPath);
        }
        catch (Exception)
        {
            // TODO: log?
            _torqueConfiguration = new TorqueConfiguration
            {
                ActiveProfile = null,
                Profiles = new List<UserProfile>()
            };
        }
    }

    public void WriteUserProfile(UserProfile profile)
    {
        if (!string.IsNullOrEmpty(profile.Name))
        {
            try
            {
                var found = _torqueConfiguration.Profiles.SingleOrDefault(p => p.Name == profile.Name);
                if (found != null)
                    _torqueConfiguration.Profiles.Remove(found);
            }
            catch (InvalidOperationException)
            {
                throw new DuplicatedProfilesFoundException(
                    $"Unable to update profile. Configuration has more than one profile with name '{profile.Name}'");
            }
            // a fresh config
            if (_torqueConfiguration.Profiles.Count == 0)
            {
                _torqueConfiguration.ActiveProfile = profile.Name;
            }
            _torqueConfiguration.Profiles.Add(profile);
            _torqueConfigurationProvider.SaveConfiguration(_torqueConfiguration, _configPath);
        }
        
        else
        {
            throw new ArgumentException("Profile name must be defined");
        }
    }

    public UserProfile ReadUserProfile(string profileName)
    {
        try
        {
            var result = _torqueConfiguration.Profiles.SingleOrDefault(profile => profile.Name == profileName);
            if (result is null)
                throw new ProfileNotFoundException(
                    $"Profile with name '{profileName}' was not found in the configuration");
            return result;
        }
        catch (InvalidOperationException)
        {
            throw new DuplicatedProfilesFoundException(
                $"Unable to read profile. Configuration has more than one profile with name '{profileName}'");
        }
    }

    public List<UserProfile> ListUserProfiles()
    {
        return _torqueConfiguration.Profiles;
    }

    public UserProfile ReadActiveUserProfile()
    {
        if (_torqueConfiguration.ActiveProfile is null)
            throw new InvalidOperationException("Active profile is not set");

        try
        {
            var result=  _torqueConfiguration.Profiles.SingleOrDefault(profile =>
                _torqueConfiguration.ActiveProfile == profile.Name);
            if (result is null)
                throw new ProfileNotFoundException(
                    "Profile with the name set as an active was not found in the configuration");
            return result;
        }
        catch (InvalidOperationException)
        {
            throw new DuplicatedProfilesFoundException(
                "Profile with the name set as an active appears in the configuration more than once");
        }
    }

    public void SetActiveUserProfile(string profileName)
    {
        try
        {
            var profile = _torqueConfiguration.Profiles.SingleOrDefault(profile => profile.Name == profileName);
            if (profile is null)
                throw new ProfileNotFoundException(
                    $"Unable to set active profile since the profile with name '{profileName}' was not found in the configuration");
            _torqueConfiguration.ActiveProfile = profileName;
            _torqueConfigurationProvider.SaveConfiguration(_torqueConfiguration, _configPath);
            
        }
        catch (InvalidOperationException)
        {
            throw new DuplicatedProfilesFoundException(
                $"Unable to set active profile. Configuration has more than one profile with name '{profileName}'");
        }
    }

    public void RemoveUserProfile(string profileName)
    {
        try
        {
            var targetProfile = _torqueConfiguration.Profiles.SingleOrDefault(profile => profile.Name == profileName);
            if (targetProfile is not null)
            {
                _torqueConfiguration.Profiles.Remove(targetProfile);
                
            }
            else
            {
                throw new ProfileNotFoundException(
                    $"Profile with name '{profileName}' was not found in the configuration");
            }
        }
        catch (InvalidOperationException)
        {
            throw new DuplicatedProfilesFoundException(
                $"Unable to remove profile. Configuration has more than one profile with name '{profileName}'");
        }
        // Check if active profile was associated with just removed item
        if (_torqueConfiguration.ActiveProfile == profileName)
        {
            var activeCandidate = _torqueConfiguration.Profiles.FirstOrDefault();
            if (activeCandidate is not null)
                _torqueConfiguration.ActiveProfile = activeCandidate.Name;
        }
        _torqueConfigurationProvider.SaveConfiguration(_torqueConfiguration, _configPath);    
    }
}