using System.Text.Json;
using ContextBinder.Models;

namespace ContextBinder.Services;

public sealed class StoreService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters =
        {
            new StorageModeJsonConverter(),
            new System.Text.Json.Serialization.JsonStringEnumConverter()
        }
    };

    private readonly BackupService _backupService;

    public StoreService(StorageLocation location, BackupService backupService)
    {
        Location = location;
        _backupService = backupService;
    }

    public StorageLocation Location { get; }

    public AppSettings LoadSettings()
    {
        EnsureDataDirectories();

        if (!File.Exists(Location.SettingsFilePath))
        {
            return CreateDefaultSettings();
        }

        try
        {
            string json = File.ReadAllText(Location.SettingsFilePath);
            AppSettings settings = JsonSerializer.Deserialize<AppSettings>(json, JsonOptions) ?? CreateDefaultSettings();
            ApplyLocationDefaults(settings);
            return settings;
        }
        catch (Exception ex) when (ex is IOException or JsonException or UnauthorizedAccessException)
        {
            throw new InvalidOperationException("登録内容と設定の読み込みに失敗しました。settings.json を確認してください。", ex);
        }
    }

    public void SaveSettings(AppSettings settings)
    {
        ApplyLocationDefaults(settings);
        EnsureDataDirectories();
        WriteJsonAtomically(Location.SettingsFilePath, settings);
    }

    public ContextBinderStore LoadStore()
    {
        EnsureDataDirectories();

        if (!File.Exists(Location.StoreFilePath))
        {
            return new ContextBinderStore();
        }

        try
        {
            string json = File.ReadAllText(Location.StoreFilePath);
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
            _backupService.CreateBackupIfExists(Location.StoreFilePath, Location.BackupDirectory);
            _backupService.PruneBackups(Location.BackupDirectory, settings.MaxBackupCount);
        }

        WriteJsonAtomically(Location.StoreFilePath, store);
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

    private AppSettings CreateDefaultSettings()
    {
        AppSettings settings = new();
        ApplyLocationDefaults(settings);
        return settings;
    }

    private void ApplyLocationDefaults(AppSettings settings)
    {
        settings.StorageMode = Location.Mode;
        settings.CustomStorageDirectory = Location.Mode == StorageMode.Custom ? Location.DataDirectory : string.Empty;
    }

    private void EnsureDataDirectories()
    {
        Directory.CreateDirectory(Location.DataDirectory);
        Directory.CreateDirectory(Location.BackupDirectory);
        Directory.CreateDirectory(Location.TrashDirectory);
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
