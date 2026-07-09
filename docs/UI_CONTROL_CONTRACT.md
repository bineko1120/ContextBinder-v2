# UI接続契約

この文書は、**少佐がVisual Studio DesignerでUIを作り、Codexが機能を接続するための約束**です。

目的は、少佐が見た目・文言・配置を自由に調整しつつ、Codexが迷わず処理をつなげられるようにすることです。

## まず読むところ

| やりたいこと | 見る場所 |
|---|---|
| MainFormを作る | [MainForm 必須コントロール](#mainform-必須コントロール) |
| メニューバーを作る | [MainForm MenuStrip](#mainform-menustrip) |
| 検索やごみ箱など将来機能の入口を置く | [MainForm 追加予定コントロール](#mainform-追加予定コントロール) |
| 操作ボタン表示やサムネイルを置く | [表示モードとサムネイル](#表示モードとサムネイル) |
| 設定画面を作る | [SettingsForm コントロール](#settingsform-コントロール) |
| スタートメニュー/自動起動を置く | [常駐・起動 / ショートカット](#常駐起動--ショートカット) |
| Codexへ依頼する文面 | [Codexに依頼するときのテンプレート](#codexに依頼するときのテンプレート) |

---

## 基本ルール

### 少佐が自由に触ってよいもの

- `Text`
- `Size`
- `Location`
- `Margin`
- `Padding`
- `Dock`
- `Anchor`
- 表示順
- GroupBox / Panel / Button / Label の配置
- 固定説明文

### 触るときに注意するもの

| 項目 | 理由 |
|---|---|
| `Name` | Codexが機能接続に使うため |
| イベントハンドラ | Codexが処理をつなぐ場所のため |
| DataGridView列の内部設定 | 表示処理と関係するため |
| Services / Models | 保存や実処理の本体のため |

### Codexが守ること

- Designerで作ったUIを勝手に作り直さない。
- `Text / Size / Location / Margin / Padding / Dock / Anchor` を必要なく上書きしない。
- 機能追加は、できるだけ Presenter / Service 側で行う。
- UI変更が必要な場合は、理由を報告する。
- 未実装ボタンは、実装前は `Enabled = false` または「今後実装予定」表示にする。

---

# MainForm MenuStrip

MenuStripはDesignerで配置しやすいため、少佐が先に作って構いません。
Codexは `Name` 契約に従って `Click` イベントを接続します。

| Name | 種類 | 役割 |
|---|---|---|
| `mainMenuStrip` | `MenuStrip` | MainForm上部メニューバー |
| `fileToolStripMenuItem` | `ToolStripMenuItem` | ファイルメニュー |
| `exportToolStripMenuItem` | `ToolStripMenuItem` | 登録内容を書き出す |
| `importToolStripMenuItem` | `ToolStripMenuItem` | 登録内容を読み込む |
| `exitToolStripMenuItem` | `ToolStripMenuItem` | アプリを終了 |
| `registerToolStripMenuItem` | `ToolStripMenuItem` | 登録メニュー |
| `addGroupToolStripMenuItem` | `ToolStripMenuItem` | グループ追加 |
| `addFileToolStripMenuItem` | `ToolStripMenuItem` | ファイル追加 |
| `addFolderToolStripMenuItem` | `ToolStripMenuItem` | フォルダー追加 |
| `addUrlToolStripMenuItem` | `ToolStripMenuItem` | URL追加 |
| `addTemplateToolStripMenuItem` | `ToolStripMenuItem` | テンプレート追加 |
| `editToolStripMenuItem` | `ToolStripMenuItem` | 編集メニュー |
| `openSelectedToolStripMenuItem` | `ToolStripMenuItem` | 選択項目を開く |
| `copySelectedToolStripMenuItem` | `ToolStripMenuItem` | 選択項目をコピー |
| `editSelectedToolStripMenuItem` | `ToolStripMenuItem` | 選択項目を編集 |
| `detailSelectedToolStripMenuItem` | `ToolStripMenuItem` | 選択項目の詳細 |
| `deleteSelectedToolStripMenuItem` | `ToolStripMenuItem` | 選択項目を削除 |
| `undoToolStripMenuItem` | `ToolStripMenuItem` | 直前操作を元に戻す |
| `viewToolStripMenuItem` | `ToolStripMenuItem` | 表示メニュー |
| `showBeginnerHintsToolStripMenuItem` | `ToolStripMenuItem` | 使い方のヒント表示切り替え |
| `showIconMeaningToolStripMenuItem` | `ToolStripMenuItem` | アイコンの意味表示切り替え |
| `actionButtonDisplayModeToolStripMenuItem` | `ToolStripMenuItem` | 操作ボタン表示モード |
| `beginnerActionButtonsToolStripMenuItem` | `ToolStripMenuItem` | 初心者向け文字つきボタン |
| `compactIconButtonsToolStripMenuItem` | `ToolStripMenuItem` | コンパクトなアイコンボタン |
| `showThumbnailsToolStripMenuItem` | `ToolStripMenuItem` | サムネイル表示切り替え |
| `toolsToolStripMenuItem` | `ToolStripMenuItem` | ツールメニュー |
| `settingsToolStripMenuItem` | `ToolStripMenuItem` | 設定画面を開く |
| `trashToolStripMenuItem` | `ToolStripMenuItem` | ごみ箱を開く |
| `backupToolStripMenuItem` | `ToolStripMenuItem` | バックアップ関連 |
| `repairShortcutsToolStripMenuItem` | `ToolStripMenuItem` | ショートカット修復 |
| `helpToolStripMenuItem` | `ToolStripMenuItem` | ヘルプメニュー |
| `usageGuideToolStripMenuItem` | `ToolStripMenuItem` | 使い方ガイド |
| `aboutToolStripMenuItem` | `ToolStripMenuItem` | バージョン情報 |

---

# MainForm 必須コントロール

MainFormをDesignerで作るとき、まず必要なものです。

| Name | 種類 | 役割 |
|---|---|---|
| `groupListBox` | `ListBox` | グループ一覧 |
| `itemGridView` | `DataGridView` | 項目一覧 |
| `addGroupButton` | `Button` | グループ追加 |
| `addFileButton` | `Button` | ファイル追加 |
| `addFolderButton` | `Button` | フォルダー追加 |
| `addUrlButton` | `Button` | URL追加 |
| `addTemplateButton` | `Button` | テンプレート追加 |
| `openButton` | `Button` | 選択項目を開く |
| `copyButton` | `Button` | パス、URL、本文をコピー |
| `editButton` | `Button` | 選択項目を編集 |
| `deleteButton` | `Button` | 選択項目を削除 |
| `statusLabel` | `Label` | 操作結果や保存状態を表示 |

## あるとよい基本コントロール

| Name | 種類 | 役割 |
|---|---|---|
| `detailButton` | `Button` | 詳細表示 |
| `settingsButton` | `Button` | 設定画面を開く |
| `showBeginnerHintsCheckBox` | `CheckBox` | 使い方のヒントを表示/非表示 |
| `showIconMeaningCheckBox` | `CheckBox` | アイコンの意味を表示/非表示 |
| `beginnerHintsGroupBox` | `GroupBox` | 使い方のヒント表示エリア |
| `iconMeaningGroupBox` | `GroupBox` | アイコンの意味表示エリア |
| `iconMeaningPanel` | `FlowLayoutPanel` または `Panel` | 猫アイコン説明を並べる場所 |

---

# 表示モードとサムネイル

## 操作ボタン表示モード

候補設定名は `ActionButtonDisplayMode` です。

| 値 | 内容 | 用途 |
|---|---|---|
| `BeginnerText` | 文字つきボタン | 初期値。初心者向け |
| `CompactIcon` | アイコンボタン中心 | 省スペース。ToolTip/ホバー説明を使う |
| `TextAndIcon` | アイコン＋文字 | 将来候補 |

| Name | 種類 | 役割 |
|---|---|---|
| `beginnerActionPanel` | `Panel` または `FlowLayoutPanel` | 初心者向け文字つきボタン領域 |
| `compactActionToolStrip` | `ToolStrip` | コンパクトなアイコン操作領域 |
| `compactActionPanel` | `Panel` または `FlowLayoutPanel` | ToolStripを使わない場合のコンパクト操作領域 |
| `actionToolTip` | `ToolTip` | 操作説明のToolTip |
| `actionHoverDescriptionLabel` | `Label` | ホバー中の操作説明 |

## サムネイル表示

候補設定名は `ItemVisualMode` です。

| 値 | 内容 |
|---|---|
| `List` | 通常一覧 |
| `ThumbnailList` | 一覧内に小さなサムネイルを表示 |
| `LargeThumbnail` | 大きめのサムネイル中心表示 |

| Name | 種類 | 役割 |
|---|---|---|
| `itemVisualModeComboBox` | `ComboBox` | 項目一覧の見せ方を切り替える |
| `showThumbnailsToolStripMenuItem` | `ToolStripMenuItem` | メニューからサムネイル表示を切り替える |
| `thumbnailSizeComboBox` | `ComboBox` | サムネイルサイズを選ぶ |
| `thumbnailSizeTrackBar` | `TrackBar` | サムネイルサイズを細かく調整 |
| `thumbnailPreviewPanel` | `Panel` | サムネイル設定のプレビュー |
| `itemThumbnailColumn` | `DataGridViewImageColumn` | 画像/動画のサムネイル列 |

注意。

- まずは画像サムネイルを優先する。
- 動画サムネイルは次段階。
- 読み込み失敗時は通常アイコンへフォールバックする。
- 動画サムネイルはWindows Shellサムネイルを優先する。
- FFmpeg同梱はライセンスと配布負荷があるため慎重に判断する。

---

# MainForm 追加予定コントロール

まだ未実装でも、先にDesignerで置いてよい入口です。

| Name | 種類 | 役割 | 実装前の扱い |
|---|---|---|---|
| `searchTextBox` | `TextBox` | 検索キーワード | 置いてOK |
| `searchButton` | `Button` | 検索実行 | 未接続ならDisabled |
| `searchScopeComboBox` | `ComboBox` | 現在のグループ/全体 | 未接続ならDisabled |
| `typeFilterComboBox` | `ComboBox` | 種類フィルター | 未接続ならDisabled |
| `clearSearchButton` | `Button` | 検索条件クリア | 未接続ならDisabled |
| `trashButton` | `Button` | ごみ箱を開く | 未接続ならDisabled |
| `importButton` | `Button` | 登録内容を読み込む | 未接続ならDisabled |
| `exportButton` | `Button` | 登録内容を書き出す | 未接続ならDisabled |
| `undoButton` | `Button` | 直前操作を元に戻す | 未接続ならDisabled |
| `moveUpButton` | `Button` | 項目を上へ移動 | 未接続ならDisabled |
| `moveDownButton` | `Button` | 項目を下へ移動 | 未接続ならDisabled |

---

# DataGridView 推奨列

`itemGridView` の列は、Codex側で作っても、少佐がDesignerで作っても構いません。

Designerで列を作る場合は、以下のNameを推奨します。

| Name | 役割 |
|---|---|
| `itemIconColumn` | 種類アイコン |
| `itemTypeColumn` | 種類文字 |
| `itemTitleColumn` | タイトル |
| `itemReferenceColumn` | パス、URL、テンプレート概要 |
| `itemStatusColumn` | 存在しないファイルなどの状態 |

---

# 右クリックメニュー

右クリックメニューをDesignerで置く場合の推奨Nameです。

| Name | 種類 | 役割 |
|---|---|---|
| `itemContextMenuStrip` | `ContextMenuStrip` | 項目右クリックメニュー |
| `openToolStripMenuItem` | `ToolStripMenuItem` | 開く |
| `copyToolStripMenuItem` | `ToolStripMenuItem` | コピー |
| `editToolStripMenuItem` | `ToolStripMenuItem` | 編集 |
| `detailToolStripMenuItem` | `ToolStripMenuItem` | 詳細 |
| `deleteToolStripMenuItem` | `ToolStripMenuItem` | 削除 |
| `openContainingFolderToolStripMenuItem` | `ToolStripMenuItem` | 置いてあるフォルダーを開く |
| `copyTitleToolStripMenuItem` | `ToolStripMenuItem` | タイトルをコピー |

---

# SettingsForm コントロール

設定画面は未実装ですが、準MVPで優先して作る対象です。

## 表示

| Name | 種類 | 役割 |
|---|---|---|
| `typeDisplayModeComboBox` | `ComboBox` | 種類表示：アイコン＋文字/アイコンのみ/文字のみ/非表示 |
| `settingsShowBeginnerHintsCheckBox` | `CheckBox` | 使い方のヒントを表示 |
| `settingsShowIconMeaningCheckBox` | `CheckBox` | アイコンの意味を表示 |
| `actionButtonDisplayModeComboBox` | `ComboBox` | 操作ボタン表示モードを切り替える |
| `itemVisualModeComboBox` | `ComboBox` | 項目一覧の見せ方を切り替える |
| `showThumbnailsCheckBox` | `CheckBox` | 画像/動画のサムネイル表示を使う |
| `thumbnailSizeComboBox` | `ComboBox` | サムネイルの既定サイズを選ぶ |

## 常駐・起動 / ショートカット

| Name | 種類 | 役割 |
|---|---|---|
| `minimizeToTrayOnCloseCheckBox` | `CheckBox` | 閉じるボタンでタスクトレイに格納 |
| `autoStartWithWindowsCheckBox` | `CheckBox` | Windows起動時に自動起動 |
| `startMinimizedCheckBox` | `CheckBox` | 自動起動時に最小化/タスクトレイで起動 |
| `registerStartMenuButton` | `Button` | スタートメニューに登録 |
| `unregisterStartMenuButton` | `Button` | スタートメニューから削除 |
| `repairShortcutsButton` | `Button` | ショートカットのリンク先を修復 |

## バックアップ・削除

| Name | 種類 | 役割 |
|---|---|---|
| `autoBackupEnabledCheckBox` | `CheckBox` | 自動バックアップを使う |
| `maxBackupCountNumericUpDown` | `NumericUpDown` | バックアップ保持数 |
| `confirmBeforeDeleteCheckBox` | `CheckBox` | 削除前に確認 |
| `moveDeletedItemsToTrashCheckBox` | `CheckBox` | 削除時にアプリ内ごみ箱へ移動 |

---

# ショートカット機能の実装方針

スタートメニュー登録とWindows起動時自動起動は、UIから直接ファイル操作せず、Serviceへ分離します。

候補Service:

- `Services/ShortcutService.cs`
- `Services/StartupRegistrationService.cs`

責務:

- ユーザー単位のStart Menu Programs配下へショートカットを作成する。
- ユーザー単位のStart Menu Programs配下からショートカットを削除する。
- ユーザー単位のStartupフォルダへショートカットを作成する。
- ユーザー単位のStartupフォルダからショートカットを削除する。
- ショートカットの存在を確認する。
- ショートカットのリンク先が現在のexeと一致するか確認する。
- 壊れたショートカットを現在のexeパスへ修復する。

注意:

- 管理者権限を要求しない。
- 第一候補はユーザー単位ショートカット。
- レジストリRunキーやタスクスケジューラは第一候補にしない。
- ZIP配布ではアプリフォルダが移動される可能性があるため、修復導線を用意する。
- 解除ボタンを用意し、アンインストーラーなしでもユーザーが片付けられるようにする。

---

# Codexに依頼するときのテンプレート

```text
Designerで配置済みのUIを作り直さず、docs/UI_CONTROL_CONTRACT.md のNameに接続してください。
Text / Size / Location / Margin / Padding / Dock / Anchor は必要なく上書きしないでください。
未実装ボタンは、実装するまでDisabledまたは「今後実装予定」表示にしてください。
保存処理やファイル操作はServicesへ分離してください。
```

---

# 少佐が先にDesignerで用意するとよい順番

1. MainFormの必須コントロール  
   グループ一覧、項目一覧、追加/操作ボタン、ステータス。

2. MainFormのMenuStripと説明エリア  
   `mainMenuStrip`、`beginnerHintsGroupBox`、`iconMeaningGroupBox`、`iconMeaningPanel`。

3. MainFormの検索入口と表示モード入口  
   検索ボックス、検索ボタン、検索範囲、種類フィルター、操作ボタン表示モード、サムネイル表示。

4. SettingsFormの常駐・起動カテゴリ  
   Windows自動起動、起動時最小化、スタートメニュー登録。

5. 将来機能の入口  
   ごみ箱、インポート、エクスポート、バックアップ復元。最初はDisabledでOK。
