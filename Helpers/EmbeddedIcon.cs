using System;
using System.IO;
using System.Reflection;

namespace BizCardApp.Helpers;

public static class EmbeddedIcon
{
    public static string EnsureExtracted(string resourceName, string fileName)
    {
        var cacheDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "BizCardApp",
            "Cache");

        Directory.CreateDirectory(cacheDir);

        var iconPath = Path.Combine(cacheDir, fileName);

        if (File.Exists(iconPath) && new FileInfo(iconPath).Length > 0)
            return iconPath;

        var asm = Assembly.GetExecutingAssembly();
        using var stream = asm.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"Missing embedded resource: {resourceName}");

        using var file = File.Create(iconPath);
        stream.CopyTo(file);

        return iconPath;
    }
}