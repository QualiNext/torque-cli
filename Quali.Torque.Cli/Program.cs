using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Quali.Torque.Cli.Commands.Blueprints;
using Quali.Torque.Cli.Commands.Configure;
using Quali.Torque.Cli.Commands.Environments;
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

        services.AddSingleton<IConsoleManager, SpectreConsoleManager>(); 
        services.AddSingleton<IClientManager, ClientManager>();

        var registrar = new TypeRegistrar(services);
        var app = new CommandApp(registrar);
        
        app.Configure(config =>
        {
            config.SetApplicationName("torque");
            config.ValidateExamples();
            config.AddBranch("configure", configure =>
            {
                configure.SetDescription("List, Add and Modify user profiles");
                configure.AddCommand<ConfigureListCommand>("list")
                    .WithDescription("List all profiles")
                    .WithExample(new[] {"configure", "list"});
                configure.AddCommand<ConfigureSetCommand>("set")
                    .WithDescription("Add or update torque user profile");
            });
            config.AddBranch("bp", blueprint =>
            {
                blueprint.SetDescription("Get, List, Validate blueprints.");
                
                blueprint.AddCommand<BlueprintGetCommand>("get")
                    .WithDescription("Get blueprint by Name.")
                    .WithExample(new [] { "bp", "get", "MyBp"});

                blueprint.AddCommand<BlueprintListCommand>("list")
                    .WithDescription("List blueprints")
                    .WithExample(new [] { "bp", "list" });

                blueprint.AddCommand<BlueprintValidateCommand>("validate")
                    .WithDescription("Validate blueprint");
            });
            config.AddBranch("env", environment =>
            {
                environment.SetDescription("Start, End, View Torque environments.");
                environment.AddCommand<EnvironmentStartCommand>("start")
                    .WithDescription("Start Environment")
                    .WithExample(new[] {"env", "start", "demo", "--duration=100", "--name=MyDemoEnv"});
                environment.AddCommand<EnvironmentGetCommand>("get")
                    .WithDescription("Get Environment Details")
                    .WithExample(new[] {"env", "get"});
                environment.AddCommand<EnvironmentEndCommand>("end")
                    .WithDescription("End Torque Environment")
                    .WithExample(new []{"env", "end", "qwdj4jr9smf"});
                environment.AddCommand<EnvironmentListCommand>("list")
                    .WithDescription("List Torque Environment")
                    .WithExample(new []{"env", "list", "--show-ended"});
                environment.AddCommand<EnvironmentExtendCommand>("extend")
                    .WithDescription("Extend Torque Environment")
                    .WithExample(new []{"env", "extend", "qwdj4jr9smf", "--duration", "120"});
            });
        });
 
        return app.Run(args);
    }
}