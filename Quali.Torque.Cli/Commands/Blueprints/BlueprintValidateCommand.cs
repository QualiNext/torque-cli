using System.Text;
using Quali.Torque.Cli.Models.Settings.Blueprints;
using Spectre.Console.Cli;
using Torque.Cli.Api;

namespace Quali.Torque.Cli.Commands.Blueprints;

public class BlueprintValidateCommand : TorqueBaseCommand<BlueprintValidateCommandSettings>
{
    public BlueprintValidateCommand(IClientManager clientManager,
        IConsoleManager consoleManager) : base(clientManager, consoleManager) { }

    public override async Task<int> ExecuteAsync(CommandContext context, BlueprintValidateCommandSettings settings)
    {
        try
        {
            var user = _clientManager.FetchUserProfile(settings);
            var torqueClient = _clientManager.GetClient(user);

            var fileContent = await File.ReadAllTextAsync(settings.BlueprintFile);
            var encodedBlueprintContent = Convert.ToBase64String(Encoding.UTF8.GetBytes(fileContent));

            var result = await torqueClient.BlueprintValidateAsync(user.Space, new BlueprintValidationRequest()
            {
                Blueprint_name = null, // not needed
                Blueprint_raw_64 = encodedBlueprintContent
            });
            if (settings.Detail)
                _consoleManager.DumpJson(result);
            else
                _consoleManager.WriteBlueprintsErrors(result);

            if (result.Errors.Count > 0)
                return 1;
        }
        catch (Exception e)
        {
            _consoleManager.WriteError(e);
            return 1;
        }

        return 0;
    }
}