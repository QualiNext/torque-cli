using Quali.Torque.Cli.Models.Settings.Base;

namespace Quali.Torque.Cli.Commands;

public abstract class TorqueAdminScopedCommand<T>: TorqueBaseCommand<T>  where T : BaseSettings
{
    protected TorqueAdminScopedCommand(IClientManager clientManager, IConsoleManager consoleManager) : base(clientManager, consoleManager)
    {
    }
    
    protected override void SetupClient(T settings)
    {
        User = ClientManager.FetchUserProfile(UserContextSettings.ConvertToUserContextSettings(settings));
        Client = ClientManager.GetClient(User);
    }
}