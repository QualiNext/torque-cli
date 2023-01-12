using Spectre.Console;
using Spectre.Console.Rendering;

namespace Quali.Torque.Cli.Utils;

public static class SpectreConsoleTorqueExtensions
{
    public static void AddGridDetailsRow(this Grid grid, string key, IRenderable value)
    {
        grid.AddRow(new Text(key, new Style(Color.LightGreen)), value);
    }
}
