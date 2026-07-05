using ContextBinder.Models;
using ContextBinder.Services;

string testRoot = Path.Combine(Path.GetTempPath(), $"ContextBinderTests_{Guid.NewGuid():N}");

try
{
    Directory.CreateDirectory(testRoot);

    AppSettings settings = new()
    {
        StorageMode = StorageMode.CustomFolder,
        CustomStorageDirectory = testRoot,
        AutoBackupEnabled = false
    };

    StoreService storeService = new(new StorageLocationService([]), new BackupService());
    FileTypeDetector detector = new();

    string imagePath = Path.Combine(testRoot, "sample.png");
    File.WriteAllText(imagePath, "not a real image");
    Assert(detector.DetectFromPath(imagePath) == BinderItemType.Image, "画像拡張子が Image と判定されること");

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

    storeService.SaveSettings(settings);
    storeService.SaveStore(store, settings);

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
