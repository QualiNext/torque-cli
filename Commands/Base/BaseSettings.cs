using System.ComponentModel;
using Spectre.Console.Cli;

namespace Torque.Cli.Commands.Base;

public abstract class BaseSettings : CommandSettings
{
    [CommandOption("-s|--space")]
    [Description("The space name")]
    public string? Space { get; set; }

    [CommandOption("-t|--token")]
    [Description("The user long token")]
    public string? Token { get; set; }
}