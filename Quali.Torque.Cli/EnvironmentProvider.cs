namespace Quali.Torque.Cli;

public interface IEnvironmentProvider
{
    string GetEnvironmentVariable(string name); 
}

public sealed class EnvironmentProvider : IEnvironmentProvider
{
    public string GetEnvironmentVariable(string name)
    {
        return Environment.GetEnvironmentVariable(name); 
    }
}