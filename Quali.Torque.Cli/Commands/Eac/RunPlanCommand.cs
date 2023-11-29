using Quali.Torque.Cli.Models.Settings.Eac;
using Quali.Torque.Cli.Utils;
using Spectre.Console;
using Torque.Cli.Api;

namespace Quali.Torque.Cli.Commands.Eac;

public class RunPlanCommand : TorqueMemberScopedCommand<PlanCommandSettings>
{
    public RunPlanCommand(IClientManager clientManager, IConsoleManager consoleManager) : base(clientManager,
        consoleManager)
    { }

    protected override async Task RunTorqueCommandAsync(PlanCommandSettings settings)
    {
        if (!Console.IsInputRedirected)
        {
            ConsoleManager.WriteError(
                "This commands expects the contents of an environment yaml file to be piped in. \r\n e.g., cat env.yaml | torque plan <ENVIRONMENT-ID>");
            return;
        }
        
        IEnumerable<EacResponse> eacs = await Client.EacAsync(User.Space);
        if (EnvironmentHelper.TryGetEnvIdFromUrl(settings.EnvironmentId, eacs, out string environmentId))
            settings.EnvironmentId = environmentId;


        var envYaml = await Console.In.ReadToEndAsync();

        var createPlanRequest = new PlanEnvironmentRequest
        {
            Env_yaml_content = envYaml
        };

        var planEnvironmentResponse = await Client.PlanPOSTAsync(User.Space, settings.EnvironmentId, createPlanRequest);
        GetEnvironmentPlanResultResponse planResultResponse = null;

        var spinnerMsg = $"Running plan...";
        await AnsiConsole.Status()
            .SpinnerStyle(Style.Parse("green bold"))
            .StartAsync(spinnerMsg, async ctx =>
            {
                var startTime = DateTimeOffset.Now.ToUnixTimeSeconds();

                while (DateTimeOffset.Now.ToUnixTimeSeconds() - startTime < 300)
                {
                    await Task.Delay(5000);

                    planResultResponse = await Client.PlanGETAsync(User.Space, settings.EnvironmentId,
                        planEnvironmentResponse.Request_handle);

                    switch (planResultResponse.Status)
                    {
                        case "Done":
                            AnsiConsole.MarkupLine("[green bold]Plan completed successfully.[/]");
                            return true;
                        case "Failed":
                            AnsiConsole.MarkupLine("[red]Plan failed.[/]");
                            return false;
                        case "Cancelled":
                            AnsiConsole.MarkupLine("[red]Plan canceled.[/]");
                            return false;
                    }

                    ctx.Status($"{spinnerMsg} {DateTimeOffset.Now.ToUnixTimeSeconds() - startTime} sec");
                }

                throw new TimeoutException($"Plan timed out.");
            });

        ConsoleManager.WritePlan(planResultResponse, settings.GrainPath);
    }
}