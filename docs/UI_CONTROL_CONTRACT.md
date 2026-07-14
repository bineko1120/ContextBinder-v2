# ContextBinder v2 UI接続契約

この文書は、少佐が Visual Studio Designer で作ったUIへ、Codexが安全に機能を接続するための契約です。

- 少佐は見た目、配置、色、余白、固定文言を決める
- Codexは `Name` 契約に従って処理を接続する
- 同じ機能に、文字ボタン・アイコンボタン・MenuStripなど複数の入口を置いてよい
- 複数の入口は、内部では同じPresenter処理を呼ぶ

画面全体の考え方は [`UI_SCREEN_MAP.md`](UI_SCREEN_MAP.md) を参照してください。

---

## 1. 基本ルール

### 少佐が自由に変更してよいもの

- `Text`
- `Size`
- `Location`
- `Margin`
- `Padding`
- `Dock`
- `Anchor`
- 色、フォント、画像
- 表示順
- Panel / GroupBox / ToolStripの構成
- コントロールを置く場所
- 固定の説明文

### 変更するときに注意するもの

| 項目 | 理由 |
|---|---|
| `Name` | Codexが機能接続に使う |
| イベントハンドラ | Codexが同じ処理へ接続する |
| DataGridView列の `Name` | データ表示と関係する |
| Services / Models | 保存や実処理の本体 |

`Name` を変更したい場合は、この文書と機能接続を同時に更新します。

### Codexが守ること

- Designerで作ったレイアウトを勝手に作り直さない
- `Text / Size / Location / Margin / Padding / Dock / Anchor` を不用意に上書きしない
- 同じ機能の複数入口へ、同じ処理を接続する
- ビジネスロジックを各ボタンへ重複実装しない
- 機能処理はPresenter / Serviceへ分離する
- UI変更が必要な場合は、理由と対象を先に報告する
- 未実装機能は `Enabled = false` または予定機能表示にする

---

# 2. 同じ機能に複数の入口を置くルール

同じ機能に、次の入口を置けます。

| 種類 | 命名規則 | 例 |
|---|---|---|
| 初心者向け文字ボタン | `<action>Button` | `addGroupButton` |
| コンパクトアイコンボタン | `<action>IconButton` | `addGroupIconButton` |
| ToolStripのアイコン | `<action>ToolStripButton` | `saveManualOrderToolStripButton` |
| MenuStrip | `<action>ToolStripMenuItem` | `addGroupToolStripMenuItem` |
| 右クリックメニュー | 用途が分かる名前 | `openContainingFolderToolStripMenuItem` |

例：グループ追加

```text
addGroupButton
addGroupIconButton
addGroupToolStripMenuItem
    ↓
同じ AddGroupRequested 処理
```

Codex側の接続イメージ：

```csharp
addGroupButton.Click += AddGroupRequested;
addGroupIconButton.Click += AddGroupRequested;
addGroupToolStripMenuItem.Click += AddGroupRequested;
```

実際の追加処理はPresenter / Serviceに1つだけ作ります。

---

# 3. 表示モード契約

候補設定名：

```csharp
public enum ActionButtonDisplayMode
{
    BeginnerText,
    CompactIcon
}
```

## BeginnerText

- 文字つきボタンを表示する
- コンパクトアイコンボタンは非表示にする
- MenuStripは表示したまま

## CompactIcon

- 文字つきボタンを非表示にする
- コンパクトアイコンボタンを表示する
- MenuStripは表示したまま
- 各アイコンへToolTipとAccessibleNameを設定する

## 配置について

コンパクトアイコンは一つのPanelへまとめる必要はありません。

例：

- `addGroupIconButton` はグループ一覧の近く
- 追加系アイコンは項目一覧の近く
- 並び替えアイコンは並び方の近く
- 設定アイコンは画面端や見出し付近

配置は少佐が決めます。Codexは `Name` と表示切替だけを利用します。

---

# 4. 共通コンポーネント

| Name | 種類 | 用途 |
|---|---|---|
| `mainMenuStrip` | `MenuStrip` | 上部メニューバー |
| `groupListBox` | `ListBox` | グループ一覧 |
| `itemGridView` | `DataGridView` | 選択グループの項目一覧 |
| `statusLabel` | `Label` | 操作結果、保存状態、エラー概要 |
| `actionToolTip` | `ToolTip` | アイコンボタンの説明 |
| `actionHoverDescriptionLabel` | `Label` | ホバー中の操作説明。任意 |

配置用コンテナの候補：

| Name | 種類 | 用途 | 必須 |
|---|---|---|---|
| `groupHeaderPanel` | `Panel` | グループ見出しと操作を置く | 任意 |
| `itemHeaderPanel` | `Panel` | 項目見出しと操作を置く | 任意 |
| `beginnerActionPanel` | `Panel` / `FlowLayoutPanel` | 文字ボタンをまとめる | 任意 |
| `compactActionPanel` | `Panel` / `FlowLayoutPanel` | アイコンをまとめる場合 | 任意 |
| `compactActionToolStrip` | `ToolStrip` | ToolStrip形式のアイコン操作 | 任意 |
| `orderActionPanel` | `Panel` | 並び替え操作をまとめる | 任意 |

コンテナの有無や配置は機能契約ではありません。

---

# 5. 基本操作の入口契約

## 5.1 登録

| 機能 | 文字ボタン | アイコンボタン | MenuStrip |
|---|---|---|---|
| グループ追加 | `addGroupButton` | `addGroupIconButton` | `addGroupToolStripMenuItem` |
| ファイル追加 | `addFileButton` | `addFileIconButton` | `addFileToolStripMenuItem` |
| フォルダー追加 | `addFolderButton` | `addFolderIconButton` | `addFolderToolStripMenuItem` |
| URL追加 | `addUrlButton` | `addUrlIconButton` | `addUrlToolStripMenuItem` |
| テンプレート追加 | `addTemplateButton` | `addTemplateIconButton` | `addTemplateToolStripMenuItem` |

## 5.2 選択項目の操作

| 機能 | 文字ボタン | アイコンボタン | MenuStrip |
|---|---|---|---|
| 開く | `openButton` | `openIconButton` | `openSelectedToolStripMenuItem` |
| コピー | `copyButton` | `copyIconButton` | `copySelectedToolStripMenuItem` |
| 編集 | `editButton` | `editIconButton` | `editSelectedToolStripMenuItem` |
| 詳細 | `detailButton` | `detailIconButton` | `detailSelectedToolStripMenuItem` |
| 登録をごみ箱へ | `deleteButton` | `deleteIconButton` | `deleteSelectedToolStripMenuItem` |

`deleteButton` は既存接続との互換性のため `Name` を維持します。画面上の `Text` は「登録をごみ箱へ」など、元ファイルを消さないことが分かる文言にして構いません。

## 5.3 ツール・管理

| 機能 | 文字ボタン | アイコンボタン | MenuStrip |
|---|---|---|---|
| 設定 | `settingsButton` | `settingsIconButton` | `settingsToolStripMenuItem` |
| 登録のごみ箱 | `trashButton` | `trashIconButton` | `trashToolStripMenuItem` |
| 登録内容を読み込む | `importButton` | `importIconButton` | `importToolStripMenuItem` |
| 登録内容を書き出す | `exportButton` | `exportIconButton` | `exportToolStripMenuItem` |
| バックアップ | `backupButton` | `backupIconButton` | `backupToolStripMenuItem` |

アイコン版が不要な機能は、少佐の判断で配置しなくて構いません。

---

# 6. 検索・絞り込み契約

| Name | 種類 | 用途 |
|---|---|---|
| `searchTextBox` | `TextBox` | 検索キーワード |
| `searchButton` | `Button` | 文字つき検索実行 |
| `searchIconButton` | `Button` | アイコン検索実行。任意 |
| `clearSearchButton` | `Button` | 文字つき検索解除 |
| `clearSearchIconButton` | `Button` | アイコン検索解除。任意 |
| `searchScopeComboBox` | `ComboBox` | 現在のグループ / 全体 |
| `typeFilterComboBox` | `ComboBox` | 種類フィルター |

検索候補：

```text
すべて / フォルダー / ファイル / 画像 / 動画 / URL / テンプレート
```

---

# 7. 並び替え・ソート契約

## 7.1 基本コントロール

| 機能 | 文字ボタン | アイコンボタン | ToolStrip / MenuStrip |
|---|---|---|---|
| 上へ移動 | `moveUpButton` | `moveUpIconButton` | `moveUpToolStripButton` / `moveUpToolStripMenuItem` |
| 下へ移動 | `moveDownButton` | `moveDownIconButton` | `moveDownToolStripButton` / `moveDownToolStripMenuItem` |
| 並び順を保存 | `saveManualOrderButton` | `saveManualOrderIconButton` | `saveManualOrderToolStripButton` / `saveManualOrderToolStripMenuItem` |
| 保存前に戻す | `revertUnsavedOrderButton` | `revertUnsavedOrderIconButton` | `revertUnsavedOrderToolStripButton` / `revertUnsavedOrderToolStripMenuItem` |

共通：

| Name | 種類 | 用途 |
|---|---|---|
| `sortModeComboBox` | `ComboBox` | 手動 / 名前 / 種類 / 追加 / 更新順 |
| `orderUnsavedStatusLabel` | `Label` | 未保存変更がある時だけ表示 |
| `sortSelectedButton` | `Button` | 選択範囲だけ並び替え。任意 |
| `sortSelectedModeComboBox` | `ComboBox` | 選択範囲ソート方式。任意 |
| `restorePreviousManualOrderButton` | `Button` | 一つ前の保存順へ戻す。将来候補 |
| `undoOrderMoveButton` | `Button` | 直前の移動を戻す。将来候補 |

## 7.2 状態ルール

- 右ドラッグ、上下移動、選択範囲ソートは編集状態へ反映する
- 移動しただけでは `SortOrder` を正式保存しない
- `saveManualOrderButton` で正式保存する
- `revertUnsavedOrderButton` で最後に保存した手動順へ戻す
- `orderUnsavedStatusLabel` は未保存変更がある時だけ表示する
- 名前順などの表示用ソートは保存済み手動順を破壊しない

## 7.3 未保存確認

設定候補：

```csharp
public enum UnsavedOrderBehavior
{
    Ask,
    AutoSave,
    Discard
}
```

確認対象：

- アプリ終了
- 保存内容の再読み込み
- インポートによる置き換え
- 対象グループ削除
- 初期化

確認文例：

```text
並び順に保存していない変更があります。

[保存して続ける]
[変更を破棄して続ける]
[キャンセル]

□ 次回からこの選択を自動的に適用する
```

---

# 8. MenuStrip契約

## ファイル

| Name | 用途 |
|---|---|
| `fileToolStripMenuItem` | ファイルメニュー |
| `exportToolStripMenuItem` | 登録内容を書き出す |
| `importToolStripMenuItem` | 登録内容を読み込む |
| `exitToolStripMenuItem` | 終了 |

## 登録

| Name | 用途 |
|---|---|
| `registerToolStripMenuItem` | 登録メニュー |
| `addGroupToolStripMenuItem` | グループ追加 |
| `addFileToolStripMenuItem` | ファイル追加 |
| `addFolderToolStripMenuItem` | フォルダー追加 |
| `addUrlToolStripMenuItem` | URL追加 |
| `addTemplateToolStripMenuItem` | テンプレート追加 |

## 編集

| Name | 用途 |
|---|---|
| `editToolStripMenuItem` | 編集メニュー |
| `openSelectedToolStripMenuItem` | 開く |
| `copySelectedToolStripMenuItem` | コピー |
| `editSelectedToolStripMenuItem` | 編集 |
| `detailSelectedToolStripMenuItem` | 詳細 |
| `deleteSelectedToolStripMenuItem` | 登録をごみ箱へ |
| `undoToolStripMenuItem` | 元に戻す |
| `moveUpToolStripMenuItem` | 上へ移動 |
| `moveDownToolStripMenuItem` | 下へ移動 |
| `sortSelectedToolStripMenuItem` | 選択範囲ソート |
| `saveManualOrderToolStripMenuItem` | 並び順を保存 |
| `revertUnsavedOrderToolStripMenuItem` | 保存前に戻す |

## 表示

| Name | 用途 |
|---|---|
| `viewToolStripMenuItem` | 表示メニュー |
| `showBeginnerHintsToolStripMenuItem` | 使い方のヒント |
| `showIconMeaningToolStripMenuItem` | アイコンの意味 |
| `actionButtonDisplayModeToolStripMenuItem` | 操作ボタン表示の親項目 |
| `beginnerActionButtonsToolStripMenuItem` | 初心者向け文字表示 |
| `compactIconButtonsToolStripMenuItem` | コンパクトアイコン表示 |
| `showThumbnailsToolStripMenuItem` | サムネイル表示 |
| `sortModeToolStripMenuItem` | 並び方の親項目 |
| `manualOrderToolStripMenuItem` | 手動並び順 |
| `sortByNameToolStripMenuItem` | 名前順 |
| `sortByTypeToolStripMenuItem` | 種類順 |
| `sortByCreatedAtToolStripMenuItem` | 追加順 |
| `sortByUpdatedAtToolStripMenuItem` | 更新順 |

## ツール・ヘルプ

| Name | 用途 |
|---|---|
| `toolsToolStripMenuItem` | ツールメニュー |
| `settingsToolStripMenuItem` | 設定 |
| `trashToolStripMenuItem` | 登録のごみ箱 |
| `backupToolStripMenuItem` | バックアップ |
| `repairShortcutsToolStripMenuItem` | ショートカット修復 |
| `helpToolStripMenuItem` | ヘルプメニュー |
| `usageGuideToolStripMenuItem` | 使い方 |
| `aboutToolStripMenuItem` | バージョン情報 |

---

# 9. DataGridView列契約

| Name | 種類の目安 | 用途 |
|---|---|---|
| `itemIconColumn` | `DataGridViewImageColumn` | 種類アイコン |
| `itemTypeColumn` | `DataGridViewTextBoxColumn` | 種類文字 |
| `itemTitleColumn` | `DataGridViewTextBoxColumn` | タイトル |
| `itemReferenceColumn` | `DataGridViewTextBoxColumn` | パス、URL、テンプレート概要 |
| `itemStatusColumn` | `DataGridViewTextBoxColumn` | 存在状態など |
| `itemThumbnailColumn` | `DataGridViewImageColumn` | 画像・動画サムネイル |

列の順序、幅、表示・非表示は少佐が調整できます。

---

# 10. 右クリックメニュー契約

| Name | 用途 |
|---|---|
| `itemContextMenuStrip` | 項目右クリックメニュー |
| `openToolStripMenuItem` | 開く |
| `copyToolStripMenuItem` | コピー |
| `editToolStripMenuItem` | 編集 |
| `detailToolStripMenuItem` | 詳細 |
| `deleteToolStripMenuItem` | 登録をごみ箱へ |
| `openContainingFolderToolStripMenuItem` | 置いてあるフォルダーを開く |
| `copyTitleToolStripMenuItem` | タイトルをコピー |

右クリックして動かさず離した場合はメニュー、右ボタンでドラッグした場合は並び替えとして扱います。

---

# 11. ToolTipとアクセシビリティ

アイコンだけの操作には、次を設定します。

- `ToolTip on actionToolTip`
- `AccessibleName`
- 必要に応じて `AccessibleDescription`

例：

```text
Name: addGroupIconButton
ToolTip: グループ追加
AccessibleName: グループ追加
AccessibleDescription: 新しいグループを作成します
```

アイコンの絵だけで意味を判断させないことを要件とします。

---

# 12. SettingsForm契約

## 表示

| Name | 用途 |
|---|---|
| `typeDisplayModeComboBox` | 種類表示方法 |
| `settingsShowBeginnerHintsCheckBox` | ヒント表示 |
| `settingsShowIconMeaningCheckBox` | アイコン説明表示 |
| `actionButtonDisplayModeComboBox` | 初心者向け / コンパクト切替 |
| `itemVisualModeComboBox` | 一覧 / サムネイル切替 |
| `showThumbnailsCheckBox` | サムネイル表示を使う |
| `thumbnailSizeComboBox` | サムネイルサイズ |

## D&D・並び順

| Name | 用途 |
|---|---|
| `confirmTitleOnDropAddCheckBox` | D&D追加時タイトル確認 |
| `enableGroupDropModifierShortcutsCheckBox` | Ctrl / Shiftショートカット |
| `confirmGroupDropCopyMoveCheckBox` | グループドロップ確認 |
| `enableItemDragReorderCheckBox` | D&D並び替えを使う |
| `manualReorderInputModeComboBox` | 右ドラッグのみ / 左右許可 |
| `defaultItemSortModeComboBox` | 既定の並び方 |
| `unsavedOrderBehaviorComboBox` | 毎回確認 / 自動保存 / 自動破棄 |

## バックアップ・削除

| Name | 用途 |
|---|---|
| `autoBackupEnabledCheckBox` | 自動バックアップ |
| `maxBackupCountNumericUpDown` | バックアップ保持数 |
| `confirmBeforeDeleteCheckBox` | 削除前確認 |
| `moveDeletedItemsToTrashCheckBox` | 登録のごみ箱へ移動 |
| `trashRetentionAutoRadioButton` | 指定日数後に自動削除 |
| `trashRetentionManualOnlyRadioButton` | 手動削除のみ |
| `trashRetentionDaysNumericUpDown` | 保持日数。既定30日 |
| `trashRetentionExplanationLabel` | 元ファイルは削除しない説明 |
| `emptyTrashButton` | ごみ箱内の登録情報をすべて削除 |

## 常駐・起動

| Name | 用途 |
|---|---|
| `minimizeToTrayOnCloseCheckBox` | 閉じるボタンでタスクトレイへ |
| `autoStartWithWindowsCheckBox` | Windows起動時に自動起動 |
| `startMinimizedCheckBox` | 自動起動時に最小化 |
| `registerStartMenuButton` | スタートメニュー登録 |
| `unregisterStartMenuButton` | スタートメニュー登録解除 |
| `repairShortcutsButton` | ショートカット修復 |

---

# 13. RecycleBinForm契約

| Name | 用途 |
|---|---|
| `trashMeaningNoticeLabel` | 元ファイルは削除しない説明 |
| `trashGridView` | 登録のごみ箱一覧 |
| `restoreRegistrationButton` | 登録を元に戻す |
| `removeRegistrationFromTrashButton` | ごみ箱から登録を削除 |
| `emptyRegistrationTrashButton` | 登録情報をすべて削除 |
| `closeButton` | 閉じる |

`trashGridView` に欲しい列：

- 種類
- タイトル
- 元のグループ
- 参照先または内容
- ごみ箱へ入れた日時
- 自動削除予定日

手動削除のみの場合は「手動で削除するまで保持」と表示します。

---

# 14. サムネイル契約

| Name | 用途 |
|---|---|
| `itemVisualModeComboBox` | 表示モード切替 |
| `showThumbnailsToolStripMenuItem` | MenuStripから切替 |
| `thumbnailSizeComboBox` | サムネイルサイズ |
| `thumbnailSizeTrackBar` | サイズ微調整。任意 |
| `thumbnailPreviewPanel` | 設定プレビュー。任意 |
| `itemThumbnailColumn` | 一覧内サムネイル |

- 画像を先に実装する
- 動画はWindows Shellサムネイルを優先する
- 読み込み失敗時は通常アイコンへ戻す
- キャッシュや非同期読み込みでUI停止を避ける

---

# 15. 少佐がDesignerで配置するときの確認

1. 見た目と場所を決める
2. 文字ボタンへ `<action>Button` のNameを付ける
3. アイコンボタンへ `<action>IconButton` のNameを付ける
4. MenuStrip項目へ契約名を付ける
5. アイコンだけの操作へToolTipとAccessibleNameを付ける
6. 未実装機能はDisabledにしておく
7. イベント処理はまだ書かない

---

# 16. Codexへ機能接続を依頼するときの文面

```text
Designerで配置済みのUIを作り直さず、docs/UI_CONTROL_CONTRACT.md のNameに接続してください。

- Text / Size / Location / Margin / Padding / Dock / Anchorを不用意に上書きしないでください。
- 文字ボタン、アイコンボタン、ToolStrip、MenuStripなど、同じ機能の複数入口は同じPresenter処理へ接続してください。
- 各入口へ処理を重複実装しないでください。
- 保存、ファイル操作、並び替え、ごみ箱処理はServiceへ分離してください。
- 未実装機能は、実装するまでDisabledのまま維持してください。
- UI変更が必要な場合は、変更前に理由と対象を報告してください。
```
