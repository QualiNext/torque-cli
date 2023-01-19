using Quali.Torque.Cli.Models.Settings.Environments;
using Spectre.Console.Cli;

namespace Quali.Torque.Cli.Commands.Environments;

public class EnvironmentExtendCommand: TorqueBaseCommand<EnvironmentExtendCommandSettings>
{
    public EnvironmentExtendCommand(IClientManager clientManager, IConsoleManager consoleManager) : base(clientManager,
        consoleManager) { }

    public override async Task<int> ExecuteAsync(CommandContext context, EnvironmentExtendCommandSettings settings)
    {
        var user = _clientManager.FetchUserProfile(settings);
        var client = _clientManager.GetClient(user);
        
        try
        {
            var envDetails = await client.EnvironmentsGETAsync(user.Space, settings.EnvironmentId);
            var endTime = envDetails.Details.State.Execution.End_time
                ?? envDetails.Details.State.Execution.Retention.Time ?? DateTimeOffset.Now;
            var duration = TimeSpan.FromMinutes(settings.Duration);
            await _clientManager.GetClient(user)
                .ExtendEnvironment(user.Space, settings.EnvironmentId, endTime.Add(duration));
            _consoleManager.WriteSuccessMessage($"Environment {settings.EnvironmentId} has been extended successfully");
            return 0;
        }
        catch (Exception e)
        {
            _consoleManager.WriteError(e);
            return 1;
        }
    }
}