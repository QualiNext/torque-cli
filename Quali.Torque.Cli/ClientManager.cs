using System.Net.Http.Headers;
using Quali.Torque.Cli.Models;
using Quali.Torque.Cli.Models.Settings.Base;
using Torque.Cli.Api;
using YamlDotNet.Core.Tokens;
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

    private string GetBaseUrl(UserContextSettings settings, string fileBaseUrl)
    {

        var baseUrl = Constants.DefaultTorqueUrl;

        var envBaseUrl = _environmentProvider.GetEnvironmentVariable(EnvironmentVariables.BaseUrl);

        baseUrl = string.IsNullOrWhiteSpace(fileBaseUrl) ? baseUrl : fileBaseUrl;

        if (settings != null && !string.IsNullOrEmpty(settings.BaseUrl))
        {
            baseUrl = settings.BaseUrl; 
        }
        
        if (!string.IsNullOrEmpty(envBaseUrl))
        {
            baseUrl = envBaseUrl; 
        }

        baseUrl = baseUrl.EndsWith('/') ? baseUrl : baseUrl + "/";

        return baseUrl;
    }

    private string GetRepositoryName(UserProfile fileProfile, UserContextSettings settings)
    {
        {
            var repositoryName = fileProfile?.RepositoryName;
            var envRepository = _environmentProvider.GetEnvironmentVariable(EnvironmentVariables.RepoName);
            
            if (!string.IsNullOrEmpty(envRepository))
            {
                repositoryName = envRepository; 
            }

            if (settings != null && !string.IsNullOrEmpty(settings.RepositoryName))
            {
                repositoryName = settings.RepositoryName; 
            }
        
            return repositoryName;
        }
    }

    private string GetSpace(UserProfile fileProfile, UserContextSettings settings)
    {
        var space = fileProfile?.Space;

        var envSpace = _environmentProvider.GetEnvironmentVariable(EnvironmentVariables.Space); 
        
        if (!string.IsNullOrEmpty(envSpace))
        {
            space = envSpace; 
        }

        if (settings != null && !string.IsNullOrEmpty(settings.Space))
        {
            space = settings.Space; 
        }
        
        return space;
    }
    private string GetToken(UserProfile fileProfile, UserContextSettings settings)
    {
        var token = fileProfile?.Token;

        var envToken = _environmentProvider.GetEnvironmentVariable(EnvironmentVariables.Token);
        if (!string.IsNullOrEmpty(envToken))
        {
            token = envToken; 
        }

        if (settings != null && !string.IsNullOrEmpty(settings.Token))
        {
            token = settings.Token; 
        }
        
        return token; 
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
        var fileProfile = _userProfilesManager.ReadActiveUserProfile();
        var userProfile = new UserProfile();

        userProfile.Token = GetToken(fileProfile, settings);
        userProfile.Space =  GetSpace(fileProfile, settings);
        userProfile.RepositoryName = GetRepositoryName(fileProfile, settings);
        
        userProfile.BaseUrl = GetBaseUrl(settings, fileProfile.BaseUrl); 

        return userProfile;
    }
}