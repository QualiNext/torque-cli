using Quali.Torque.Cli.Models;
using Quali.Torque.Cli.Models.Settings.Environments;
using Spectre.Console.Cli;
using Torque.Cli.Api;

namespace Quali.Torque.Cli.Commands.Environments;

public class EnvironmentStartCommand : TorqueBaseCommand<EnvironmentStartCommandSettings>
{
    public EnvironmentStartCommand(IClientManager clientManager, IConsoleManager consoleManager) : base(clientManager,
        consoleManager) { }

    private static string GenerateEnvironmentName(string blueprintName)
    {
        var suffix = DateTime.Now.ToString("g");
        return $@"{blueprintName}-{suffix}";
    }

    public override async Task<int> ExecuteAsync(CommandContext context, EnvironmentStartCommandSettings settings)
    {
        var user = _clientManager.FetchUserProfile(settings);
        var torqueClient = _clientManager.GetClient(user);

        var createEnvRequest = new CreateSandboxRequest()
        {
            Blueprint_name = settings.BlueprintName,
            Inputs = settings.Inputs,
            Repository_name = settings.RepositoryName,
            Environment_name = settings.Name ?? GenerateEnvironmentName(settings.BlueprintName),
            Duration = $"PT{settings.Duration}M"
        };

        try
        {
            var envResponse = await torqueClient.EnvironmentsPOSTAsync(user.Space, createEnvRequest);

            if (settings.WaitActive)
            {
                await _consoleManager.WaitEnvironment(new EnvironmentWaiterData()
                {
                    Timeout = settings.Timeout,
                    Space = user.Space,
                    Client = torqueClient,
                    EnvironmentId = envResponse.Id
                });
            }

            if (settings.Detail)
                _consoleManager.DumpJson(envResponse);
            else
                _consoleManager.WriteEnvironmentCreated(envResponse.Id,
                    $"{torqueClient.BaseUrl}/{user.Space}/sandboxes/{envResponse.Id}/devops");
            
        }
        catch (Exception ex)
        {
            _consoleManager.WriteError(ex);
            return 1;
        }

        return 0;
    }
}