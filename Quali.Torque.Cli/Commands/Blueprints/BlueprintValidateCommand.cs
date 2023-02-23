using System.Text;
using Quali.Torque.Cli.Models.Settings.Blueprints;
using Torque.Cli.Api;

namespace Quali.Torque.Cli.Commands.Blueprints;

public class BlueprintValidateCommand : TorqueMemberScopedCommand<BlueprintValidateUserContextSettings>
{
    public BlueprintValidateCommand(IClientManager clientManager,
        IConsoleManager consoleManager) : base(clientManager, consoleManager)
    {
    }

    protected override async Task RunTorqueCommandAsync(BlueprintValidateUserContextSettings settings)
    {
        var fileContent = await File.ReadAllTextAsync(settings.BlueprintFile);
        var encodedBlueprintContent = Convert.ToBase64String(Encoding.UTF8.GetBytes(fileContent));

        var result = await Client.BlueprintValidateAsync(User.Space, new BlueprintValidationRequest()
        {
            Blueprint_name = null, // not needed
            Blueprint_raw_64 = encodedBlueprintContent
        });
        if (settings.Detail)
            ConsoleManager.DumpJson(result);
        else
            ConsoleManager.WriteBlueprintsErrors(result);

        if (result.Errors.Count > 0)
            throw new Exception("Blueprint in not valid!");
    }
}