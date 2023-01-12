using System.Net.Http.Headers;
using Quali.Torque.Cli.Commands.Base;
using Quali.Torque.Cli.Models;
using Torque.Cli.Api;
using static System.String;

namespace Quali.Torque.Cli;

public interface IClientManager
{
    TorqueApiClient GetClient(UserProfile userProfile);
    UserProfile FetchUserProfile(BaseSettings settings);
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
        var baseUrl = _environmentProvider.GetEnvironmentVariable(Constants.BaseUrlEnvVarName) ?? Constants.DefaultTorqueUrl;
        
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return new TorqueApiClient(baseUrl, httpClient);
    }

    public UserProfile FetchUserProfile(BaseSettings settings)
    {
        var profileName = settings.Profile ?? "default";
        var userProfile = _userProfilesManager.ReadUserProfile(profileName);

        if (!IsNullOrEmpty(settings.Space))
            userProfile.Space = settings.Space;
        
        if (!IsNullOrEmpty(settings.Token))
            userProfile.Token = settings.Token;

        if (!IsNullOrEmpty(settings.RepositoryName))
            userProfile.RepositoryName = settings.RepositoryName;

        return userProfile;
    }
}