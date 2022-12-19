using System.Net.Http.Headers;
using Torque.Cli.Api;
using Torque.Cli.Commands.Base;
using Torque.Cli.Models;

namespace Torque.Cli;

public class ClientManager
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly UserProfilesManager _userProfilesManager;

    public ClientManager(IHttpClientFactory httpClientFactory, UserProfilesManager userProfilesManager)
    {
        _httpClientFactory = httpClientFactory;
        _userProfilesManager = userProfilesManager;
    }

    public TorqueApiClient GetClient(UserProfile userProfile)
    {
        var httpClient = _httpClientFactory.CreateClient();
        var token = userProfile.Token;
        var baseUrl = Environment.GetEnvironmentVariable("TORQUE_SERVER_URL") ?? "https://portal.qtorque.io/api/";
        
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return new TorqueApiClient(baseUrl, httpClient);
    }

    public UserProfile FetchUserProfile(BaseSettings settings)
    {
        var profileName = settings.Profile ?? "default";
        var userProfile = _userProfilesManager.ReadUserProfile(profileName);

        if (!String.IsNullOrEmpty(settings.Space))
            userProfile.Space = settings.Space;
        
        if (!String.IsNullOrEmpty(settings.Token))
            userProfile.Token = settings.Token;

        return userProfile;
    }
}