using Quali.Torque.Cli.Models.Settings.Environments;

namespace Quali.Torque.Cli.Commands.Environments;

public class EnvironmentExtendCommand: TorqueMemberScopedCommand<EnvironmentExtendCommandSettings>
{
    public EnvironmentExtendCommand(IClientManager clientManager, IConsoleManager consoleManager) : base(clientManager,
        consoleManager) { }

    protected override async Task RunTorqueCommandAsync(EnvironmentExtendCommandSettings settings)
    {
        var envDetails = await Client.EnvironmentsGETAsync(User.Space, settings.EnvironmentId);
        var endTime = envDetails.Details.State.Execution.End_time
                      ?? envDetails.Details.State.Execution.Retention.Time ?? DateTimeOffset.Now;
        var duration = TimeSpan.FromMinutes(settings.Duration);
        await Client.ExtendEnvironment(User.Space, settings.EnvironmentId, endTime.Add(duration));
        ConsoleManager.WriteSuccessMessage($"Environment {settings.EnvironmentId} has been extended successfully");
    }
}