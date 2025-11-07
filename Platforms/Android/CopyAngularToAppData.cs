using Android.Content.Res;

namespace PomodoroMaui.Platforms.Android;

public static class CopyAngularToAppData
{
    const string AssetFolder = "browser";

    public static void Copy(AssetManager assets)
    {
        CopyAssetsFolder(assets, "browser", Path.Combine(FileSystem.AppDataDirectory, "browser"));
        Console.WriteLine($"Copied assets from: path into {FileSystem.AppDataDirectory}");
    }
    
    private static void CopyAssetsFolder(AssetManager assets, string assetFolderPath, string destFolderPath)
    {
        Directory.CreateDirectory(destFolderPath);

        string[] assetsList = assets.List(assetFolderPath) ?? [];

        foreach(var asset in assetsList)
        {
            var assetPath = Path.Combine(assetFolderPath, asset);
            var destPath = Path.Combine(destFolderPath, asset);
            var subFiles = assets.List(assetPath);
            if (subFiles != null && subFiles.Length > 0)
            {
                CopyAssetsFolder(assets, assetPath, destPath);
            }
            else
            {
                using var assetStream = assets.Open(assetPath);
                using var destStream = File.Create(destPath);
                assetStream.CopyTo(destStream);
            }
        }
    }
}