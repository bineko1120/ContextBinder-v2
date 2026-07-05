namespace ContextBinder.Models;

public sealed class StorageLocation
{
    public StorageMode Mode { get; set; } = StorageMode.Standard;

    public string DataDirectory { get; set; } = string.Empty;

    public string StoreFilePath { get; set; } = string.Empty;

    public string SettingsFilePath { get; set; } = string.Empty;

    public string BackupDirectory { get; set; } = string.Empty;

    public string TrashDirectory { get; set; } = string.Empty;
}
