using Torque.Cli.Commands;
using Torque.Cli.Commands.Blueprints;
// using Demo.Commands.Run;
// using Demo.Commands.Serve;
using Spectre.Console.Cli;

namespace Torque.Cli;

public static class Program
{
    public static int Main(string[] args)
    {
        var app = new CommandApp();
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