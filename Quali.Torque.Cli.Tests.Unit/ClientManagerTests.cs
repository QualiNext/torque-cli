using AutoFixture;
using AutoFixture.AutoFakeItEasy;
using FakeItEasy;
using FluentAssertions;
using Quali.Torque.Cli.Models;
using Quali.Torque.Cli.Models.Settings.Base;

namespace Quali.Torque.Cli.Tests.Unit;

public class ClientManagerTests
{
    private readonly IFixture _fixture = new Fixture().Customize(new AutoFakeItEasyCustomization { ConfigureMembers = true });

    #region FetchUserProfile Tests

    [Theory]
    [InlineData("fileToken", "environmentVariable", "settingsToken", "settingsToken")]
    [InlineData("fileToken", "environmentVariable", null, "environmentVariable")]
    [InlineData("fileToken", null, null, "fileToken")]
    [InlineData(null, null, null, null)]
    public void FetchUserProfile_TokenIsValid(string fileToken, string envVariable, string settingsToken, string expected)
    {
        //Arrange
        var envProvider = _fixture.Freeze<IEnvironmentProvider>(); 
        var userProfileManager = _fixture.Freeze<IUserProfilesManager>(); 
        
        var clientManager = _fixture.Create<ClientManager>();

        A.CallTo(() => envProvider.GetEnvironmentVariable(EnvironmentVariables.Token))
            .Returns(envVariable); 
        
        var userProfile = _fixture.Build<UserProfile>()
            .With(x => x.Token, fileToken)
            .Create();

        A.CallTo(() => userProfileManager.ReadActiveUserProfile())
            .Returns(userProfile); 
        
        var setting = _fixture.Build<UserContextSettings>()
            .With(x => x.Token , settingsToken)
            .Create(); 

        //Act
        var result = clientManager.FetchUserProfile(setting); 

        //Assert
        result.Token.Should().Be(expected); 
    }

    [Theory]
    [InlineData("fileSpace", "environmentVariable", "settingsSpace", "settingsSpace")]
    [InlineData("fileSpace", "environmentVariable", null, "environmentVariable")]
    [InlineData("fileSpace", null, null, "fileSpace")]
    [InlineData(null, null, null, null)]
    public void FetchUserProfile_SpaceIsValid(string fileSpace, string envVariable, string settingsSpace, string expected)
    {
        //Arrange
        var envProvider = _fixture.Freeze<IEnvironmentProvider>(); 
        var userProfileManager = _fixture.Freeze<IUserProfilesManager>(); 
        
        var clientManager = _fixture.Create<ClientManager>();

        A.CallTo(() => envProvider.GetEnvironmentVariable(EnvironmentVariables.Space))
            .Returns(envVariable); 
        
        var userProfile = _fixture.Build<UserProfile>()
            .With(x => x.Space, fileSpace)
            .Create();

        A.CallTo(() => userProfileManager.ReadActiveUserProfile())
            .Returns(userProfile); 
        
        var setting = _fixture.Build<UserContextSettings>()
            .With(x => x.Space , settingsSpace)
            .Create(); 

        //Act
        var result = clientManager.FetchUserProfile(setting); 

        //Assert
        result.Space.Should().Be(expected); 
    }
    
    [Theory]
    [InlineData("fileRepoName", "environmentVariable", "settingsRepoName", "settingsRepoName")]
    [InlineData("fileRepoName", "environmentVariable", null, "environmentVariable")]
    [InlineData("fileRepoName", null, null, "fileRepoName")]
    [InlineData(null, null, null, null)]
    public void FetchUserProfile_RepoNameIsValid(string fileRepoName, string envVariable, string settingsRepoName, string expected)
    {
        //Arrange
        var envProvider = _fixture.Freeze<IEnvironmentProvider>(); 
        var userProfileManager = _fixture.Freeze<IUserProfilesManager>(); 
        
        var clientManager = _fixture.Create<ClientManager>();

        A.CallTo(() => envProvider.GetEnvironmentVariable(EnvironmentVariables.RepoName))
            .Returns(envVariable); 
        
        var userProfile = _fixture.Build<UserProfile>()
            .With(x => x.RepositoryName, fileRepoName)
            .Create();

        A.CallTo(() => userProfileManager.ReadActiveUserProfile())
            .Returns(userProfile); 
        
        var setting = _fixture.Build<UserContextSettings>()
            .With(x => x.RepositoryName , settingsRepoName)
            .Create(); 

        //Act
        var result = clientManager.FetchUserProfile(setting); 

        //Assert
        result.RepositoryName.Should().Be(expected); 
    }

    #endregion
}