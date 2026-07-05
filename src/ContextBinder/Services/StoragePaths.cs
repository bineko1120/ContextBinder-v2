namespace ContextBinder.Services;

public sealed record StoragePaths(
    string DataDirectory,
    string StoreFilePath,
    string SettingsFilePath,
    string BackupDirectory,
    string TrashDirectory);
