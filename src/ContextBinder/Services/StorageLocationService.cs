using ContextBinder.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ContextBinder.Services;

public sealed class StorageLocationService
{
    public const string DataDirectoryName = "ContextBinder_Data";
    public const string StoreFileName = "contextbinder.store.json";
    public const string SettingsFileName = "settings.json";
    public const string StorageMarkerFileName = "contextbinder.storage.json";
    public const string PortableFlagFileName = "portable.flag";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new StorageModeJsonConverter() }
    };

    private readonly string[] _args;
    private readonly string _applicationDirectory;
    private readonly string _standardDataDirectory;

    public StorageLocationService(
        string[]? args = null,
        string? applicationDirectory = null,
        string? standardDataDirectory = null)
    {
        _args = args ?? [];
        _applicationDirectory = Path.GetFullPath(applicationDirectory ?? AppContext.BaseDirectory);
        _standardDataDirectory = ResolveStandardDataDirectory(standardDataDirectory);
    }

    public StorageLocation? ResolveExistingLocation()
    {
        string? commandLineDataDirectory = GetCommandLineDataDirectory();
        if (!string.IsNullOrWhiteSpace(commandLineDataDirectory))
        {
            return CreateLocation(StorageMode.Custom, commandLineDataDirectory);
        }

        string storageMarkerPath = GetStorageMarkerPath();
        if (File.Exists(storageMarkerPath))
        {
            return ResolveFromStorageMarker(storageMarkerPath);
        }

        if (File.Exists(GetPortableFlagPath()))
        {
            return CreateLocation(StorageMode.Portable);
        }

        StorageLocation standardLocation = CreateLocation(StorageMode.Standard);
        if (File.Exists(standardLocation.SettingsFilePath) || File.Exists(standardLocation.StoreFilePath))
        {
            return standardLocation;
        }

        return null;
    }

    public StorageLocation CreateLocation(StorageMode mode, string? customDirectory = null)
    {
        string dataDirectory = mode switch
        {
            StorageMode.Portable => Path.Combine(_applicationDirectory, DataDirectoryName),
            StorageMode.Custom when !string.IsNullOrWhiteSpace(customDirectory) => customDirectory,
            StorageMode.Custom => throw new InvalidOperationException("自分で選んだ保存場所が指定されていません。"),
            _ => _standardDataDirectory
        };

        dataDirectory = Path.GetFullPath(dataDirectory);

        return new StorageLocation
        {
            Mode = mode,
            DataDirectory = dataDirectory,
            StoreFilePath = Path.Combine(dataDirectory, StoreFileName),
            SettingsFilePath = Path.Combine(dataDirectory, SettingsFileName),
            BackupDirectory = Path.Combine(dataDirectory, "backups"),
            TrashDirectory = Path.Combine(dataDirectory, "trash")
        };
    }

    public StorageLocation InitializeFirstRun(StorageMode mode, string? customDirectory = null)
    {
        StorageLocation location = CreateLocation(mode, customDirectory);
        EnsureDirectories(location);

        if (mode == StorageMode.Portable)
        {
            try
            {
                File.WriteAllText(GetPortableFlagPath(), "ContextBinder portable storage");
            }
            catch (Exception ex) when (IsStorageAccessException(ex))
            {
                throw new InvalidOperationException(
                    "このアプリのフォルダに保存する設定を書き込めませんでした。アプリフォルダへの書き込み権限を確認してください。",
                    ex);
            }
        }

        if (mode == StorageMode.Custom)
        {
            WriteStorageMarker(location);
        }

        return location;
    }

    public void EnsureDirectories(StorageLocation location)
    {
        try
        {
            Directory.CreateDirectory(location.DataDirectory);
            Directory.CreateDirectory(location.BackupDirectory);
            Directory.CreateDirectory(location.TrashDirectory);
        }
        catch (Exception ex) when (IsStorageAccessException(ex))
        {
            throw new InvalidOperationException(CreateDirectoryPreparationMessage(location), ex);
        }
    }

    public AppSettings CreateInitialSettings(StorageLocation location, AppSettings? selectedSettings = null)
    {
        return AppSettingsFactory.CreateForFirstRun(location, selectedSettings);
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

    private StorageLocation ResolveFromStorageMarker(string storageMarkerPath)
    {
        try
        {
            string json = File.ReadAllText(storageMarkerPath);
            StorageLocationMarker marker = JsonSerializer.Deserialize<StorageLocationMarker>(json, JsonOptions)
                ?? throw new InvalidOperationException("保存場所設定を読み込めませんでした。");

            return marker.Mode switch
            {
                StorageMode.Standard => CreateLocation(StorageMode.Standard),
                StorageMode.Portable => CreateLocation(StorageMode.Portable),
                StorageMode.Custom => CreateLocation(StorageMode.Custom, marker.DataDirectory),
                _ => throw new InvalidOperationException("保存場所設定の種類が不正です。")
            };
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or JsonException or InvalidOperationException)
        {
            throw new InvalidOperationException("保存場所設定を読み込めませんでした。", ex);
        }
    }

    private void WriteStorageMarker(StorageLocation location)
    {
        try
        {
            StorageLocationMarker marker = new()
            {
                Mode = location.Mode,
                DataDirectory = location.DataDirectory
            };

            string json = JsonSerializer.Serialize(marker, JsonOptions);
            File.WriteAllText(GetStorageMarkerPath(), json);
        }
        catch (Exception ex) when (IsStorageAccessException(ex))
        {
            throw new InvalidOperationException(
                "保存場所の設定をアプリフォルダに保存できませんでした。別の保存場所を選ぶか、アプリフォルダへの書き込み権限を確認してください。",
                ex);
        }
    }

    private string GetStorageMarkerPath()
    {
        return Path.Combine(_applicationDirectory, StorageMarkerFileName);
    }

    private string GetPortableFlagPath()
    {
        return Path.Combine(_applicationDirectory, PortableFlagFileName);
    }

    private static string ResolveStandardDataDirectory(string? standardDataDirectory)
    {
        if (!string.IsNullOrWhiteSpace(standardDataDirectory))
        {
            return Path.GetFullPath(standardDataDirectory);
        }

        string appDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        if (string.IsNullOrWhiteSpace(appDataDirectory))
        {
            throw new InvalidOperationException("通常の保存場所を取得できませんでした。Windowsのアプリ用フォルダを確認してください。");
        }

        return Path.GetFullPath(Path.Combine(appDataDirectory, "ContextBinder"));
    }

    private static string CreateDirectoryPreparationMessage(StorageLocation location)
    {
        string modeDescription = location.Mode switch
        {
            StorageMode.Portable => "このアプリのフォルダ",
            StorageMode.Custom => "選択したフォルダ",
            _ => "通常の場所"
        };

        return $"登録内容と設定の保存場所を準備できませんでした。\n保存方法: {modeDescription}\n保存先: {location.DataDirectory}";
    }

    private static bool IsStorageAccessException(Exception ex)
    {
        return ex is IOException or UnauthorizedAccessException or ArgumentException or NotSupportedException;
    }

    private sealed class StorageLocationMarker
    {
        public int SchemaVersion { get; set; } = 1;

        public StorageMode Mode { get; set; } = StorageMode.Standard;

        public string DataDirectory { get; set; } = string.Empty;
    }
}

public sealed class StorageModeJsonConverter : JsonConverter<StorageMode>
{
    public override StorageMode Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Number && reader.TryGetInt32(out int numericValue))
        {
            return Enum.IsDefined(typeof(StorageMode), numericValue)
                ? (StorageMode)numericValue
                : StorageMode.Standard;
        }

        string? value = reader.GetString();
        return value switch
        {
            "Standard" or "AppData" => StorageMode.Standard,
            "Portable" or "ApplicationFolder" => StorageMode.Portable,
            "Custom" or "CustomFolder" => StorageMode.Custom,
            _ => StorageMode.Standard
        };
    }

    public override void Write(Utf8JsonWriter writer, StorageMode value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
