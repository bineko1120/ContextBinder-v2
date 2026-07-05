using ContextBinder.Models;

namespace ContextBinder.Services;

public sealed class StorageLocationService
{
    public const string DataDirectoryName = "ContextBinder_Data";
    public const string StoreFileName = "contextbinder.store.json";
    public const string SettingsFileName = "settings.json";
    public const string StorageMarkerFileName = "contextbinder.storage.json";

    private readonly string[] _args;

    public StorageLocationService(string[]? args = null)
    {
        _args = args ?? [];
    }

    public StoragePaths ResolveStoragePaths(AppSettings? settings = null)
    {
        string dataDirectory = ResolveDataDirectory(settings);

        return new StoragePaths(
            dataDirectory,
            Path.Combine(dataDirectory, StoreFileName),
            Path.Combine(dataDirectory, SettingsFileName),
            Path.Combine(dataDirectory, "backups"),
            Path.Combine(dataDirectory, "trash"));
    }

    private string ResolveDataDirectory(AppSettings? settings)
    {
        string? commandLineDataDirectory = GetCommandLineDataDirectory();
        if (!string.IsNullOrWhiteSpace(commandLineDataDirectory))
        {
            return Path.GetFullPath(commandLineDataDirectory);
        }

        StorageMode storageMode = settings?.StorageMode ?? StorageMode.AppData;
        return storageMode switch
        {
            StorageMode.ApplicationFolder => Path.Combine(AppContext.BaseDirectory, DataDirectoryName),
            StorageMode.CustomFolder when !string.IsNullOrWhiteSpace(settings?.CustomStorageDirectory)
                => Path.GetFullPath(settings.CustomStorageDirectory),
            _ => Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "ContextBinder")
        };
    }

    private string? GetCommandLineDataDirectory()
    {
        for (int index = 0; index < _args.Length; index++)
        {
            if (string.Equals(_args[index], "--data-dir", StringComparison.OrdinalIgnoreCase)
                && index + 1 < _args.Length)
            {
                return _args[index + 1];
            }
        }

        return null;
    }
}
