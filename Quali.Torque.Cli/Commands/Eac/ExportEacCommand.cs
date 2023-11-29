using System.Net.Mime;
using Quali.Torque.Cli.Models.Settings.Eac;
using Torque.Cli.Api;

namespace Quali.Torque.Cli.Commands.Eac;

public class ExportEacCommand : TorqueMemberScopedCommand<ExportEacCommandSettings>
{
    public ExportEacCommand(IClientManager clientManager, IConsoleManager consoleManager) : base(clientManager, consoleManager)
    {
    }

    protected override async Task RunTorqueCommandAsync(ExportEacCommandSettings settings)
    {
        FileResponse fileResponse = await Client.EacAsync(User.Space, settings.EnvironmentId);
        
        if (string.IsNullOrWhiteSpace(settings.FileName))
        {
            var contentDisposition = fileResponse.Headers["Content-Disposition"].First();
            settings.FileName = new ContentDisposition(contentDisposition).FileName;
        }

        await using (FileStream fileStream = File.Create(settings.FileName ?? throw new ArgumentException("filename")))
        {
            await fileResponse.Stream.CopyToAsync(fileStream);
        }
        
        ConsoleManager.WriteInfo($"Environment file downloaded: {settings.FileName}");
    }
}