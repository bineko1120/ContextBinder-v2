# ContextBinder v2 実装状況一覧

この一覧は `feature/initial-contextbinder-v2` 時点のコードを基準にした棚卸しです。
PR #3 の Designer-first 化は未マージのため、この表では現行土台の機能実装状況を優先して整理します。

## 状態の定義

| 状態 | 意味 |
|---|---|
| 実装済み | ユーザー操作から処理まで一通り動く |
| UIだけ存在 | ボタンやメニューはあるが、処理は未完成または仮実装 |
| 土台のみ存在 | Model / Service / 設定値などの一部はあるが、画面連携や主要処理が不足 |
| 未実装 | 画面、処理、保存のいずれも未整備 |
| 要確認 | 実装はあるが、仕様どおりか追加確認が必要 |

## 機能別一覧

| 機能名 | 現状 | 関連ファイル | UIの有無 | 処理の有無 | テストの有無 | 次に必要な作業 | 優先度 |
|---|---|---|---|---|---|---|---|
| グループ管理 | 実装済み | `Forms/MainForm.cs`, `Models/BinderGroup.cs`, `Services/StoreService.cs` | あり | 追加、選択、保存/読み込みあり | あり（保存/読み込み中心） | 名前変更、並び替え、削除の整理 | 高 |
| ファイル追加 | 実装済み | `Forms/MainForm.cs`, `Forms/ItemEditForm.cs`, `Services/FileTypeDetector.cs` | あり | OpenFileDialogから登録 | あり（一部） | D&D時のタイトル確認設定を反映 | 高 |
| フォルダー追加 | 実装済み | `Forms/MainForm.cs`, `Forms/ItemEditForm.cs`, `Services/FileTypeDetector.cs` | あり | FolderBrowserDialogから登録 | あり（一部） | フォルダー存在確認表示の改善 | 高 |
| URL追加 | 実装済み | `Forms/MainForm.cs`, `Forms/ItemEditForm.cs`, `Services/FileTypeDetector.cs` | あり | 手動入力で登録 | あり（一部） | URL形式チェックと重複時ジャンプ | 高 |
| テンプレート追加 | 実装済み | `Forms/MainForm.cs`, `Forms/ItemEditForm.cs` | あり | 手動入力で登録 | あり（一部） | 専用編集画面とプレビュー改善 | 高 |
| 開く | 実装済み | `Forms/MainForm.cs`, `Services/ItemActionService.cs` | あり | `UseShellExecute = true` で開く。テンプレートはコピー | なし（手動確認中心） | エラー文言と存在しない参照先の案内改善 | 高 |
| コピー | 実装済み | `Forms/MainForm.cs`, `Services/ClipboardService.cs`, `Services/ItemActionService.cs` | あり | パス、URL、テンプレート本文をコピー | なし（STA依存） | タイトルコピーなど右クリック拡張 | 高 |
| 編集 | 実装済み | `Forms/MainForm.cs`, `Forms/ItemEditForm.cs` | あり | タイトル、参照先、種類を編集 | なし（手動確認中心） | Designer-first の `ItemEditForm` 化 | 高 |
| 詳細 | UIだけ存在 | `Forms/MainForm.cs` | あり | MessageBoxで簡易表示 | なし | `ItemDetailForm` をDesigner-firstで追加 | 中 |
| 削除 | 土台のみ存在 | `Forms/MainForm.cs`, `Services/TrashService.cs`, `Models/DeletedItemRecord.cs` | あり | 削除確認、アプリ内ごみ箱記録あり | なし | ごみ箱画面、復元、完全削除 | 高 |
| JSON保存/読み込み | 実装済み | `Services/StoreService.cs`, `Models/ContextBinderStore.cs`, `Models/AppSettings.cs` | なし | `contextbinder.store.json`, `settings.json` 保存/読み込み | あり | スキーマ移行方針の整理 | 高 |
| 保存場所選択 | 実装済み | `Services/StorageLocationService.cs`, `Forms/FirstRunSetupForm.cs`, `Models/StorageLocation.cs` | あり | Standard / Portable / Custom 解決 | あり | 設定画面からの変更/移行UI | 高 |
| 初回セットアップ | 実装済み | `Forms/FirstRunSetupForm.cs`, `Services/AppSettingsFactory.cs`, `Program.cs` | あり | 保存場所と初期設定を保存 | あり | Designer-first で作り直しやすい分割 | 高 |
| 設定画面 | 未実装 | `Models/AppSettings.cs`, `Services/AppSettingsFactory.cs` | なし | 設定値の保存土台のみ | あり（一部設定値） | `SettingsForm` をDesigner-firstで追加 | 高 |
| 検索 | 土台のみ存在 | `Services/SearchService.cs`, `Models/SearchScope.cs`, `Models/AppSettings.cs` | なし | Serviceはキーワード検索可能 | なし | MainFormの検索UI接続 | 中 |
| 絞り込み | 未実装 | `Models/BinderItemType.cs` | なし | 種類フィルター未実装 | なし | 種類フィルターUIと検索Service拡張 | 中 |
| 右クリック種類別メニュー | 未実装 | `Forms/MainForm.cs`, `Services/ItemActionService.cs`, `Services/ClipboardService.cs` | 基本メニューのみ | 種類別メニューはTODO | なし | 種類別の開く/コピー/フォルダーを開く等を追加 | 中 |
| D&D追加 | 土台のみ存在 | `Forms/MainForm.cs`, `Services/DragDropService.cs`, `Services/FileTypeDetector.cs` | あり | ファイル/テキストの追加は可能 | なし | 設定値反映、タイトル確認、重複時ジャンプ | 高 |
| グループ間コピー/移動 | 未実装 | `Models/BinderGroup.cs`, `Models/BinderItem.cs` | なし | 未実装 | なし | グループD&D、コピー/移動確認 | 中 |
| 外部D&D | 未実装 | `Models/AppSettings.cs` | なし | 設定値のみ | あり（一部設定値） | DataObject生成、ドラッグ開始処理 | 中 |
| ファイル存在チェック | 土台のみ存在 | `Forms/MainForm.cs` | 状態列あり | 表示時に存在確認 | なし | 再チェック、警告表示、まとめて確認 | 中 |
| ごみ箱 | 土台のみ存在 | `Services/TrashService.cs`, `Models/DeletedItemRecord.cs`, `Models/ContextBinderStore.cs` | なし | 削除時記録のみ | なし | `RecycleBinForm`、復元、完全削除 | 高 |
| Undo | 未実装 | なし | なし | 未実装 | なし | 削除直後Undoから検討 | 中 |
| バックアップ | 土台のみ存在 | `Services/BackupService.cs`, `Services/StoreService.cs`, `Models/AppSettings.cs` | なし | 保存時バックアップ、保持数削除 | あり（一部） | 起動時バックアップ、設定画面、フォルダを開く | 中 |
| バックアップ復元 | 未実装 | `Services/BackupService.cs` | なし | 未実装 | なし | `BackupRestoreDialog` と復元処理 | 中 |
| インポート/エクスポート | 未実装 | `Services/StoreService.cs` | なし | 未実装 | なし | 読み込み/書き出しServiceとDialog | 中 |
| タスクトレイ | 実装済み | `Forms/MainForm.cs`, `Services/IconAssetService.cs` | あり | NotifyIcon、開く/終了、閉じる時格納 | あり（初期化中心） | 起動時最小化との連携 | 高 |
| アイコンの意味 | 実装済み | `Forms/MainForm.cs`, `Services/IconAssetService.cs`, `UiTexts.cs` | あり | 表示ON/OFFを保存 | あり（設定保存中心） | Designer管理UIへの移行 | 中 |
| 初心者向け説明 | 実装済み | `Forms/MainForm.cs`, `UiTexts.cs` | あり | 表示ON/OFFを保存 | あり（設定保存中心） | Designer管理UIへの移行 | 中 |
| BOOTH配布用publish | 未実装 | `ContextBinder.csproj`, `README.md` | なし | publish手順/ZIP作成未整備 | なし | publishプロファイル、ZIP作成手順、同梱物確認 | 中 |

## 準MVPに追加する導入・常駐支援

これらは将来候補ではなく、設定画面実装時に優先して入れる準MVP対象です。

| 機能名 | 分類 | 現状 | 関連ファイル | UIの有無 | 処理の有無 | テストの有無 | 次に必要な作業 | 優先度 |
|---|---|---|---|---|---|---|---|---|
| スタートメニュー登録 | 配布・導入補助 / SettingsForm対象 | 未実装 | なし | なし | 未実装 | なし | `ShortcutService` と SettingsForm のボタン追加 | 中〜高 |
| スタートメニュー登録解除 | 配布・導入補助 / SettingsForm対象 | 未実装 | なし | なし | 未実装 | なし | ユーザー単位Start Menuショートカット削除 | 中〜高 |
| Windows起動時自動起動 | 常駐・起動 / SettingsForm対象 | 未実装 | なし | なし | 未実装 | なし | Startupフォルダへのショートカット作成 | 高 |
| Windows起動時自動起動解除 | 常駐・起動 / SettingsForm対象 | 未実装 | なし | なし | 未実装 | なし | Startupフォルダのショートカット削除 | 高 |
| 起動時最小化 | 常駐・起動 / SettingsForm対象 | 未実装 | `Models/AppSettings.cs`, `Program.cs`, `Forms/MainForm.cs` | なし | 閉じる時格納のみ実装済み | なし | `StartMinimizedToTray` を追加して起動時に反映 | 高 |
| ショートカット修復 | 配布・導入補助 / SettingsForm対象 | 未実装 | なし | なし | 未実装 | なし | exe移動後にStart Menu/Startupリンク先を更新 | 中 |

## 次フェーズの推奨順

1. `SettingsForm` をDesigner-firstで追加し、既存 `AppSettings` の編集と保存を接続する。
2. `ShortcutService` を追加し、スタートメニュー登録とWindows起動時自動起動をユーザー単位ショートカットで実装する。
3. MainFormの検索UIを接続し、`SearchService` に種類フィルターを追加する。
4. ごみ箱画面を追加し、復元と完全削除を実装する。
5. バックアップ復元、インポート/エクスポート、BOOTH向けpublish手順を整える。
