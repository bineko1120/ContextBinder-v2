using ContextBinder.Forms;
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

    AppSettings recommendedSettings = AppSettingsFactory.CreateRecommended();
    AssertRecommendedSettings(recommendedSettings);

    StorageLocationService standardLocationService = new([], appRoot, standardRoot);
    Assert(standardLocationService.ResolveExistingLocation() is null, "未設定時は初回セットアップが必要になること");
    RunStaTest(() =>
    {
        using FirstRunSetupForm setupForm = new(standardLocationService);
        Assert(setupForm.SelectedStorageMode == StorageMode.Standard, "初回セットアップフォームの既定保存場所が標準モードであること");
        Assert(setupForm.SelectedAppSettings.TypeDisplayMode == TypeDisplayMode.IconAndText, "初回セットアップフォームの既定設定が初心者おすすめであること");
    });

    StorageLocation standardLocation = standardLocationService.InitializeFirstRun(StorageMode.Standard);
    Assert(standardLocation.Mode == StorageMode.Standard, "標準モードで保存先解決できること");
    Assert(standardLocation.DataDirectory == Path.GetFullPath(standardRoot), "標準モードの保存先が指定AppData相当になること");
    AssertRequiredDirectoriesExist(standardLocation);

    AppSettings settings = standardLocationService.CreateInitialSettings(standardLocation);
    AssertRecommendedSettings(settings);
    settings.AutoBackupEnabled = false;
    StoreService standardStoreService = new(standardLocation, new BackupService());
    standardStoreService.SaveSettings(settings);
    Assert(File.Exists(standardLocation.SettingsFilePath), "標準モードで settings.json が作成されること");

    AppSettings loadedStandardSettings = standardStoreService.LoadSettings();
    Assert(loadedStandardSettings.FirstRunCompleted, "標準モードで FirstRunCompleted が保存・読み込みされること");
    Assert(loadedStandardSettings.StorageMode == StorageMode.Standard, "標準モードの設定が読み戻されること");
    Assert(loadedStandardSettings.ShowBeginnerHints, "初心者向け説明の初期値が保存・読み込みされること");
    Assert(loadedStandardSettings.ShowIconLegend, "アイコンの意味の初期値が保存・読み込みされること");
    Assert(loadedStandardSettings.TypeDisplayMode == TypeDisplayMode.IconAndText, "種類表示モードの初期値が保存・読み込みされること");

    standardStoreService.EnsureStoreFile();
    Assert(File.Exists(standardLocation.StoreFilePath), "標準モードで contextbinder.store.json が作成されること");
    Assert(standardStoreService.LoadStore().Groups.Count == 0, "標準モードで空の登録内容を読み込めること");

    AppSettings customDisplaySettings = standardLocationService.CreateInitialSettings(standardLocation, AppSettingsFactory.CreateRecommended());
    customDisplaySettings.ShowBeginnerHints = false;
    customDisplaySettings.ShowIconLegend = false;
    customDisplaySettings.TypeDisplayMode = TypeDisplayMode.TextOnly;
    customDisplaySettings.ConfirmTitleOnDropAdd = true;
    customDisplaySettings.FocusExistingItemOnDuplicate = false;
    customDisplaySettings.EnableGroupDropModifierShortcuts = true;
    customDisplaySettings.MaxBackupCount = 7;
    customDisplaySettings.SearchTemplateBody = false;
    standardStoreService.SaveSettings(customDisplaySettings);
    AppSettings loadedCustomDisplaySettings = standardStoreService.LoadSettings();
    Assert(!loadedCustomDisplaySettings.ShowBeginnerHints, "初心者向け説明OFFが settings.json に保存・読み込みされること");
    Assert(!loadedCustomDisplaySettings.ShowIconLegend, "アイコンの意味OFFが settings.json に保存・読み込みされること");
    Assert(loadedCustomDisplaySettings.TypeDisplayMode == TypeDisplayMode.TextOnly, "TypeDisplayMode が保存・読み込みされること");
    Assert(loadedCustomDisplaySettings.ConfirmTitleOnDropAdd, "D&Dタイトル確認設定が保存・読み込みされること");
    Assert(!loadedCustomDisplaySettings.FocusExistingItemOnDuplicate, "重複時ジャンプ設定が保存・読み込みされること");
    Assert(loadedCustomDisplaySettings.EnableGroupDropModifierShortcuts, "Ctrl/Shiftドロップ設定が保存・読み込みされること");
    Assert(loadedCustomDisplaySettings.MaxBackupCount == 7, "バックアップ保持数が保存・読み込みされること");
    Assert(!loadedCustomDisplaySettings.SearchTemplateBody, "テンプレート本文検索設定が保存・読み込みされること");

    string missingIconRoot = Path.Combine(testRoot, "missing-icons");
    RunStaTest(() =>
    {
        using IconAssetService missingIconService = new(missingIconRoot);
        Assert(missingIconService.GetItemIcon(BinderItemType.Image).Width > 0, "アイコンファイルがない場合でも項目アイコンを返せること");
        Assert(missingIconService.GetAppIcon() is not null, "アプリアイコンがない場合でも既定アイコンを返せること");
        using MainForm form = new(new StoreService(standardLocation, new BackupService()), missingIconService);
        Assert(form.Text == "ContextBinder v2", "空の登録内容でも MainForm を初期化できること");
    });

    customDisplaySettings.TypeDisplayMode = TypeDisplayMode.Hidden;
    standardStoreService.SaveSettings(customDisplaySettings);
    RunStaTest(() =>
    {
        using IconAssetService missingIconService = new(missingIconRoot);
        using MainForm form = new(new StoreService(standardLocation, new BackupService()), missingIconService);
        Assert(form.Text == "ContextBinder v2", "説明/アイコンの意味OFFかつ種類非表示でも MainForm を初期化できること");
    });
    standardStoreService.SaveSettings(loadedStandardSettings);

    string invalidIconRoot = Path.Combine(testRoot, "invalid-icons");
    Directory.CreateDirectory(invalidIconRoot);
    File.WriteAllText(Path.Combine(invalidIconRoot, "icon_app.ico"), "not an icon");
    File.WriteAllText(Path.Combine(invalidIconRoot, "icon_cat_file.png"), "not an image");
    using (IconAssetService invalidIconService = new(invalidIconRoot))
    {
        Assert(invalidIconService.GetItemIcon(BinderItemType.File).Width > 0, "壊れた項目アイコンでもプレースホルダーを返せること");
        Assert(invalidIconService.GetAppIcon() is not null, "壊れたアプリアイコンでも既定アイコンを返せること");
    }

    StartupErrorService startupErrorService = new(Path.Combine(testRoot, "fallback-log"));
    string? startupLogPath = startupErrorService.WriteStartupError(
        new InvalidOperationException("startup smoke"),
        standardLocation);
    Assert(startupLogPath == Path.Combine(standardLocation.DataDirectory, StartupErrorService.LogFileName), "起動エラーは登録内容と設定フォルダへ保存されること");
    if (startupLogPath is null)
    {
        throw new InvalidOperationException("テスト失敗: 起動エラーログが保存されること");
    }

    Assert(File.ReadAllText(startupLogPath).Contains("startup smoke", StringComparison.Ordinal), "起動エラーログに例外内容が残ること");
    Assert(StartupErrorService.CreateUserMessage(new InvalidOperationException("startup smoke"), startupLogPath).Contains("起動エラーログ", StringComparison.Ordinal), "起動エラー表示にログ保存先が含まれること");

    string blockedLogTarget = Path.Combine(testRoot, "blocked-log-target");
    string fallbackLogRoot = Path.Combine(testRoot, "fallback-log-root");
    File.WriteAllText(blockedLogTarget, "not a directory");
    StartupErrorService fallbackStartupErrorService = new(fallbackLogRoot);
    string? fallbackStartupLogPath = fallbackStartupErrorService.WriteStartupError(
        new InvalidOperationException("fallback startup smoke"),
        new StorageLocation { DataDirectory = blockedLogTarget });
    Assert(fallbackStartupLogPath == Path.Combine(fallbackLogRoot, StartupErrorService.LogFileName), "登録内容と設定フォルダにログを書けない場合はアプリフォルダ相当へ保存されること");

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

static void AssertRecommendedSettings(AppSettings settings)
{
    Assert(settings.TypeDisplayMode == TypeDisplayMode.IconAndText, "初心者おすすめの種類表示がアイコン＋文字であること");
    Assert(settings.ShowBeginnerHints, "初心者おすすめで初心者向け説明がONであること");
    Assert(settings.ShowIconLegend, "初心者おすすめでアイコンの意味がONであること");
    Assert(settings.ShowOperationStatus, "初心者おすすめで操作結果ステータスがONであること");
    Assert(!settings.ConfirmTitleOnDropAdd, "初心者おすすめでD&Dタイトル確認がOFFであること");
    Assert(settings.FocusExistingItemOnDuplicate, "初心者おすすめで重複時ジャンプがONであること");
    Assert(!settings.EnableGroupDropModifierShortcuts, "初心者おすすめでCtrl/ShiftドロップショートカットがOFFであること");
    Assert(settings.ConfirmGroupDropCopyMove, "初心者おすすめで通常グループドロップ確認がONであること");
    Assert(settings.EnableItemDragReorder, "初心者おすすめで項目D&D並び替えがONであること");
    Assert(settings.EnableExternalFileDropOut, "初心者おすすめでファイル外部D&DがONであること");
    Assert(settings.EnableExternalUrlTextDragOut, "初心者おすすめでURL外部D&DがONであること");
    Assert(settings.EnableExternalTemplateTextDragOut, "初心者おすすめでテンプレート外部D&DがONであること");
    Assert(settings.MinimizeToTrayOnClose, "初心者おすすめで閉じるボタンのタスクトレイ格納がONであること");
    Assert(settings.EnableContextMenuDetails, "初心者おすすめで右クリック詳細操作がONであること");
    Assert(settings.AutoBackupEnabled, "初心者おすすめで自動バックアップがONであること");
    Assert(settings.MaxBackupCount == 20, "初心者おすすめでバックアップ保持数が20であること");
    Assert(settings.ConfirmBeforeDelete, "初心者おすすめで削除前確認がONであること");
    Assert(settings.MoveDeletedItemsToTrash, "初心者おすすめで削除時ごみ箱移動がONであること");
    Assert(settings.SearchScope == SearchScope.CurrentGroup, "初心者おすすめで検索範囲が現在のグループであること");
    Assert(settings.SearchTemplateBody, "初心者おすすめでテンプレート本文検索がONであること");
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

static void RunStaTest(Action action)
{
    Exception? caughtException = null;
    Thread thread = new(() =>
    {
        try
        {
            action();
        }
        catch (Exception ex)
        {
            caughtException = ex;
        }
    });

    thread.SetApartmentState(ApartmentState.STA);
    thread.Start();
    thread.Join();

    if (caughtException is not null)
    {
        throw new InvalidOperationException("テスト失敗: STA 初期化処理で例外が発生しました。", caughtException);
    }
}
