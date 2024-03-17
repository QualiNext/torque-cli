using Quali.Torque.Cli.Models.Settings.Environments;

namespace Quali.Torque.Cli.Commands.Environments;

public class EnvironmentBulkStartCommand : TorqueMemberScopedCommand<EnvironmentBulkStartUserContextSettings>
{
    public EnvironmentBulkStartCommand(IClientManager clientManager, IConsoleManager consoleManager) : base(clientManager,
        consoleManager)
    {
    }

    protected override async Task RunTorqueCommandAsync(EnvironmentBulkStartUserContextSettings settings)
    {
        string[] lines = await File.ReadAllLinesAsync(settings.CsvPath);

        int lineNumber = 1;
        foreach (string line in lines.Skip(1)) // Skip the header line
        {
            lineNumber++;
            string[] values = line.Split(',');
            if (values.Length != 6)
            {
                ConsoleManager.WriteError($"Wrong number of columns found in line {lineNumber} of the CSV. skipping line.");
                continue;
            }

            string space = values[0];
            string blueprint = values[1];
            string repository = values[2];
            int duration = int.Parse(values[3]);
            string[] owners = values[4].Split(';');
            IDictionary<string, string> inputs;
            try
            {
                inputs = values[5].Split(';').Select(pair => pair.Split(':')).ToDictionary(pair => pair[0], pair => pair[1]);
            }
            catch (Exception e)
            {
                ConsoleManager.WriteError($"Error parsing inputs on line {lineNumber}. Unparsed value was \"{values[5]}\", expected input in the format name:value;name:value. Skipping line.");
                continue;
            }
            
            foreach (var owner in owners) // this behavior (env per owner) is copied from the older python bulk deployer: https://github.com/QualiSystemsLab/Torque-Bulk-Deployer/blob/master/torque_bulk_deployer/bulk_deployer.py
            {
                string environmentName = $"{blueprint} - {owner}";
                await EnvironmentStartCommand.StartEnvironment(ConsoleManager, Client, space, repository, null, null, blueprint, inputs, environmentName, duration, owner, false, 0, settings.Detail);
            }
        }
    }
}