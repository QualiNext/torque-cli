using Quali.Torque.Cli.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Quali.Torque.Cli;

public interface ITorqueConfigurationProvider
{
    void SaveConfiguration(TorqueConfiguration config, string filePath);
    TorqueConfiguration LoadConfiguration(string filePath);
}

public class TorqueYamlConfigurationProvider : ITorqueConfigurationProvider
{
    private readonly IDeserializer _deserializer;
    private readonly ISerializer _serializer;

    public TorqueYamlConfigurationProvider()
    {
        _serializer = new SerializerBuilder()
            .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull)
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        _deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
    }

    public void SaveConfiguration(TorqueConfiguration config, string filePath)
    {
        var parentDirectory = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(parentDirectory))
        {
            Directory.CreateDirectory(parentDirectory);
        }

        using (var writer = new StreamWriter(filePath))
        {
            var stringResult = _serializer.Serialize(config);
            writer.Write(stringResult);
        }
    }

    public TorqueConfiguration LoadConfiguration(string filePath)
    {
        var config = _deserializer.Deserialize<TorqueConfiguration>(File.ReadAllText(filePath));
        return config;
    }
}