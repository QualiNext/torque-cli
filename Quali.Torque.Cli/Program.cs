using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Quali.Torque.Cli.Commands.Blueprints;
using Quali.Torque.Cli.Utils;
using Spectre.Console.Cli;

namespace Quali.Torque.Cli;

public class Program
{
    public static int Main(string[] args)
    {
        var services = new ServiceCollection();

        services.AddHttpClient<HttpClient>("Default", configure =>
        {
            // var userAgentHeader = Environment.GetEnvironmentVariable(Constants.UserAgentEnvVarName) ?? "Torque-CLI/1.0.0"; 
            var version = PackageVersionExtractor.GetVersion();
            var productValue = new ProductInfoHeaderValue(Constants.UserAgentValue, version);
            configure.DefaultRequestHeaders.UserAgent.Add(productValue);
        });
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