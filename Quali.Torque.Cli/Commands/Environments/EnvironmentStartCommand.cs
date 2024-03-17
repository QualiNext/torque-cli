using Quali.Torque.Cli.Models;
using Quali.Torque.Cli.Models.Settings.Environments;
using Torque.Cli.Api;

namespace Quali.Torque.Cli.Commands.Environments;

public class EnvironmentStartCommand : TorqueMemberScopedCommand<EnvironmentStartUserContextSettings>
{
    public EnvironmentStartCommand(IClientManager clientManager, IConsoleManager consoleManager) : base(clientManager,
        consoleManager)
    {
    }

    private static string GenerateEnvironmentName(string blueprintName)
    {
        var suffix = DateTime.Now.ToString("g");
        return $@"{blueprintName}-{suffix}";
    }

    protected override async Task RunTorqueCommandAsync(EnvironmentStartUserContextSettings settings)
    {
        string environmentName = string.IsNullOrWhiteSpace(settings.Name)
            ? GenerateEnvironmentName(settings.BlueprintName)
            : settings.Name;
        await StartEnvironment(ConsoleManager, Client, User.Space, settings.RepositoryName, settings.Branch,
            settings.CommitId, settings.BlueprintName, settings.Inputs, environmentName, settings.Duration, 
            null, settings.WaitActive, settings.Timeout, settings.Detail);
    }

    public static async Task StartEnvironment(IConsoleManager consoleManager, TorqueApiClient client, string space,
        string repository, string branch, string commitId, string blueprintName, IDictionary<string, string> inputs,
        string environmentName, int durationMinutes, string owner, bool waitActive, int timeout, bool detail)
    {
        ICollection<GetWorkflowLaunchDetailsResponse> workflowLaunchDetails = await client.DetailsAsync(space);
        var workflowRequest = WorkflowLaunchDetailsToWorkflowRequest(workflowLaunchDetails);
        var createEnvRequest = new CreateSandboxRequest
        {
            Owner_email = owner,
            Inputs = inputs,
            Environment_name = environmentName,
            Duration = $"PT{durationMinutes}M",
            Blueprint_name = blueprintName,
            Source = new BlueprintSourceRequest
            {
                Repository_name = repository,
                Branch = branch,
                Commit = commitId
            },
            Workflows = workflowRequest
        };

        var envResponse = await client.EnvironmentsPOSTAsync(space, createEnvRequest);

        if (waitActive)
        {
            await consoleManager.WaitEnvironment(new EnvironmentWaiterData
            {
                Timeout = timeout,
                Space = space,
                Client = client,
                EnvironmentId = envResponse.Id
            });
        }

        if (detail)
            consoleManager.DumpJson(envResponse);
        else
            consoleManager.WriteEnvironmentCreated(envResponse.Id,
                $"{client.BaseUrl}/{space}/sandboxes/{envResponse.Id}/devops");
    }

    private static ICollection<LaunchWorkflowRequest> WorkflowLaunchDetailsToWorkflowRequest(
        ICollection<GetWorkflowLaunchDetailsResponse> workflowsDetails)
    {
        return workflowsDetails.Select(workflowLaunchDetails => new LaunchWorkflowRequest
        {
            Name = workflowLaunchDetails.Workflow_name,
            Schedules = workflowLaunchDetails.Scheduler.Cron_expressions
                .Select(s => new LaunchScheduleRequest { Overridden = false, Scheduler = s })
                .ToList()
        }).ToList();
    }
}