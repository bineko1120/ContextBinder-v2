# UI接続契約

この文書は、**少佐がVisual Studio DesignerでUIを作り、Codexが機能を接続するための約束**です。

目的は、少佐が見た目・文言・配置を自由に調整しつつ、Codexが迷わず処理をつなげられるようにすることです。

## まず読むところ

| やりたいこと | 見る場所 |
|---|---|
| MainFormを作る | [MainForm 必須コントロール](#mainform-必須コントロール) |
| メニューバーを作る | [MainForm MenuStrip](#mainform-menustrip) |
| 並び替えやソートを置く | [並び替え・ソートUI契約](#並び替えソートui契約) |
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

| Name | 種類 | 用途 | 実装状況 | Codexが接続するイベント |
|---|---|---|---|---|
| `mainMenuStrip` | `MenuStrip` | MainForm上部メニューバー | 未実装 | なし。各MenuItemに接続 |
| `fileToolStripMenuItem` | `ToolStripMenuItem` | ファイルメニュー | 未実装 | なし |
| `exportToolStripMenuItem` | `ToolStripMenuItem` | 登録内容を書き出す | 未実装 | `Click` |
| `importToolStripMenuItem` | `ToolStripMenuItem` | 登録内容を読み込む | 未実装 | `Click` |
| `exitToolStripMenuItem` | `ToolStripMenuItem` | アプリを終了する | 未実装 | `Click` |
| `registerToolStripMenuItem` | `ToolStripMenuItem` | 登録メニュー | 未実装 | なし |
| `addGroupToolStripMenuItem` | `ToolStripMenuItem` | グループを追加する | 未実装 | `Click` |
| `addFileToolStripMenuItem` | `ToolStripMenuItem` | ファイルを追加する | 未実装 | `Click` |
| `addFolderToolStripMenuItem` | `ToolStripMenuItem` | フォルダーを追加する | 未実装 | `Click` |
| `addUrlToolStripMenuItem` | `ToolStripMenuItem` | URLを追加する | 未実装 | `Click` |
| `addTemplateToolStripMenuItem` | `ToolStripMenuItem` | テンプレートを追加する | 未実装 | `Click` |
| `editToolStripMenuItem` | `ToolStripMenuItem` | 編集メニュー | 未実装 | なし |
| `openSelectedToolStripMenuItem` | `ToolStripMenuItem` | 選択項目を開く | 未実装 | `Click` |
| `copySelectedToolStripMenuItem` | `ToolStripMenuItem` | 選択項目をコピーする | 未実装 | `Click` |
| `editSelectedToolStripMenuItem` | `ToolStripMenuItem` | 選択項目を編集する | 未実装 | `Click` |
| `detailSelectedToolStripMenuItem` | `ToolStripMenuItem` | 選択項目の詳細を表示する | 未実装 | `Click` |
| `deleteSelectedToolStripMenuItem` | `ToolStripMenuItem` | 選択項目を削除する | 未実装 | `Click` |
| `undoToolStripMenuItem` | `ToolStripMenuItem` | 直前操作を元に戻す | 未実装 | `Click` |
| `moveUpToolStripMenuItem` | `ToolStripMenuItem` | 選択項目を上へ移動する | 未実装 | `Click` |
| `moveDownToolStripMenuItem` | `ToolStripMenuItem` | 選択項目を下へ移動する | 未実装 | `Click` |
| `sortSelectedToolStripMenuItem` | `ToolStripMenuItem` | 選択範囲だけを指定順で並び替える | 未実装 | `Click` |
| `applyCurrentOrderAsManualToolStripMenuItem` | `ToolStripMenuItem` | 現在の表示順を手動並び順として保存する | 未実装 | `Click` |
| `viewToolStripMenuItem` | `ToolStripMenuItem` | 表示メニュー | 未実装 | なし |
| `showBeginnerHintsToolStripMenuItem` | `ToolStripMenuItem` | 初心者向け説明の表示を切り替える | 未実装 | `Click` または `CheckedChanged` 相当 |
| `showIconMeaningToolStripMenuItem` | `ToolStripMenuItem` | アイコンの意味の表示を切り替える | 未実装 | `Click` または `CheckedChanged` 相当 |
| `actionButtonDisplayModeToolStripMenuItem` | `ToolStripMenuItem` | 操作ボタン表示モードの親メニュー | 未実装 | なし |
| `beginnerActionButtonsToolStripMenuItem` | `ToolStripMenuItem` | 初心者向け文字つきボタンへ切り替える | 未実装 | `Click` |
| `compactIconButtonsToolStripMenuItem` | `ToolStripMenuItem` | コンパクトなアイコンボタンへ切り替える | 未実装 | `Click` |
| `showThumbnailsToolStripMenuItem` | `ToolStripMenuItem` | サムネイル表示を切り替える | 未実装 | `Click` |
| `sortModeToolStripMenuItem` | `ToolStripMenuItem` | 表示用ソートの親メニュー | 未実装 | なし |
| `manualOrderToolStripMenuItem` | `ToolStripMenuItem` | 手動並び順で表示する | 未実装 | `Click` |
| `sortByNameToolStripMenuItem` | `ToolStripMenuItem` | 名前順で表示する | 未実装 | `Click` |
| `sortByTypeToolStripMenuItem` | `ToolStripMenuItem` | 種類順で表示する | 未実装 | `Click` |
| `sortByCreatedAtToolStripMenuItem` | `ToolStripMenuItem` | 追加順で表示する | 未実装 | `Click` |
| `sortByUpdatedAtToolStripMenuItem` | `ToolStripMenuItem` | 更新順で表示する | 未実装 | `Click` |
| `toolsToolStripMenuItem` | `ToolStripMenuItem` | ツールメニュー | 未実装 | なし |
| `settingsToolStripMenuItem` | `ToolStripMenuItem` | 設定画面を開く | 未実装 | `Click` |
| `trashToolStripMenuItem` | `ToolStripMenuItem` | ごみ箱を開く | 未実装 | `Click` |
| `backupToolStripMenuItem` | `ToolStripMenuItem` | バックアップ関連画面を開く | 未実装 | `Click` |
| `repairShortcutsToolStripMenuItem` | `ToolStripMenuItem` | ショートカット修復を実行する | 未実装 | `Click` |
| `helpToolStripMenuItem` | `ToolStripMenuItem` | ヘルプメニュー | 未実装 | なし |
| `usageGuideToolStripMenuItem` | `ToolStripMenuItem` | 使い方ガイドを開く | 未実装 | `Click` |
| `aboutToolStripMenuItem` | `ToolStripMenuItem` | バージョン情報を表示する | 未実装 | `Click` |

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

# 並び替え・ソートUI契約

## 基本方針

項目の手動並び替えは、誤操作を避けるため既定では右ボタンドラッグで行う。

| 操作 | 期待する挙動 |
|---|---|
| 右クリックして動かさず離す | 種類別の右クリックメニューを開く |
| 右ボタンを押したままドラッグする | 項目の手動並び替え、またはグループへのコピー/移動 |
| 左ドラッグで外部から中央一覧へドロップ | 新規登録 |
| 左ドラッグで項目を外部へ出す | 外部アプリへファイル/URL/本文を渡す |

右クリックと右ドラッグの判定は、Windowsのドラッグ開始距離を超えた場合だけドラッグ扱いにする。
タッチパッドや右ドラッグが使いづらい環境のため、上下移動ボタンは常に用意する。

## 手動並び順の状態

手動並び順は、以下の2段階で扱う。

| 状態 | 内容 |
|---|---|
| 保存済み手動並び順 | 最後にユーザーが「並び順を保存」した正式な並び |
| 編集中の手動並び順 | 右ドラッグ、上下ボタン、選択範囲ソートで変更中の未保存状態 |

名前順、種類順、追加順、更新順は表示用ソートであり、保存済み手動並び順を破壊しない。
表示用ソート中に右ドラッグや上下移動をした場合は、その時点の表示順を編集中の手動並び順へコピーし、未保存状態にする。

## MainForm コントロール候補

| Name | 種類 | 用途 | 必須 |
|---|---|---|---|
| `sortModeComboBox` | ComboBox | 手動並び順、名前順、種類順、追加順、更新順を切り替える | 推奨 |
| `moveUpButton` | Button | 選択項目を上へ移動し、編集中の手動並び順を更新する | 推奨 |
| `moveDownButton` | Button | 選択項目を下へ移動し、編集中の手動並び順を更新する | 推奨 |
| `saveManualOrderButton` | Button | 編集中の並び順を保存済み手動並び順として確定する | 推奨 |
| `revertUnsavedOrderButton` | Button | 未保存の並び替えを破棄し、最後に保存した手動並び順へ戻す | 推奨 |
| `orderUnsavedStatusLabel` | Label | 未保存の並び順がある時だけ表示する警告/状態表示 | 推奨 |
| `sortSelectedButton` | Button | 選択範囲だけを指定順で並び替える | 任意 |
| `sortSelectedModeComboBox` | ComboBox | 選択範囲ソートの方式を選ぶ | 任意 |
| `restorePreviousManualOrderButton` | Button | 一つ前に保存した手動並び順へ戻す | 任意 |
| `undoOrderMoveButton` | Button | 直前の並び替え操作だけを元に戻す | 任意 |

`orderUnsavedStatusLabel` は、未保存の並び替えが存在する場合だけ表示する。
変更がない時は非表示にする。

表示用ソートを固定したい場合も、まず編集中の手動並び順に反映し、最後に `saveManualOrderButton` で保存する。

## コンパクト操作ボタン候補

コンパクト操作ボタン表示では、文字なしアイコンボタンを使い、ToolTipまたはホバー説明で意味を表示する。

| Name | 種類 | 用途 |
|---|---|---|
| `moveUpToolStripButton` | ToolStripButton | 上へ移動 |
| `moveDownToolStripButton` | ToolStripButton | 下へ移動 |
| `saveManualOrderToolStripButton` | ToolStripButton | 並び順を保存 |
| `revertUnsavedOrderToolStripButton` | ToolStripButton | 保存前に戻す |
| `orderUnsavedStatusToolStripLabel` | ToolStripLabel | 未保存状態の短い表示 |

初心者向け表示とコンパクト表示は見た目だけを分け、内部では同じPresenter処理を呼ぶ。

## MenuStrip 追加候補

表示メニュー：

- `sortModeToolStripMenuItem`
- `manualOrderToolStripMenuItem`
- `sortByNameToolStripMenuItem`
- `sortByTypeToolStripMenuItem`
- `sortByCreatedAtToolStripMenuItem`
- `sortByUpdatedAtToolStripMenuItem`

編集メニュー：

- `moveUpToolStripMenuItem`
- `moveDownToolStripMenuItem`
- `sortSelectedToolStripMenuItem`
- `saveManualOrderToolStripMenuItem`
- `revertUnsavedOrderToolStripMenuItem`
- `restorePreviousManualOrderToolStripMenuItem`

## 未保存確認

並び順に未保存変更がある状態で、その変更が失われる可能性がある操作を行う場合、既定では確認を表示する。

確認文の例：

```text
並び順に保存していない変更があります。

[保存して続ける]
[変更を破棄して続ける]
[キャンセル]

□ 次回からこの選択を自動的に適用する
```

「次回からこの選択を自動的に適用する」を選んだ場合は、SettingsFormの詳細設定から戻せるようにする。

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
| `itemThumbnailColumn` | 画像/動画サムネイル |

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

## 検索・並び順

| Name | 種類 | 役割 |
|---|---|---|
| `defaultItemSortModeComboBox` | `ComboBox` | 既定の並び方を選ぶ |
| `manualReorderInputModeComboBox` | `ComboBox` | 右ドラッグのみ、または左右ドラッグ許可を選ぶ |
| `unsavedOrderBehaviorComboBox` | `ComboBox` | 未保存並び順がある場合の既定動作を選ぶ |

推奨初期値：

- `defaultItemSortModeComboBox`: 手動並び順
- `manualReorderInputModeComboBox`: 右ドラッグのみ
- `unsavedOrderBehaviorComboBox`: 毎回確認する

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
| `moveDeletedItemsToTrashCheckBox` | `CheckBox` | 削除時に登録のごみ箱へ移動 |
| `trashRetentionAutoRadioButton` | `RadioButton` | 指定日数後に登録のごみ箱から自動削除する |
| `trashRetentionManualOnlyRadioButton` | `RadioButton` | 自動削除せず手動でのみ削除する |
| `trashRetentionDaysNumericUpDown` | `NumericUpDown` | 登録のごみ箱に保持する日数。初期値30日 |
| `trashRetentionExplanationLabel` | `Label` | ごみ箱で削除されるのは登録情報だけで、元ファイルは削除されない説明 |
| `emptyTrashButton` | `Button` | ごみ箱内の登録情報をすべて削除する |

## RecycleBinForm / 登録のごみ箱

| Name | 種類 | 役割 |
|---|---|---|
| `trashMeaningNoticeLabel` | `Label` | 元ファイルではなく登録情報だけを削除する説明 |
| `trashGridView` | `DataGridView` | ごみ箱内の登録一覧 |
| `restoreRegistrationButton` | `Button` | 選択した登録を元に戻す |
| `removeRegistrationFromTrashButton` | `Button` | 選択した登録情報をごみ箱から削除する |
| `emptyRegistrationTrashButton` | `Button` | ごみ箱内の登録情報をすべて削除する |

`trashGridView` には、削除日時と自動削除予定日を表示する。
手動削除のみ設定の場合、自動削除予定日は「手動で削除するまで保持」と表示する。

---

# 並び替え・ソート実装方針

並び替え処理はMainFormに直書きせず、Serviceへ分離します。

候補Service:

- `Services/ItemOrderingService.cs`
- `Services/ItemSortService.cs`

候補Model/AppSettings:

```csharp
public enum ItemSortMode
{
    Manual,
    Name,
    Type,
    CreatedAt,
    UpdatedAt
}

public enum ManualReorderInputMode
{
    RightDragOnly,
    LeftOrRightDrag
}

public enum UnsavedOrderBehavior
{
    Ask,
    AutoSave,
    Discard
}
```

`BinderItem.SortOrder` は、最後に保存した手動並び順だけに使う。
右ドラッグ、上下移動、選択範囲ソートは、まず編集中の手動並び順へ反映し、`saveManualOrderButton` で明示保存する。

テスト候補:

- Manual順で表示される
- Name順へ切り替えてもSortOrderは壊れない
- Type順へ切り替えてもSortOrderは壊れない
- 手動並び順へ戻すと保存済み手動順に戻る
- 右ドラッグ後は未保存状態になる
- 上へ移動で編集中の手動並び順が更新される
- 下へ移動で編集中の手動並び順が更新される
- 並び順を保存するとSortOrderが更新される
- 保存前に戻すと最後に保存した手動順へ戻る
- 未保存変更がある時だけ未保存表示が出る
- 未保存確認で「次回から自動適用」を選ぶと設定へ保存される
- 選択範囲だけ名前順にできる
- 選択外の項目位置が不必要に崩れない

---

# ショートカット機能の実装方針
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

4. MainFormの並び替え入口  
   `moveUpButton`、`moveDownButton`、`sortModeComboBox`、`saveManualOrderButton`、`revertUnsavedOrderButton`、`orderUnsavedStatusLabel`。

5. SettingsFormの常駐・起動カテゴリ  
   Windows自動起動、起動時最小化、スタートメニュー登録。

6. 将来機能の入口  
   ごみ箱、インポート、エクスポート、バックアップ復元。最初はDisabledでOK。
