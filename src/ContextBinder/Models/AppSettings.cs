namespace ContextBinder.Models;

public sealed class AppSettings
{
    public int SchemaVersion { get; set; } = 1;

    public bool FirstRunCompleted { get; set; }

    public StorageMode StorageMode { get; set; } = StorageMode.Standard;

    public string CustomStorageDirectory { get; set; } = string.Empty;

    public TypeDisplayMode TypeDisplayMode { get; set; } = TypeDisplayMode.IconAndText;

    public SearchScope SearchScope { get; set; } = SearchScope.CurrentGroup;

    public bool ConfirmDuplicateOnDrop { get; set; } = false;

    public bool ShowOperationStatus { get; set; } = true;

    public bool MinimizeToTrayOnClose { get; set; } = true;

    public bool AutoBackupEnabled { get; set; } = true;

    public int MaxBackupCount { get; set; } = 20;
}
