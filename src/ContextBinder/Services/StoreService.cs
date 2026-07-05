using System.Text.Json;
using ContextBinder.Models;

namespace ContextBinder.Services;

public sealed class StoreService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
    };

    private readonly StorageLocationService _storageLocationService;
    private readonly BackupService _backupService;

    public StoreService(StorageLocationService storageLocationService, BackupService backupService)
    {
        _storageLocationService = storageLocationService;
        _backupService = backupService;
        Paths = _storageLocationService.ResolveStoragePaths();
    }

    public StoragePaths Paths { get; private set; }

    public AppSettings LoadSettings()
    {
        EnsureDataDirectories();

        if (!File.Exists(Paths.SettingsFilePath))
        {
            return new AppSettings();
        }

        try
        {
            string json = File.ReadAllText(Paths.SettingsFilePath);
            AppSettings settings = JsonSerializer.Deserialize<AppSettings>(json, JsonOptions) ?? new AppSettings();
            UseSettings(settings);
            return settings;
        }
        catch (Exception ex) when (ex is IOException or JsonException or UnauthorizedAccessException)
        {
            throw new InvalidOperationException("登録内容と設定の読み込みに失敗しました。settings.json を確認してください。", ex);
        }
    }

    public void SaveSettings(AppSettings settings)
    {
        UseSettings(settings);
        EnsureDataDirectories();
        WriteJsonAtomically(Paths.SettingsFilePath, settings);
    }

    public ContextBinderStore LoadStore()
    {
        EnsureDataDirectories();

        if (!File.Exists(Paths.StoreFilePath))
        {
            return new ContextBinderStore();
        }

        try
        {
            string json = File.ReadAllText(Paths.StoreFilePath);
            ContextBinderStore store = JsonSerializer.Deserialize<ContextBinderStore>(json, JsonOptions) ?? new ContextBinderStore();
            NormalizeStore(store);
            return store;
        }
        catch (Exception ex) when (ex is IOException or JsonException or UnauthorizedAccessException)
        {
            throw new InvalidOperationException("登録内容と設定の読み込みに失敗しました。contextbinder.store.json を確認してください。", ex);
        }
    }

    public void SaveStore(ContextBinderStore store, AppSettings settings)
    {
        EnsureDataDirectories();
        store.UpdatedAt = DateTimeOffset.Now;

        if (settings.AutoBackupEnabled)
        {
            _backupService.CreateBackupIfExists(Paths.StoreFilePath, Paths.BackupDirectory);
            _backupService.PruneBackups(Paths.BackupDirectory, settings.MaxBackupCount);
        }

        WriteJsonAtomically(Paths.StoreFilePath, store);
    }

    public bool HasDuplicateReference(BinderGroup group, BinderItem item, string? exceptItemId = null)
    {
        string referenceKey = NormalizeReference(item);
        if (string.IsNullOrWhiteSpace(referenceKey))
        {
            return false;
        }

        return group.Items.Any(existing =>
            !string.Equals(existing.Id, exceptItemId, StringComparison.OrdinalIgnoreCase)
            && string.Equals(NormalizeReference(existing), referenceKey, StringComparison.OrdinalIgnoreCase));
    }

    private void UseSettings(AppSettings settings)
    {
        Paths = _storageLocationService.ResolveStoragePaths(settings);
    }

    private void EnsureDataDirectories()
    {
        Directory.CreateDirectory(Paths.DataDirectory);
        Directory.CreateDirectory(Paths.BackupDirectory);
        Directory.CreateDirectory(Paths.TrashDirectory);
    }

    private static void NormalizeStore(ContextBinderStore store)
    {
        store.Groups ??= [];
        store.DeletedItems ??= [];

        foreach (BinderGroup group in store.Groups)
        {
            group.Items ??= [];
            foreach (BinderItem item in group.Items)
            {
                item.GroupId = group.Id;
            }
        }
    }

    private static void WriteJsonAtomically<T>(string filePath, T value)
    {
        string directory = Path.GetDirectoryName(filePath) ?? AppContext.BaseDirectory;
        Directory.CreateDirectory(directory);

        string temporaryPath = Path.Combine(directory, $"{Path.GetFileName(filePath)}.tmp");
        string json = JsonSerializer.Serialize(value, JsonOptions);
        File.WriteAllText(temporaryPath, json);
        File.Move(temporaryPath, filePath, overwrite: true);
    }

    private static string NormalizeReference(BinderItem item)
    {
        string reference = item.ReferenceText.Trim();
        if (string.IsNullOrWhiteSpace(reference))
        {
            return string.Empty;
        }

        if (item.Type is BinderItemType.File or BinderItemType.Image or BinderItemType.Video or BinderItemType.Folder)
        {
            try
            {
                return Path.GetFullPath(reference).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            }
            catch (ArgumentException)
            {
                return reference;
            }
        }

        if (item.Type == BinderItemType.Url)
        {
            return reference.TrimEnd('/');
        }

        return reference;
    }
}
