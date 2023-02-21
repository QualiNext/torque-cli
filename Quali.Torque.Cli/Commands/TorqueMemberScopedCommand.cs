using Quali.Torque.Cli.Models.Settings.Base;

namespace Quali.Torque.Cli.Commands;

public abstract class TorqueMemberScopedCommand<T>: TorqueBaseCommand<T>  where T : UserContextSettings
{
    protected TorqueMemberScopedCommand(IClientManager clientManager, IConsoleManager consoleManager) : base(clientManager, consoleManager)
    {
    }
    protected override void SetupClient(T settings)
    {
        User = ClientManager.FetchUserProfile(settings);
        Client = ClientManager.GetClient(User);
    }
}