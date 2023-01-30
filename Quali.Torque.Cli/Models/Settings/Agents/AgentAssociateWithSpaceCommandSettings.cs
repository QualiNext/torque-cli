using System.ComponentModel;
using Quali.Torque.Cli.Models.Settings.Base;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Quali.Torque.Cli.Models.Settings.Agents;

public class AgentAssociateWithSpaceCommandSettings: BaseSettings
{
    private static string[] _types = { "k8s", "aks" };

    [CommandArgument(0, "<AGENT-NAME>")]
    [Description("The name of agent to associate with a space")]
    public string AgentName { get; set; }
    
    [CommandArgument(1, "<SPACE-NAME>")]
    [Description("The space name.")]
    public string SpaceName { get; set; }
    
    [CommandOption("--type <TYPE>")]
    [Description("The type of agent. Must be 'K8S' or 'AKS'")]
    [DefaultValue("K8S")]
    public string Type { get; set; }

    [CommandOption("--ns <VALUE>")]
    [Description("The default namespace where K8s resources would be deployed if no other namespace is specified in the blueprint")]
    public string DefaultNamespace { get; set; }
    
    [CommandOption("--sa <VALUE>")]
    [Description("The default permissions to launch environments in this space.")]
    public string DefaultServiceAccount { get; set; }
    
    public override ValidationResult Validate()
    {
        if (!_types.Contains(Type))
            return ValidationResult.Error($"--type must be one of: {string.Join(", ", _types)} ");

        return ValidationResult.Success();
    }
}