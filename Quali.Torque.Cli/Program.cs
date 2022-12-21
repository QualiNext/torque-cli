using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Quali.Torque.Cli.Commands.Blueprints;
using Spectre.Console.Cli;
using Torque.Cli.Api;

namespace Quali.Torque.Cli;

public class Program
{
    public static int Main(string[] args)
    {
        var services = new ServiceCollection();

        services.AddHttpClient<TorqueApiClient>(configure =>
        {
            var userAgentHeader = Environment.GetEnvironmentVariable("TORQUE_USERAGENT") ?? "Torque-CLI/1.0.0"; 
            configure.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(userAgentHeader));
        });
        // services.AddHttpClient(httpClient =>
        // {
        //     // TODO: version shouldn't be hardcoded
        //     var userAgentHeader = Environment.GetEnvironmentVariable("TORQUE_USERAGENT") ?? "Torque-CLI/1.0.0"; 
        //     httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(userAgentHeader));
        // });
        
        services.AddSingleton<IUserProfilesManager, UserProfilesManager>();
        services.AddSingleton<IEnvironmentProvider, EnvironmentProvider>();

        services.AddSingleton<IConsoleWriter, ConsoleWriter>(); 
        services.AddSingleton<IClientManager, ClientManager>();

        var registrar = new TypeRegistrar(services);
        var app = new CommandApp(registrar);
        
        app.Configure(config =>
        {
            config.SetApplicationName("torque");
            config.ValidateExamples();
    
            config.AddBranch("bp", blueprint =>
            {
                blueprint.SetDescription("Get, List, Validate blueprints.");
                
                blueprint.AddCommand<BlueprintGetCommand>("get")
                    .WithDescription("Get blueprint by Name.")
                    .WithExample(new [] { "bp", "get", "MyBp", "demo"});

                blueprint.AddCommand<BlueprintListCommand>("list")
                    .WithDescription("List blueprints")
                    .WithExample(new [] { "bp", "list" });
            });
        });
 
        return app.Run(args);
    }
}