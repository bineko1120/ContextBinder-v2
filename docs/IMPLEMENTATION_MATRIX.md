# ContextBinder v2 実装状況まとめ

> UIを作るときは、まず [`UI_SCREEN_MAP.md`](UI_SCREEN_MAP.md) を見てください。
> この文書は、各機能が今どこまで実装されているかを確認するための棚卸しです。

この文書は、ContextBinder v2 の「今どこまでできていて、次に何を作るか」を見るための棚卸しです。

細かい関連ファイルや実装詳細は、必要になったタイミングで Codex に確認させます。ここでは、少佐が開発順を判断しやすい粒度に絞ります。

## まず見る場所

| 見たいこと | 見る場所 |
|---|---|
| UI全体と画面一覧 | [`UI_SCREEN_MAP.md`](UI_SCREEN_MAP.md) |
| 今すぐ使える機能 | [実装済み](#実装済み) |
| 入口はあるが未完成の機能 | [土台あり・要仕上げ](#土台あり要仕上げ) |
| これから作る機能 | [未実装](#未実装) |
| 表示モード/サムネイル | [準MVPに追加する表示・操作UI](#準mvpに追加する表示操作ui) |
| スタートメニュー/自動起動 | [準MVPに昇格した機能](#準mvpに昇格した機能) |
| 次に作る順番 | [推奨順](#推奨順) |

## 状態の意味

| 状態 | 意味 |
|---|---|
| 実装済み | 画面から操作でき、基本処理まで動く |
| 土台あり | Model / Service / UI の一部はあるが、まだ仕上げが必要 |
| UIのみ | ボタンや入口はあるが、処理はまだ弱い |
| 未実装 | これから作る |
| 要確認 | 動くが、仕様どおりか手動確認したい |

---

## 実装済み

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
| JSON保存/読み込み | 実装済み | `Services/StoreService.cs`, `Models/ContextBinderStore.cs`, `Models/AppSettings.cs` | なし | `contextbinder.store.json`, `settings.json` 保存/読み込み | あり | スキーマ移行方針の整理 | 高 |
| 保存場所選択 | 実装済み | `Services/StorageLocationService.cs`, `Forms/FirstRunSetupForm.cs`, `Models/StorageLocation.cs` | あり | Standard / Portable / Custom 解決 | あり | 設定画面からの変更/移行UI | 高 |
| 初回セットアップ | 実装済み | `Forms/FirstRunSetupForm.cs`, `Services/AppSettingsFactory.cs`, `Program.cs` | あり | 保存場所と初期設定を保存 | あり | Designer-first で作り直しやすい分割 | 高 |
| タスクトレイ | 実装済み | `Forms/MainForm.cs`, `Services/IconAssetService.cs` | あり | NotifyIcon、開く/終了、閉じる時格納 | あり（初期化中心） | 起動時最小化との連携 | 高 |
| アイコンの意味 | 実装済み | `Forms/MainForm.cs`, `Services/IconAssetService.cs`, `UiTexts.cs` | あり | 表示ON/OFFを保存 | あり（設定保存中心） | Designer管理UIへの移行 | 中 |
| 初心者向け説明 | 実装済み | `Forms/MainForm.cs`, `UiTexts.cs` | あり | 表示ON/OFFを保存 | あり（設定保存中心） | Designer管理UIへの移行 | 中 |

---

## 土台あり・要仕上げ

| 機能名 | 現状 | 関連ファイル | UIの有無 | 処理の有無 | テストの有無 | 次に必要な作業 | 優先度 |
|---|---|---|---|---|---|---|---|
| 詳細 | UIのみ存在 | `Forms/MainForm.cs` | あり | MessageBoxで簡易表示 | なし | `ItemDetailForm` をDesigner-firstで追加 | 中 |
| 削除 | 土台のみ存在 | `Forms/MainForm.cs`, `Services/TrashService.cs`, `Models/DeletedItemRecord.cs` | あり | 削除確認、アプリ内ごみ箱記録あり | なし | ごみ箱画面、復元、完全削除 | 高 |
| 検索 | 土台のみ存在 | `Services/SearchService.cs`, `Models/SearchScope.cs`, `Models/AppSettings.cs` | なし | Serviceはキーワード検索可能 | なし | MainFormの検索UI接続 | 中 |
| D&D追加 | 土台のみ存在 | `Forms/MainForm.cs`, `Services/DragDropService.cs`, `Services/FileTypeDetector.cs` | あり | ファイル/テキストの追加は可能 | なし | 設定値反映、タイトル確認、重複時ジャンプ | 高 |
| ファイル存在チェック | 土台のみ存在 | `Forms/MainForm.cs` | 状態列あり | 表示時に存在確認 | なし | 再チェック、警告表示、まとめて確認 | 中 |
| ごみ箱 | 土台のみ存在 | `Services/TrashService.cs`, `Models/DeletedItemRecord.cs`, `Models/ContextBinderStore.cs` | なし | 削除時記録のみ | なし | `RecycleBinForm`、復元、完全削除 | 高 |
| バックアップ | 土台のみ存在 | `Services/BackupService.cs`, `Services/StoreService.cs`, `Models/AppSettings.cs` | なし | 保存時バックアップ、保持数削除 | あり（一部） | 起動時バックアップ、設定画面、フォルダを開く | 中 |

---

## 未実装

| 機能名 | 関連ファイル | 次に必要な作業 | 優先度 |
|---|---|---|---|
| 設定画面 | `Models/AppSettings.cs`, `Services/AppSettingsFactory.cs` | `SettingsForm` をDesigner-firstで追加 | 高 |
| 絞り込み | `Models/BinderItemType.cs` | 種類フィルターUIと検索Service拡張 | 中 |
| 右クリック種類別メニュー | `Forms/MainForm.cs`, `Services/ItemActionService.cs`, `Services/ClipboardService.cs` | 種類別の開く/コピー/フォルダーを開く等を追加 | 中 |
| グループ間コピー/移動 | `Models/BinderGroup.cs`, `Models/BinderItem.cs` | グループD&D、コピー/移動確認 | 中 |
| 外部D&D | `Models/AppSettings.cs` | DataObject生成、ドラッグ開始処理 | 中 |
| Undo | なし | 削除直後Undoから検討 | 中 |
| バックアップ復元 | `Services/BackupService.cs` | `BackupRestoreDialog` と復元処理 | 中 |
| インポート/エクスポート | `Services/StoreService.cs` | 読み込み/書き出しServiceとDialog | 中 |
| BOOTH配布用publish | `ContextBinder.csproj`, `README.md` | publishプロファイル、ZIP作成手順、同梱物確認 | 中 |

---

## 準MVPに追加する表示・操作UI

| 機能名 | 現状 | 内容 | 優先度 |
|---|---|---|---|
| 操作ボタン表示モード | 未実装 | 初心者向け文字ボタン / コンパクトアイコンボタン | 中〜高 |
| 上部メニューバー | 未実装 | ファイル / 登録 / 編集 / 表示 / ツール / ヘルプ | 中〜高 |
| 画像サムネイル表示 | 未実装 | 画像のサムネイルを一覧表示 | 中 |
| 動画サムネイル表示 | 未実装 | 動画のサムネイルを一覧表示。配布・速度に注意 | 中〜低 |
| 項目並び替え | 土台あり | D&D、上下ボタン、手動順保存 | 高 |
| 表示用ソート | 未実装 | 手動 / 名前 / 種類 / 追加 / 更新順 | 中〜高 |
| 選択範囲ソート | 未実装 | 選択した項目だけ指定順で並べ、手動順として保存 | 中 |

---

## 準MVPに昇格した機能

| 機能名 | 分類 | 現状 | 次に必要な作業 | 優先度 |
|---|---|---|---|---|
| スタートメニュー登録 | 配布・導入補助 | 未実装 | `ShortcutService` と SettingsForm のボタン追加 | 中〜高 |
| スタートメニュー登録解除 | 配布・導入補助 | 未実装 | ユーザー単位Start Menuショートカット削除 | 中〜高 |
| Windows起動時自動起動 | 常駐・起動 | 未実装 | Startupフォルダへのショートカット作成 | 高 |
| Windows起動時自動起動解除 | 常駐・起動 | 未実装 | Startupフォルダのショートカット削除 | 高 |
| 起動時最小化 | 常駐・起動 | 未実装 | `StartMinimizedToTray` を追加して起動時に反映 | 高 |
| ショートカット修復 | 配布・導入補助 | 未実装 | exe移動後にStart Menu/Startupリンク先を更新 | 中 |

---

## 推奨順

1. 少佐が [`UI_SCREEN_MAP.md`](UI_SCREEN_MAP.md) に沿ってMainFormをDesignerで作る。
2. Codexが `UI_CONTROL_CONTRACT.md` のNameに既存機能を接続する。
3. `SettingsForm` をDesigner-firstで追加し、既存設定と保存を接続する。
4. `ShortcutService` を追加し、スタートメニュー登録とWindows自動起動を実装する。
5. 検索・絞り込み・並び替えをMainFormへ接続する。
6. ごみ箱画面を追加し、復元と完全削除を実装する。
7. バックアップ復元、インポート/エクスポート、BOOTH向けpublishを整える。
