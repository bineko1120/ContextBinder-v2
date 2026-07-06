using ContextBinder.Models;
using ContextBinder.Services;

string testRoot = Path.Combine(Path.GetTempPath(), $"ContextBinderTests_{Guid.NewGuid():N}");

try
{
    Directory.CreateDirectory(testRoot);

    string standardRoot = Path.Combine(testRoot, "standard");
    string appRoot = Path.Combine(testRoot, "app");
    string portableAppRoot = Path.Combine(testRoot, "portable-app");
    string customAppRoot = Path.Combine(testRoot, "custom-app");
    string customRoot = Path.Combine(testRoot, "custom");
    Directory.CreateDirectory(appRoot);
    Directory.CreateDirectory(portableAppRoot);
    Directory.CreateDirectory(customAppRoot);

    FileTypeDetector detector = new();

    string imagePath = Path.Combine(testRoot, "sample.png");
    File.WriteAllText(imagePath, "not a real image");
    Assert(detector.DetectFromPath(imagePath) == BinderItemType.Image, "画像拡張子が Image と判定されること");

    StorageLocationService standardLocationService = new([], appRoot, standardRoot);
    Assert(standardLocationService.ResolveExistingLocation() is null, "未設定時は初回セットアップが必要になること");

    StorageLocation standardLocation = standardLocationService.InitializeFirstRun(StorageMode.Standard);
    Assert(standardLocation.Mode == StorageMode.Standard, "標準モードで保存先解決できること");
    Assert(standardLocation.DataDirectory == Path.GetFullPath(standardRoot), "標準モードの保存先が指定AppData相当になること");
    AssertRequiredDirectoriesExist(standardLocation);

    AppSettings settings = standardLocationService.CreateInitialSettings(standardLocation);
    settings.AutoBackupEnabled = false;
    StoreService standardStoreService = new(standardLocation, new BackupService());
    standardStoreService.SaveSettings(settings);
    Assert(File.Exists(standardLocation.SettingsFilePath), "標準モードで settings.json が作成されること");

    AppSettings loadedStandardSettings = standardStoreService.LoadSettings();
    Assert(loadedStandardSettings.FirstRunCompleted, "標準モードで FirstRunCompleted が保存・読み込みされること");
    Assert(loadedStandardSettings.StorageMode == StorageMode.Standard, "標準モードの設定が読み戻されること");

    standardStoreService.EnsureStoreFile();
    Assert(File.Exists(standardLocation.StoreFilePath), "標準モードで contextbinder.store.json が作成されること");
    Assert(standardStoreService.LoadStore().Groups.Count == 0, "標準モードで空の登録内容を読み込めること");

    BinderGroup standardGroup = new()
    {
        Name = "標準モード確認",
        SortOrder = 0
    };

    ContextBinderStore standardStore = new()
    {
        Groups = [standardGroup]
    };

    standardStoreService.SaveStore(standardStore, loadedStandardSettings);
    Assert(standardStoreService.LoadStore().Groups.Count == 1, "標準モードで登録内容を保存・読み込みできること");

    StorageLocation? resolvedStandardLocation = standardLocationService.ResolveExistingLocation();
    Assert(resolvedStandardLocation?.Mode == StorageMode.Standard, "標準設定がある場合は標準モードで起動すること");

    string corruptStandardRoot = Path.Combine(testRoot, "corrupt-standard");
    string corruptAppRoot = Path.Combine(testRoot, "corrupt-app");
    Directory.CreateDirectory(corruptAppRoot);
    StorageLocationService corruptLocationService = new([], corruptAppRoot, corruptStandardRoot);
    StorageLocation corruptStandardLocation = corruptLocationService.InitializeFirstRun(StorageMode.Standard);
    File.WriteAllText(corruptStandardLocation.SettingsFilePath, string.Empty);
    StoreService corruptStoreService = new(corruptStandardLocation, new BackupService());
    AssertThrowsFriendlyError(
        () => corruptStoreService.LoadSettings(),
        "settings.json",
        "空の settings.json は分かりやすいエラーになること");

    File.WriteAllText(corruptStandardLocation.StoreFilePath, "{");
    AssertThrowsFriendlyError(
        () => corruptStoreService.LoadStore(),
        "contextbinder.store.json",
        "壊れた contextbinder.store.json は分かりやすいエラーになること");

    string blockedStandardRoot = Path.Combine(testRoot, "blocked-standard");
    string blockedAppRoot = Path.Combine(testRoot, "blocked-app");
    Directory.CreateDirectory(blockedAppRoot);
    File.WriteAllText(blockedStandardRoot, "not a directory");
    StorageLocationService blockedLocationService = new([], blockedAppRoot, blockedStandardRoot);
    AssertThrowsFriendlyError(
        () => blockedLocationService.InitializeFirstRun(StorageMode.Standard),
        "登録内容と設定",
        "保存先に同名ファイルがある場合は分かりやすいエラーになること");

    string unusedPortableStandardRoot = Path.Combine(testRoot, "unused-standard-portable");
    StorageLocationService portableLocationService = new([], portableAppRoot, unusedPortableStandardRoot);
    StorageLocation portableLocation = portableLocationService.InitializeFirstRun(StorageMode.Portable);
    Assert(portableLocation.Mode == StorageMode.Portable, "ポータブルモードで保存先解決できること");
    Assert(portableLocation.DataDirectory == Path.Combine(Path.GetFullPath(portableAppRoot), StorageLocationService.DataDirectoryName), "ポータブルモードの保存先がアプリフォルダ内になること");
    Assert(File.Exists(Path.Combine(portableAppRoot, StorageLocationService.PortableFlagFileName)), "portable.flag が作成されること");
    AssertRequiredDirectoriesExist(portableLocation);
    Assert(portableLocationService.ResolveExistingLocation()?.Mode == StorageMode.Portable, "portable.flag からポータブルモードを読み込めること");
    Assert(!Directory.Exists(unusedPortableStandardRoot), "ポータブルモードでは標準保存先相当のフォルダを作成しないこと");

    string unusedCustomStandardRoot = Path.Combine(testRoot, "unused-standard-custom");
    StorageLocationService customLocationService = new([], customAppRoot, unusedCustomStandardRoot);
    StorageLocation customLocation = customLocationService.InitializeFirstRun(StorageMode.Custom, customRoot);
    Assert(customLocation.Mode == StorageMode.Custom, "カスタムモードで保存先解決できること");
    Assert(customLocation.DataDirectory == Path.GetFullPath(customRoot), "カスタムモードの保存先が指定フォルダになること");
    Assert(File.Exists(Path.Combine(customAppRoot, StorageLocationService.StorageMarkerFileName)), "contextbinder.storage.json が作成されること");
    AssertRequiredDirectoriesExist(customLocation);

    StorageLocation? resolvedCustomLocation = customLocationService.ResolveExistingLocation();
    Assert(resolvedCustomLocation?.Mode == StorageMode.Custom, "contextbinder.storage.json からカスタムモードを読み込めること");
    if (resolvedCustomLocation is null)
    {
        throw new InvalidOperationException("テスト失敗: contextbinder.storage.json から保存場所を読み込めること");
    }

    Assert(resolvedCustomLocation.DataDirectory == customLocation.DataDirectory, "contextbinder.storage.json から保存先フォルダを復元できること");
    Assert(!Directory.Exists(unusedCustomStandardRoot), "カスタムモードでは標準保存先相当のフォルダを作成しないこと");

    BinderGroup group = new()
    {
        Name = "TRPG",
        SortOrder = 0
    };

    BinderItem urlItem = new()
    {
        GroupId = group.Id,
        Type = BinderItemType.Url,
        Title = "Example",
        PathOrUrl = "https://example.com/"
    };

    BinderItem templateItem = new()
    {
        GroupId = group.Id,
        Type = BinderItemType.Template,
        Title = "定型文",
        TemplateText = "お世話になっております。"
    };

    group.Items.Add(urlItem);
    group.Items.Add(templateItem);

    ContextBinderStore store = new()
    {
        Groups = [group]
    };

    StoreService storeService = new(customLocation, new BackupService());
    AppSettings customSettings = customLocationService.CreateInitialSettings(customLocation);
    customSettings.AutoBackupEnabled = false;
    storeService.SaveSettings(customSettings);
    storeService.SaveStore(store, customSettings);

    ContextBinderStore loadedStore = storeService.LoadStore();
    Assert(loadedStore.Groups.Count == 1, "グループが保存・読み込みされること");
    Assert(loadedStore.Groups[0].Items.Count == 2, "項目が保存・読み込みされること");
    Assert(loadedStore.Groups[0].Items[1].TemplateText == templateItem.TemplateText, "テンプレート本文が保持されること");

    BinderItem duplicateUrl = new()
    {
        GroupId = group.Id,
        Type = BinderItemType.Url,
        Title = "Duplicate",
        PathOrUrl = "https://example.com"
    };
    Assert(storeService.HasDuplicateReference(loadedStore.Groups[0], duplicateUrl), "同一グループ内のURL重複が検出されること");

    BinderItem otherUrl = new()
    {
        GroupId = group.Id,
        Type = BinderItemType.Url,
        Title = "Other",
        PathOrUrl = "https://example.org"
    };
    Assert(!storeService.HasDuplicateReference(loadedStore.Groups[0], otherUrl), "異なる参照先が重複扱いされないこと");

    Console.WriteLine("CONTEXTBINDER_SMOKE_TESTS_OK");
}
finally
{
    if (Directory.Exists(testRoot))
    {
        Directory.Delete(testRoot, recursive: true);
    }
}

static void Assert(bool condition, string message)
{
    if (!condition)
    {
        throw new InvalidOperationException($"テスト失敗: {message}");
    }
}

static void AssertRequiredDirectoriesExist(StorageLocation location)
{
    Assert(Directory.Exists(location.DataDirectory), "登録内容と設定フォルダが作成されること");
    Assert(Directory.Exists(location.BackupDirectory), "backups フォルダが作成されること");
    Assert(Directory.Exists(location.TrashDirectory), "trash フォルダが作成されること");
}

static void AssertThrowsFriendlyError(Action action, string expectedMessagePart, string message)
{
    try
    {
        action();
    }
    catch (InvalidOperationException ex)
    {
        Assert(ex.Message.Contains("登録内容と設定", StringComparison.Ordinal), message);
        Assert(ex.Message.Contains(expectedMessagePart, StringComparison.Ordinal), message);
        return;
    }

    throw new InvalidOperationException($"テスト失敗: {message}");
}
