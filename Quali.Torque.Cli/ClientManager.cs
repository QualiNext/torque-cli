using System.Net.Http.Headers;
using Quali.Torque.Cli.Models;
using Quali.Torque.Cli.Models.Settings.Base;
using Torque.Cli.Api;
using static System.String;

namespace Quali.Torque.Cli;

public interface IClientManager
{
    TorqueApiClient GetClient(UserProfile userProfile);
    UserProfile FetchUserProfile(UserContextSettings settings);
}

public class ClientManager : IClientManager
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IUserProfilesManager _userProfilesManager;
    private readonly IEnvironmentProvider _environmentProvider;

    public ClientManager(IHttpClientFactory httpClientFactory, IUserProfilesManager userProfilesManager, IEnvironmentProvider environmentProvider)
    {
        _httpClientFactory = httpClientFactory;
        _userProfilesManager = userProfilesManager;
        _environmentProvider = environmentProvider;
    }

    public TorqueApiClient GetClient(UserProfile userProfile)
    {
        var httpClient = _httpClientFactory.CreateClient("Default");
        var token = userProfile.Token;
        var baseUrl = userProfile.BaseUrl;
        
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return new TorqueApiClient(baseUrl, httpClient);
    }

    /// <summary>
    /// Read current user context from a config file and override it if needed.
    /// Overriding has the following order:
    /// first, try to from user input params (--space, --token)
    /// then, if params are not set try to read them from env vars
    /// </summary>
    public UserProfile FetchUserProfile(UserContextSettings settings)
    {
        var profileName = settings.Profile ?? "default";
        var userProfile = _userProfilesManager.ReadUserProfile(profileName);

        userProfile.Space = settings.Space ??
                            _environmentProvider.GetEnvironmentVariable(Constants.TorqueSpace) ?? 
                            userProfile.Space;

        userProfile.Token = settings.Token ??
                            _environmentProvider.GetEnvironmentVariable(Constants.TorqueToken) ??
                            userProfile.Token;

        userProfile.RepositoryName = settings.RepositoryName ??
                                     _environmentProvider.GetEnvironmentVariable(Constants.TorqueRepoName) ??
                                     settings.RepositoryName;

        // Since overriding Torque URL is a rear case, we do not store it in a config file but allow reading from user 
        userProfile.BaseUrl = settings.BaseUrl ??
                              _environmentProvider.GetEnvironmentVariable(Constants.DefaultTorqueUrl) ??
                              Constants.DefaultTorqueUrl;

        return userProfile;
    }
}