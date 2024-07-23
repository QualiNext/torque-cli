using Newtonsoft.Json;
using Quali.Torque.Cli.Models;
using Spectre.Console.Cli;
using Torque.Cli.Api;

namespace Quali.Torque.Cli.Commands;

public abstract class TorqueBaseCommand<T> : AsyncCommand<T> where T : CommandSettings
{
    protected readonly IClientManager ClientManager;
    protected readonly IConsoleManager ConsoleManager;
    protected UserProfile User;
    protected TorqueApiClient Client;

    protected TorqueBaseCommand(IClientManager clientManager, IConsoleManager consoleManager)
    {
        ClientManager = clientManager;
        ConsoleManager = consoleManager;
    }

    protected abstract Task RunTorqueCommandAsync(T settings);

    protected abstract void SetupClient(T settings);

    public override async Task<int> ExecuteAsync(CommandContext context, T settings)
    {
        SetupClient(settings);
        try
        {
            await RunTorqueCommandAsync(settings);
            return 0;
        }
        catch (ApiException e)
        {
            ConsoleManager.WriteError("Error: unable to execute a command. Details:");
            if(!e.Message.StartsWith("Operation failed")) // Generic error when our swagger metadata doesn't specify the problem. 
                ConsoleManager.WriteError($"Error message: {e.Message}");
            ConsoleManager.WriteError($"Status code: {e.StatusCode}");

            if (e is ApiException<ErrorResponse> apiError)
            {
                ConsoleManager.WriteError($"Result:");
                foreach (var resultError in apiError.Result.Errors) 
                    ConsoleManager.WriteError($"  {resultError.Message}");
            }
            
            ConsoleManager.WriteError($"Base Url: {Client.BaseUrl}");
            ConsoleManager.WriteError($"Space: {User.Space}");
            
            var torqueResponse = JsonConvert.DeserializeObject<TorqueApiErrorResponse>(e.Response);
            if (torqueResponse != null)
                foreach (var error in torqueResponse.Errors)
                {
                    ConsoleManager.WriteError(error.Message);
                }
        }
        catch (Exception e)
        {
            ConsoleManager.WriteException(e);
        }

        return 1;
    }
}