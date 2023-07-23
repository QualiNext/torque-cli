using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Quali.Torque.Cli.Commands.Agents;
using Quali.Torque.Cli.Commands.Blueprints;
using Quali.Torque.Cli.Commands.Config;
using Quali.Torque.Cli.Commands.Environments;
using Quali.Torque.Cli.Commands.Spaces;
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
            ProductInfoHeaderValue productValue;
            var userAgentHeader = Environment.GetEnvironmentVariable(EnvironmentVariables.UserAgent);

            if (string.IsNullOrEmpty(userAgentHeader))
            {
                productValue = new ProductInfoHeaderValue(Constants.DefaultUserAgentValue, UserAgentUtils.GetCurrentVersion());
            }
            else
            {
                try
                {
                    var valueParts = UserAgentUtils.ParseCustomUserAgent(userAgentHeader);
                    productValue = new ProductInfoHeaderValue(valueParts[0], valueParts[1]);
                }
                catch (Exception)
                {
                    // TODO: log error once logging is ready
                    productValue = new ProductInfoHeaderValue(Constants.DefaultUserAgentValue, UserAgentUtils.GetCurrentVersion());
                }   
            }
            
            configure.DefaultRequestHeaders.UserAgent.Add(productValue);
        });
        services.AddSingleton<IUserProfilesManager, UserProfilesManager>();
        services.AddSingleton<IEnvironmentProvider, EnvironmentProvider>();
        services.AddSingleton<ITorqueConfigurationProvider, TorqueYamlConfigurationProvider>();

        services.AddSingleton<IConsoleManager, SpectreConsoleManager>(); 
        services.AddSingleton<IClientManager, ClientManager>();

        var registrar = new TypeRegistrar(services);
        var app = new CommandApp(registrar);
        
        app.Configure(config =>
        {
            config.SetApplicationName("torque");
            config.ValidateExamples();
           
            config.AddBranch("blueprint", blueprint =>
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
                
                blueprint.AddCommand<BlueprintPublishCommand>("publish")
                    .WithDescription("Publish blueprint to catalog");
                
                blueprint.AddCommand<BlueprintUnpublishCommand>("unpublish")
                    .WithDescription("Remove blueprint from catalog");
            }).WithAlias("bp");
            
            config.AddBranch("environment", environment =>
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
            }).WithAlias("env");
            
            config.AddBranch("config", configure =>
            {
                configure.SetDescription("List, Add and Modify user profiles");
                configure.AddCommand<ConfigListCommand>("list")
                    .WithDescription("List all profiles")
                    .WithExample(new[] {"config", "list"});
                configure.AddCommand<ConfigSetCommand>("set")
                    .WithDescription("Add or update torque user profile");
                configure.AddCommand<ConfigRemoveCommand>("remove")
                    .WithDescription("Remove specified profile")
                    .WithExample(new[] {"config", "remove", "myprofile"});
                configure.AddCommand<ConfigSetActiveCommand>("activate")
                    .WithDescription("Set active profile")
                    .WithExample(new[] {"config", "activate", "myprofile"});
            });

            config.AddBranch("agent", agent =>
            {
                agent.SetDescription("List, associate agents");

                agent.AddCommand<AgentsListCommand>("list")
                    .WithDescription("List agents in Space.")
                    .WithExample(new[] {"agent", "list", "mySpace"});

                agent.AddCommand<AgentAssociateWithSpaceCommand>("associate")
                    .WithDescription("Associate Agent with Space")
                    .WithExample(new[] {"agent", "associate", "myAgent", "mySpace", "--ns", "demo", "--sa", "mySA"});
            });

            config.AddBranch("space", space =>
            {
                space.SetDescription("Create, delete spaces, connect repo to space.");

                space.AddCommand<SpaceCreateCommand>("create")
                    .WithDescription("Create space")
                    .WithExample(new[] {"space", "create", "demo"});

                space.AddCommand<SpaceDeleteCommand>("delete")
                    .WithDescription("Delete space")
                    .WithExample(new[] {"space", "delete", "demo"});

                space.AddCommand<SpaceListCommand>("list")
                    .WithDescription("Show list of spaces")
                    .WithExample(new[] {"space", "list"});

                // space.AddCommand<SpaceAddRepoCommand>("connect")
                //     .WithDescription("Connect repo to space")
                //     .WithExample(new[] {"space", "connect", "myRepo"});
            });
        });
 
        return app.Run(args);
    }
}