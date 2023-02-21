using Quali.Torque.Cli.Models;
using Quali.Torque.Cli.Models.Settings.Environments;
using Torque.Cli.Api;

namespace Quali.Torque.Cli.Commands.Environments;

public class EnvironmentStartCommand : TorqueMemberScopedCommand<EnvironmentStartUserContextSettings>
{
    public EnvironmentStartCommand(IClientManager clientManager, IConsoleManager consoleManager) : base(clientManager,
        consoleManager) { }

    private static string GenerateEnvironmentName(string blueprintName)
    {
        var suffix = DateTime.Now.ToString("g");
        return $@"{blueprintName}-{suffix}";
    }

    protected override async Task RunTorqueCommandAsync(EnvironmentStartUserContextSettings settings)
    {
        var createEnvRequest = new CreateSandboxRequest
        {
            Inputs = settings.Inputs,
            Environment_name = settings.Name ?? GenerateEnvironmentName(settings.BlueprintName),
            Duration = $"PT{settings.Duration}M",
            Source = new BlueprintSourceRequest
            {
                Blueprint_name = settings.BlueprintName,
                Repository_name = settings.RepositoryName,
                Branch = settings.Branch,
                Commit = settings.CommitId
            }
        };
        
        var envResponse = await Client.EnvironmentsPOSTAsync(User.Space, createEnvRequest);

        if (settings.WaitActive)
        {
            await ConsoleManager.WaitEnvironment(new EnvironmentWaiterData
            {
                Timeout = settings.Timeout,
                Space = User.Space,
                Client = Client,
                EnvironmentId = envResponse.Id
            });
        }

        if (settings.Detail)
            ConsoleManager.DumpJson(envResponse);
        else
            ConsoleManager.WriteEnvironmentCreated(envResponse.Id,
                $"{Client.BaseUrl}/{User.Space}/sandboxes/{envResponse.Id}/devops");
    }
}