using Spectre.Console;
using Spectre.Console.Rendering;

namespace Quali.Torque.Cli.Utils;

public static class SpectreConsoleTorqueExtensions
{
    public static void AddGridDetailsRow(this Grid grid, string key, IRenderable value)
    {
        grid.AddRow(new Text(key, new Style(Color.LightGreen)), value);
    }

    public static string MaskToken(this string tokenString)
    {
        if (string.IsNullOrEmpty(tokenString))
            return "";

        return tokenString.Length > 3 ? $"******{tokenString[^4..]}" : "******";
    }
}
