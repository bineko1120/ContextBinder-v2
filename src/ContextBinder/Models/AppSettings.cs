namespace ContextBinder.Models;

public sealed class AppSettings
{
    public int SchemaVersion { get; set; } = 1;

    public bool FirstRunCompleted { get; set; }

    public StorageMode StorageMode { get; set; } = StorageMode.Standard;

    public string CustomStorageDirectory { get; set; } = string.Empty;

    public TypeDisplayMode TypeDisplayMode { get; set; } = TypeDisplayMode.IconAndText;

    public SearchScope SearchScope { get; set; } = SearchScope.CurrentGroup;

    public bool ShowBeginnerHints { get; set; } = true;

    public bool ShowIconLegend { get; set; } = true;

    public bool ConfirmDuplicateOnDrop { get; set; } = false;

    public bool ConfirmTitleOnDropAdd { get; set; } = false;

    public bool FocusExistingItemOnDuplicate { get; set; } = true;

    public bool EnableGroupDropModifierShortcuts { get; set; } = false;

    public bool ConfirmGroupDropCopyMove { get; set; } = true;

    public bool EnableItemDragReorder { get; set; } = true;

    public bool EnableExternalFileDropOut { get; set; } = true;

    public bool EnableExternalUrlTextDragOut { get; set; } = true;

    public bool EnableExternalTemplateTextDragOut { get; set; } = true;

    public bool ShowOperationStatus { get; set; } = true;

    public bool MinimizeToTrayOnClose { get; set; } = true;

    public bool EnableContextMenuDetails { get; set; } = true;

    public bool AutoBackupEnabled { get; set; } = true;

    public int MaxBackupCount { get; set; } = 20;

    public bool ConfirmBeforeDelete { get; set; } = true;

    public bool MoveDeletedItemsToTrash { get; set; } = true;

    public bool SearchTemplateBody { get; set; } = true;
}
