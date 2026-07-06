using ContextBinder.Models;

namespace ContextBinder;

// User-editable UI text values.
// Keep layout sizing in UiLayoutSettings.cs and keep behavior/event logic in Forms/*.cs.
internal static class UiTexts
{
    internal readonly record struct TextPair(string Title, string Description);

    internal readonly record struct ActionText(string Label, string ToolTip);

    internal readonly record struct IconMeaningText(BinderItemType Type, string Title, string Description);

    internal static class FirstRunSetup
    {
        public const string WindowTitle = "ContextBinder 初回セットアップ";
        public const string BackButton = "戻る";
        public const string NextButton = "次へ";
        public const string StartButton = "開始";
        public const string CancelButton = "キャンセル";
        public const string BrowseButton = "参照...";

        public static readonly string[] StepLabels =
        [
            "1. 始め方",
            "2. 保存場所",
            "3. 使いやすさ設定",
            "4. 確認"
        ];

        public const string WelcomeHeading = "ContextBinderへようこそ";
        public const string WelcomeDescription = """
このツールは、ファイル・フォルダ・URL・テンプレート文を
グループごとにまとめて、すぐ開く/コピーできるツールです。

まずは保存場所と使いやすさ設定を選びます。
""";

        public const string RecommendedSetupLabel = "初心者おすすめ設定で始める";
        public const string RecommendedSetupDescription = """
迷った場合はこちらを選んでください。
見やすさと安全性を優先した設定で始めます。
次の画面で保存場所を選べます。
おすすめ設定の内容は後で確認できます。
""";

        public const string CustomSetupLabel = "カスタム設定を選ぶ";
        public const string CustomSetupDescription = """
表示、ドラッグ＆ドロップ、削除確認、バックアップなどを自分で選びます。
ある程度使い方を決めたい人向けです。
保存場所も次の画面で選べます。
""";

        public const string StorageHeading = "登録内容と設定の保存場所を選んでください";
        public const string StandardStorageLabel = "通常の場所に保存（おすすめ）";
        public const string StandardStorageDescription = """
Windowsの標準的なアプリ用フォルダに、登録内容と設定を保存します。
迷った場合はこれを選んでください。
アプリ本体のフォルダを移動しても、登録内容と設定は維持されます。
""";
        public const string PortableStorageLabel = "このアプリのフォルダに保存";
        public const string PortableStorageDescription = """
アプリ本体と同じ場所に ContextBinder_Data フォルダを作り、登録内容と設定を保存します。
フォルダごとバックアップ・移動したい人向けです。
Program Files など書き込み権限が厳しい場所では失敗することがあります。
AppDataには保存しません。
""";
        public const string CustomStorageLabel = "自分で選んだ場所に保存";
        public const string CustomStorageDescription = """
自分で選んだフォルダに登録内容と設定を保存します。
OneDrive、別ドライブ、外部ドライブなどを使いたい人向けです。
同期中、権限不足、外部ドライブ未接続には注意してください。
AppDataには保存しません。
""";
        public const string StandardStorageCurrentDescription = "Windowsの標準的なアプリ用フォルダに登録内容と設定を保存します。";
        public const string PortableStorageCurrentDescription = "このアプリのフォルダに登録内容と設定を保存します。AppDataには保存しません。";
        public const string CustomStorageCurrentDescription = "自分で選んだフォルダに登録内容と設定を保存します。AppDataには保存しません。";
        public const string StoragePreviewPrefix = "保存先プレビュー";
        public const string CustomStorageNotSelected = "未選択";
        public const string CustomFolderDialogDescription = "登録内容と設定を保存するフォルダを選んでください。";

        public static readonly string[] TypeDisplayModeItems =
        [
            "アイコン＋文字",
            "アイコンのみ",
            "文字のみ",
            "非表示（非推奨）"
        ];

        public const string EditRecommendedSettings = "おすすめ設定を少し変更する";
        public const string SettingsCustomHeading = "使いやすさ設定を選んでください";
        public const string SettingsRecommendedHeading = "おすすめ設定の内容を確認してください";
        public const string SettingsCustomDescription = "画面の表示、ドラッグ＆ドロップ、削除確認などを自分の使い方に合わせて選べます。";
        public const string SettingsRecommendedDescription = "初心者おすすめ設定は、見やすさと安全性を優先した初期値です。内容を確認し、必要なら少しだけ変更できます。";

        public const string TypeDisplayModeLabel = "種類表示";
        public const string TypeDisplayModeDescription = "一覧で「種類」をどう表示するか選べます。非表示は分かりづらくなるため非推奨です。";
        public const string ShowBeginnerHintsLabel = "初心者向け説明を表示";
        public const string ShowBeginnerHintsDescription = "画面下に、操作の意味を分かりやすく表示します。";
        public const string ShowIconMeaningLabel = "アイコンの意味を表示";
        public const string ShowIconMeaningDescription = "猫アイコンが何を表しているか表示します。";
        public const string ShowOperationStatusLabel = "操作結果ステータスを表示";
        public const string ShowOperationStatusDescription = "読み込みや保存などの結果を画面下に表示します。";

        public const string ConfirmTitleOnDropAddLabel = "D&D追加時にタイトルを確認";
        public const string ConfirmTitleOnDropAddDescription = "登録時にタイトルを確認したい場合に使います。";
        public const string FocusExistingItemOnDuplicateLabel = "重複時に既存項目へジャンプ";
        public const string FocusExistingItemOnDuplicateDescription = "同じ内容がすでにある場合、その項目を見つけやすくします。";
        public const string EnableGroupDropModifierShortcutsLabel = "Ctrl/Shiftドロップショートカット";
        public const string EnableGroupDropModifierShortcutsDescription = "グループへドラッグしたとき、Ctrl/Shiftキーでコピー・移動を切り替えられます。";
        public const string ConfirmGroupDropCopyMoveLabel = "通常グループドロップ時に確認";
        public const string ConfirmGroupDropCopyMoveDescription = "グループへドロップしたとき、コピーか移動か迷わないよう確認します。";
        public const string EnableItemDragReorderLabel = "項目のD&D並び替え";
        public const string EnableItemDragReorderDescription = "一覧の順番を手で入れ替えられます。";
        public const string EnableExternalFileDropOutLabel = "ファイル/画像/動画/フォルダを外部D&Dで渡す";
        public const string EnableExternalFileDropOutDescription = "ファイル、画像、動画、フォルダーを他のアプリへドラッグして使いたい人向けです。";
        public const string EnableExternalUrlTextDragOutLabel = "URLを外部D&Dでテキストとして渡す";
        public const string EnableExternalUrlTextDragOutDescription = "URLを他のアプリへドラッグして使いたい人向けです。";
        public const string EnableExternalTemplateTextDragOutLabel = "テンプレート本文を外部D&Dで渡す";
        public const string EnableExternalTemplateTextDragOutDescription = "テンプレート文を他のアプリへドラッグして使いたい人向けです。";

        public const string MinimizeToTrayOnCloseLabel = "閉じるボタンでタスクトレイに格納";
        public const string MinimizeToTrayOnCloseDescription = "完全終了せず、タスクトレイに格納します。";
        public const string EnableContextMenuDetailsLabel = "右クリック詳細操作を有効にする";
        public const string EnableContextMenuDetailsDescription = "フォルダを開く、詳細を確認するなどの操作を追加します。";
        public const string AutoBackupEnabledLabel = "自動バックアップ";
        public const string AutoBackupEnabledDescription = "登録内容と設定を自動でバックアップします。";
        public const string MaxBackupCountLabel = "バックアップ保存数";
        public const string MaxBackupCountDescription = "残しておくバックアップの数です。初期値は20件です。";
        public const string ConfirmBeforeDeleteLabel = "削除前に確認";
        public const string ConfirmBeforeDeleteDescription = "削除前に確認して、間違って消しにくくします。";
        public const string MoveDeletedItemsToTrashLabel = "削除時にアプリ内ごみ箱へ移動";
        public const string MoveDeletedItemsToTrashDescription = "すぐ完全削除せず、アプリ内のごみ箱であとから見直せます。";
        public const string SearchTemplateBodyLabel = "テンプレート本文も検索対象にする";
        public const string SearchTemplateBodyDescription = "テンプレート文の中身も検索対象にします。";

        public static readonly TextPair DisplaySettingsCategory = new("表示", "一覧の見え方と、画面下に出す説明を選びます。");
        public static readonly TextPair DragDropSettingsCategory = new("ドラッグ＆ドロップ", "ファイルやURLを登録するとき、または他のアプリへ渡すときの動きを選びます。");
        public static readonly TextPair CloseBehaviorSettingsCategory = new("閉じるときの動作", "アプリを閉じたとき、完全終了するか右下にしまうかを選びます。");
        public static readonly TextPair ContextMenuSettingsCategory = new("右クリックの便利機能", "項目を右クリックしたときに使える操作を増やします。");
        public static readonly TextPair BackupDeleteSettingsCategory = new("バックアップと削除", "登録内容と設定を守り、間違って消しにくくするための設定です。");
        public static readonly TextPair SearchSettingsCategory = new("検索", "項目を探すとき、どこまで検索対象にするかを選びます。");

        public const string ConfirmationHeading = "確認して開始";
        public const string ConfirmationDescription = "選んだ内容を確認してください。「開始」を押すと、登録内容と設定の保存先を作成してContextBinderを起動します。";
        public const string StartModeConfirmationTitle = "始め方";
        public const string StorageConfirmationTitle = "保存場所";
        public const string MainSettingsConfirmationTitle = "主な設定";
        public const string CreatedItemsConfirmationTitle = "作成されるもの";
        public const string SavedItemsConfirmationTitle = "保存されるもの";
        public const string NotSavedItemsConfirmationTitle = "保存されないもの";
        public const string RecommendedSetupSummary = "初心者おすすめ設定";
        public const string CustomSetupSummary = "カスタム設定";

        public static readonly string[] CreatedItems =
        [
            "・contextbinder.store.json",
            "・settings.json",
            "・backups",
            "・trash"
        ];

        public static readonly string[] SavedItems =
        [
            "・登録したファイル、フォルダ、URLの参照先",
            "・登録したテンプレート文",
            "・グループ名と並び順",
            "・表示設定や操作設定",
            "・自動バックアップ",
            "・ごみ箱、削除履歴"
        ];

        public static readonly string[] NotSavedItems =
        [
            "・登録元のファイルそのもの",
            "・登録元の画像や動画そのもの",
            "・登録元のフォルダの中身"
        ];

        public const string CustomStorageRequiredMessage = "自分で選んだ場所に保存する場合は、保存するフォルダを選んでください。";
        public const string ValidationDialogTitle = "入力確認";
        public const string TypeDisplayHiddenWarningMessage = "種類表示を非表示にすると、項目の種類が分かりにくくなります。この設定で進みますか？";
        public const string TypeDisplayHiddenWarningTitle = "種類表示の確認";

        public static string GetStorageModeDisplayName(StorageMode mode)
        {
            return mode switch
            {
                StorageMode.Portable => PortableStorageLabel,
                StorageMode.Custom => CustomStorageLabel,
                _ => StandardStorageLabel
            };
        }

        public static string GetTypeDisplayModeDisplayName(TypeDisplayMode mode)
        {
            return mode switch
            {
                TypeDisplayMode.IconOnly => TypeDisplayModeItems[1],
                TypeDisplayMode.TextOnly => TypeDisplayModeItems[2],
                TypeDisplayMode.Hidden => TypeDisplayModeItems[3],
                _ => TypeDisplayModeItems[0]
            };
        }

        public static string[] BuildMainSettingsSummary(AppSettings settings)
        {
            return
            [
                $"・{TypeDisplayModeLabel}: {GetTypeDisplayModeDisplayName(settings.TypeDisplayMode)}",
                $"・初心者向け説明: {ShowOrHide(settings.ShowBeginnerHints)}",
                $"・アイコンの意味: {ShowOrHide(settings.ShowIconLegend)}",
                $"・操作結果ステータス: {ShowOrHide(settings.ShowOperationStatus)}",
                $"・追加時にタイトル確認: {DoOrNot(settings.ConfirmTitleOnDropAdd)}",
                $"・重複時に既存項目へ移動: {DoOrNot(settings.FocusExistingItemOnDuplicate)}",
                $"・Ctrl/Shiftキーでコピー・移動を切り替え: {UseOrNot(settings.EnableGroupDropModifierShortcuts)}",
                $"・グループへドロップしたときの確認: {DoOrNot(settings.ConfirmGroupDropCopyMove)}",
                $"・項目のドラッグ並び替え: {UseOrNot(settings.EnableItemDragReorder)}",
                $"・閉じるボタンでタスクトレイに格納: {DoOrNot(settings.MinimizeToTrayOnClose)}",
                $"・右クリックの詳細操作: {UseOrNot(settings.EnableContextMenuDetails)}",
                $"・自動バックアップ: {UseOrNot(settings.AutoBackupEnabled)}（{settings.MaxBackupCount}件保持）",
                $"・削除前の確認: {DoOrNot(settings.ConfirmBeforeDelete)}",
                $"・削除時にアプリ内のごみ箱へ移動: {DoOrNot(settings.MoveDeletedItemsToTrash)}",
                $"・テンプレート本文も検索: {DoOrNot(settings.SearchTemplateBody)}"
            ];
        }

        private static string ShowOrHide(bool value)
        {
            return value ? "表示する" : "表示しない";
        }

        private static string UseOrNot(bool value)
        {
            return value ? "使う" : "使わない";
        }

        private static string DoOrNot(bool value)
        {
            return value ? "する" : "しない";
        }
    }

    internal static class MainForm
    {
        public const string WindowTitle = "ContextBinder v2";
        public const string GroupListTitle = "グループ一覧";
        public const string ShowBeginnerHintsToggle = "初心者向け説明を表示";
        public const string ShowIconMeaningToggle = "アイコンの意味を表示";
        public const string BeginnerHintsTitle = "使い方のヒント";
        public const string IconMeaningTitle = "アイコンの意味";
        public const string BeginnerHintsText = """
・グループ追加: 用途ごとに登録先を分けます。
・ファイル / フォルダー / URL / テンプレート追加: 参照先や文章を登録します。
・開く / コピー: 選択した項目を開いたり、パス・URL・本文をコピーします。
・編集 / 詳細 / 削除: 登録内容を確認・整理します。登録元ファイルそのものは削除されません。
""";

        public static readonly IconMeaningText[] IconMeanings =
        [
            new(BinderItemType.Folder, "フォルダー", "フォルダーを開きます"),
            new(BinderItemType.File, "ファイル", "既定のアプリで開きます"),
            new(BinderItemType.Image, "画像", "画像ファイルです"),
            new(BinderItemType.Video, "動画", "動画ファイルです"),
            new(BinderItemType.Url, "URL", "ブラウザで開きます"),
            new(BinderItemType.Template, "テンプレート", "本文をコピーして使います")
        ];

        public static readonly ActionText AddGroupButton = new("グループ追加", "用途ごとに登録先を分けます。");
        public static readonly ActionText AddFileButton = new("ファイル追加", "ファイルへの参照を現在のグループへ登録します。");
        public static readonly ActionText AddFolderButton = new("フォルダー追加", "フォルダーへの参照を現在のグループへ登録します。");
        public static readonly ActionText AddUrlButton = new("URL追加", "WebページのURLを現在のグループへ登録します。");
        public static readonly ActionText AddTemplateButton = new("テンプレート追加", "よく使う文章を登録します。");
        public static readonly ActionText OpenButton = new("開く", "選択した項目を開きます。テンプレートは本文をコピーします。");
        public static readonly ActionText CopyButton = new("コピー", "選択した項目のパス、URL、本文などをコピーします。");
        public static readonly ActionText EditButton = new("編集", "タイトルや参照先を編集します。");
        public static readonly ActionText DetailButton = new("詳細", "登録内容の詳細を確認します。");
        public static readonly ActionText DeleteButton = new("削除", "選択した項目を削除します。登録元ファイルは削除されません。");

        public const string TypeColumnHeader = "種類";
        public const string TitleColumnHeader = "タイトル";
        public const string ReferenceColumnHeader = "参照先";
        public const string StatusColumnHeader = "状態";
        public const string TrayOpen = "開く";
        public const string TrayExit = "終了";
    }
}
