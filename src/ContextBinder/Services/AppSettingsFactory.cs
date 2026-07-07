using ContextBinder.Models;

namespace ContextBinder.Services;

public static class AppSettingsFactory
{
    public static AppSettings CreateRecommended()
    {
        return new AppSettings
        {
            FirstRunCompleted = false,
            TypeDisplayMode = TypeDisplayMode.IconAndText,
            SearchScope = SearchScope.CurrentGroup,
            ShowBeginnerHints = true,
            ShowIconLegend = true,
            ConfirmDuplicateOnDrop = false,
            ConfirmTitleOnDropAdd = false,
            FocusExistingItemOnDuplicate = true,
            EnableGroupDropModifierShortcuts = false,
            ConfirmGroupDropCopyMove = true,
            EnableItemDragReorder = true,
            EnableExternalFileDropOut = true,
            EnableExternalUrlTextDragOut = true,
            EnableExternalTemplateTextDragOut = true,
            ShowOperationStatus = true,
            MinimizeToTrayOnClose = true,
            EnableContextMenuDetails = true,
            AutoBackupEnabled = true,
            MaxBackupCount = 20,
            ConfirmBeforeDelete = true,
            MoveDeletedItemsToTrash = true,
            SearchTemplateBody = true
        };
    }

    public static AppSettings CreateForFirstRun(StorageLocation location, AppSettings? selectedSettings = null)
    {
        AppSettings settings = selectedSettings ?? CreateRecommended();
        settings.FirstRunCompleted = true;
        settings.StorageMode = location.Mode;
        settings.CustomStorageDirectory = location.Mode == StorageMode.Custom ? location.DataDirectory : string.Empty;
        return settings;
    }
}
