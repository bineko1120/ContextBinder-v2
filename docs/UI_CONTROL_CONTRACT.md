# ContextBinder v2 UI接続契約

この文書は、少佐がVisual Studio Designerで作ったUIへ、Codexが安全に機能を接続するための現在の契約です。

- 少佐は配置、親子関係、表示文言、操作導線を決める
- Codexは `Name`、`Tag`、表示モード契約に従って処理を接続する
- Beginner／CompactはMainForm内のPanelで分ける
- 表示モード分離のために新しいUserControlを導入しない
- 同じ機能の文字ボタン、アイコンボタン、MenuStripは同じPresenter処理を呼ぶ

画面構造は [`UI_SCREEN_MAP.md`](UI_SCREEN_MAP.md)、テーマ方針は [`UI_THEME_GUIDE.md`](UI_THEME_GUIDE.md) を参照してください。

---

# 1. `Name` と `Text` と `Tag`

| 項目 | 日本語での意味 | 用途 |
|---|---|---|
| `Name` | 内部名 | Codexが部品を識別し、機能を接続する |
| `Text` | 画面表示文字 | 少佐が日本語で自由に決める |
| `Tag` | 補助情報 | WinFormsUiViewerと実行時表示モードの識別に使う |

例：

```text
Name = addGroupButton
Text = グループを追加
Tag = view:Beginner
```

アイコン版：

```text
Name = addGroupIconButton
Text = ＋ または空欄
Tag = view:Compact
ToolTip = グループを追加
```

## 1.1 `Name` の読み方

英語名は、基本的に次の組み合わせです。

```text
動作 + 対象 + 部品種類
```

例：

```text
addGroupIconButton
```

```text
add        = 追加する
group      = グループ
IconButton = アイコンボタン
```

## 1.2 よく使う動作名

| 英語 | 日本語 |
|---|---|
| `add` | 追加する |
| `open` | 開く |
| `copy` | コピーする |
| `edit` | 編集する |
| `detail` | 詳細を表示する |
| `delete` | 登録をごみ箱へ移動する |
| `search` | 検索する |
| `clear` | 解除する |
| `toggle` | 開閉・切り替える |
| `moveUp` | 上へ移動する |
| `moveDown` | 下へ移動する |
| `save` | 保存する |
| `revert` | 保存前へ戻す |
| `restore` | 元に戻す |
| `import` | 登録内容を読み込む |
| `export` | 登録内容を書き出す |
| `show` | 表示する |
| `repair` | 修復する |

## 1.3 よく使う部品種類

| 英語 | 日本語 |
|---|---|
| `Panel` | 部品をまとめる領域 |
| `FlowPanel` | 自動で並べる領域 |
| `Button` | 文字つきボタン |
| `IconButton` | アイコンボタン |
| `ToolStripMenuItem` | メニュー項目 |
| `Label` | 文字表示 |
| `TextBox` | 文字入力欄 |
| `ComboBox` | 選択肢を開く欄 |
| `CheckBox` | ON/OFFまたは複数選択 |
| `ListBox` | 単純な一覧 |
| `GridView` | 表形式の一覧 |

---

# 2. DesignerとCodexの境界

## 2.1 少佐がDesignerで決めるもの

- `Name`
- 親コンテナ
- 子コントロールの順序
- 大まかな `Location`
- 大まかな `Size`
- `Dock`
- `Anchor`
- 表示文言
- Beginner／Compactの所属
- `Tag`

## 2.2 Codexが無断で変更しないもの

- `Name`
- 親コンテナ
- 子コントロールの順序
- `Dock`
- `Anchor`
- 表示モード用 `Tag`
- 少佐が確定した配置

正確なSize、Margin、Padding、フォント階層は、少佐の画面確認を前提に後から調整できます。

## 2.3 `.Designer.cs`

- 少佐は原則として直接編集しない
- Codexもテーマ処理やビジネスロジックを直接書かない
- 表示切替、機能接続、テーマ適用は通常の `.cs`、Presenter、Serviceへ分離する

---

# 3. Beginner／Compact表示Tag契約

## 3.1 正式Tag

| 表示対象 | 正式Tag |
|---|---|
| Beginner専用 | `view:Beginner` |
| Compact専用 | `view:Compact` |
| 両方に表示する共通部品 | 表示モード用Tagを付けない |

表記は大文字・小文字を含めて固定します。

## 3.2 WinFormsUiViewerプロファイル

| プロファイル | 対応Tag |
|---|---|
| `All` | フィルターなし |
| `Beginner` | `view:Beginner` |
| `Compact` | `view:Compact` |

WinFormsUiViewerは、共通部品を残したまま、プロファイルと一致しない表示専用コントロールを除外します。

## 3.3 Tagを付ける単位

### 領域全体が専用の場合

親PanelへTagを付けます。

```text
beginnerActionPanel
Tag = view:Beginner

itemCompactActionPanel
Tag = view:Compact
```

子ボタンへ同じTagを重複して付ける必要はありません。

### 共通Panel内で一部だけ切り替える場合

各コントロールへTagを付けます。

```text
addGroupButton
Tag = view:Beginner

addGroupIconButton
Tag = view:Compact
```

## 3.4 複数Tag

WinFormsUiViewerが複数Tagを扱う場合でも、現在の表示モード用Tagは1部品につき原則1つにします。

別用途のTagを併記する必要が出た場合は、セミコロン区切りを標準とします。

```text
view:Compact;state:Preview
```

Codexは既存Tag文字列を丸ごと上書きせず、必要な要素を解析・維持します。

## 3.5 実行時表示切替

WinFormsUiViewerのTagは設計確認用であると同時に、Codexが実行時表示切替を実装する際の分類基準です。

```text
BeginnerText
→ view:Beginnerを表示
→ view:Compactを非表示

CompactIcon
→ view:Beginnerを非表示
→ view:Compactを表示
```

`MenuStrip`、検索、一覧など共通部品は常に表示します。

---

# 4. MainFormコンテナ契約

| `Name` | 日本語での意味 | 種類 | 役割 | 表示Tag |
|---|---|---|---|---|
| `mainMenuStrip` | 上部メニュー | `MenuStrip` | ファイル、登録、編集、表示、ツール、ヘルプ | なし |
| `mainSplitContainer` | 左右分割 | `SplitContainer` | グループ領域と作業領域を分ける | なし |
| `groupHeaderPanel` | グループ見出し | `Panel` | 見出しと追加操作 | なし |
| `workAreaPanel` | 右側作業領域 | `Panel` | 項目領域と右側操作をまとめる | なし |
| `itemAreaPanel` | 中央項目領域 | `Panel` | 検索、一覧、下部情報 | なし |
| `rightActionHostPanel` | 右側操作の置き場所 | `Panel` | Beginner／Compact Panelを同じ位置に保持する | なし |
| `beginnerActionPanel` | 初心者向け操作 | `Panel` / `FlowLayoutPanel` | 文字つきボタンを縦に表示する | `view:Beginner` |
| `itemCompactActionPanel` | コンパクト操作 | `Panel` / `FlowLayoutPanel` | アイコン操作を右側へ縦に表示する | `view:Compact` |
| `bottomInformationPanel` | 下部情報 | `Panel` | 状態、ヒント、アイコン説明 | なし |

## 4.1 `rightActionHostPanel`

- Beginner／Compactの共通親
- 子Panelは同じ位置で `Dock = Fill` を基本とする
- Viewerと実行時は片方だけ表示する
- Beginner時とCompact時で親の幅を切り替える
- 幅の正確な値はUI完成後に決める

## 4.2 UserControl

Beginner／Compact表示分離のためにUserControlを使わないことを現在の契約とします。

試行中に作られたUserControlファイルは、別途整理指示が出るまでMainFormへ接続しません。

---

# 5. グループ領域契約

| 画面上の意味 | `Name` | 種類 | 機能 | Tag |
|---|---|---|---|---|
| グループ一覧見出し | `groupTitleLabel` | `Label` | 「グループ一覧」等を表示 | なし |
| グループ一覧 | `groupListBox` | `ListBox` | グループを表示・選択 | なし |
| グループ追加・文字 | `addGroupButton` | `Button` | Beginner向けグループ追加 | `view:Beginner` |
| グループ追加・アイコン | `addGroupIconButton` | `Button` | Compact向けグループ追加 | `view:Compact` |
| メニューから追加 | `addGroupToolStripMenuItem` | `ToolStripMenuItem` | MenuStripからグループ追加 | なし |

`addGroupIconButton`：

```text
ToolTip = グループを追加
AccessibleName = グループを追加
AccessibleDescription = 新しいグループを作成します
```

---

# 6. 検索フォーム契約

## 6.1 コンテナ

| `Name` | 日本語での意味 | 種類 | 機能 |
|---|---|---|---|
| `searchAndViewPanel` | 検索フォーム全体 | `Panel` | 見出しと開閉内容をまとめる |
| `searchHeaderPanel` | 常時表示見出し | `Panel` | 「検索フォーム」と開閉ボタン |
| `searchAndViewContentPanel` | 開閉する検索内容 | `Panel` | 検索入力、範囲、種類絞り込み |
| `searchInputPanel` | 検索入力行 | `TableLayoutPanel` | 入力欄、検索、解除を並べる |
| `searchScopePanel` | 検索範囲行 | `FlowLayoutPanel` | 範囲ラベルとComboBox |
| `typeFilterPanel` | 種類絞り込み領域 | `Panel` | 見出しとCheckBox群 |
| `typeFilterFlowPanel` | 種類CheckBox配置 | `FlowLayoutPanel` | 左寄せ、横並び、折り返し |

## 6.2 見出しと開閉

| 画面上の意味 | `Name` | 種類 | 機能 |
|---|---|---|---|
| 検索フォーム | `searchSectionTitleLabel` | `Label` | セクション名、絞り込み中表示 |
| 開く／閉じる | `toggleSearchPanelButton` | `Button` | 内容の表示・非表示を切り替える |

初期状態：

```text
searchAndViewContentPanel.Visible = false
```

Designerでは編集のため表示したままで構いません。実行時初期化で閉じます。

表示例：

```text
閉：検索フォーム [開く▼]
開：検索フォーム [閉じる▲]
```

## 6.3 検索入力

| 画面上の意味 | `Name` | 種類 | 機能 |
|---|---|---|---|
| 検索文字 | `searchTextBox` | `TextBox` | キーワード入力 |
| 検索実行 | `searchButton` | `Button` | 検索を実行 |
| 検索解除 | `clearSearchButton` | `Button` | キーワードと条件を解除 |

`searchInputPanel`では、検索入力欄を可変幅、検索・解除ボタンをAutoSizeとする構成を基本とします。

## 6.4 検索範囲

| 画面上の意味 | `Name` | 種類 | 機能 |
|---|---|---|---|
| 検索範囲ラベル | `searchScopeLabel` | `Label` | 「検索範囲」 |
| 検索範囲選択 | `searchScopeComboBox` | `ComboBox` | 現在のグループ／全体 |

候補：

```text
現在のグループ
すべてのグループ
```

## 6.5 種類絞り込み

種類絞り込みはComboBoxではなくCheckBox群を正式採用します。

| 表示文字 | `Name` | 種類 | 機能 |
|---|---|---|---|
| 絞り込み種類 | `typeFilterTitleLabel` | `Label` | 種類フィルター見出し |
| すべて | `allTypesCheckBox` | `CheckBox` | 全種類を一括選択・解除 |
| フォルダー | `folderTypeCheckBox` | `CheckBox` | フォルダーを対象にする |
| ファイル | `fileTypeCheckBox` | `CheckBox` | 一般ファイルを対象にする |
| 画像 | `imageTypeCheckBox` | `CheckBox` | 画像を対象にする |
| 動画 | `videoTypeCheckBox` | `CheckBox` | 動画を対象にする |
| URL | `urlTypeCheckBox` | `CheckBox` | URLを対象にする |
| テンプレート | `templateTypeCheckBox` | `CheckBox` | テンプレートを対象にする |

廃止予定：

```text
typeFilterComboBox
```

新規接続では使用しません。既存コードに残っている場合は、CheckBox群へ移行してから削除します。

### 絞り込みルール

- 個別種類はOR条件
- キーワード検索とはAND条件
- 「すべて」をチェックすると全個別種類をチェック
- 一部だけ選択中は「すべて」を中間状態にする案を採用可能
- 一つも選ばれていない場合は空状態または入力案内を表示

### レイアウト

```text
typeFilterFlowPanel
FlowDirection = LeftToRight
WrapContents = true
```

- 左寄せのまま使用する
- 横幅不足時は折り返す
- 折り返し時は親Panelと検索フォーム全体の高さを増やす

---

# 7. 項目追加入口契約

| 機能 | 文字ボタン | アイコンボタン | MenuStrip |
|---|---|---|---|
| ファイル追加 | `addFileButton` | `addFileIconButton` | `addFileToolStripMenuItem` |
| フォルダー追加 | `addFolderButton` | `addFolderIconButton` | `addFolderToolStripMenuItem` |
| URL追加 | `addUrlButton` | `addUrlIconButton` | `addUrlToolStripMenuItem` |
| テンプレート追加 | `addTemplateButton` | `addTemplateIconButton` | `addTemplateToolStripMenuItem` |

配置：

- 文字ボタンは `beginnerActionPanel` 内
- アイコンボタンは `itemCompactActionPanel` 内
- 親PanelにTagがあるため、子ボタンの表示Tagは原則不要
- MenuStripは共通

---

# 8. 選択項目操作契約

| 機能 | 文字ボタン | アイコンボタン | MenuStrip |
|---|---|---|---|
| 開く | `openButton` | `openIconButton` | `openSelectedToolStripMenuItem` |
| コピー | `copyButton` | `copyIconButton` | `copySelectedToolStripMenuItem` |
| 編集 | `editButton` | `editIconButton` | `editSelectedToolStripMenuItem` |
| 詳細 | `detailButton` | `detailIconButton` | `detailSelectedToolStripMenuItem` |
| 登録をごみ箱へ | `deleteButton` | `deleteIconButton` | `deleteSelectedToolStripMenuItem` |

`deleteButton`は既存互換のため内部名を維持します。画面表示は「登録をごみ箱へ」とし、元ファイルを削除しないことを明確にします。

右側操作の区画：

```text
追加操作
────────
選択項目操作
────────
危険操作
```

ごみ箱操作は他の操作から距離を取ります。

---

# 9. 並び替え契約

| 機能 | 文字ボタン | アイコンボタン | MenuStrip / ToolStrip |
|---|---|---|---|
| 上へ移動 | `moveUpButton` | `moveUpIconButton` | `moveUpToolStripMenuItem` / `moveUpToolStripButton` |
| 下へ移動 | `moveDownButton` | `moveDownIconButton` | `moveDownToolStripMenuItem` / `moveDownToolStripButton` |
| 並び順を保存 | `saveManualOrderButton` | `saveManualOrderIconButton` | `saveManualOrderToolStripMenuItem` / `saveManualOrderToolStripButton` |
| 保存前に戻す | `revertUnsavedOrderButton` | `revertUnsavedOrderIconButton` | `revertUnsavedOrderToolStripMenuItem` / `revertUnsavedOrderToolStripButton` |

共通部品：

| `Name` | 種類 | 機能 |
|---|---|---|
| `sortModeComboBox` | `ComboBox` | 手動、名前、種類、追加、更新順 |
| `orderUnsavedStatusLabel` | `Label` | 未保存変更がある時だけ表示 |
| `sortSelectedButton` | `Button` | 選択範囲だけを並び替える。任意 |
| `sortSelectedModeComboBox` | `ComboBox` | 選択範囲の並び方。任意 |

状態ルール：

- 右ドラッグ、上下移動、選択範囲ソートは作業中の手動順へ反映
- 移動しただけでは正式保存しない
- `saveManualOrderButton`で正式保存
- `revertUnsavedOrderButton`で最後の保存順へ戻す
- 名前順等の表示ソートは保存済み手動順を壊さない

未保存時の候補：

```csharp
public enum UnsavedOrderBehavior
{
    Ask,
    AutoSave,
    Discard
}
```

---

# 10. 項目一覧契約

| 日本語の列 | `Name` | 種類の目安 | 機能 |
|---|---|---|---|
| 種類アイコン | `itemIconColumn` | `DataGridViewImageColumn` | 種類アイコン |
| 種類 | `itemTypeColumn` | `DataGridViewTextBoxColumn` | フォルダー、画像、URL等 |
| タイトル | `itemTitleColumn` | `DataGridViewTextBoxColumn` | 登録名 |
| 内容・参照先 | `itemReferenceColumn` | `DataGridViewTextBoxColumn` | パス、URL、本文概要 |
| 状態 | `itemStatusColumn` | `DataGridViewTextBoxColumn` | 存在しないファイル等 |
| サムネイル | `itemThumbnailColumn` | `DataGridViewImageColumn` | 画像・動画プレビュー |

一覧本体：

```text
Name = itemGridView
種類 = DataGridView
```

列の順序、幅、表示・非表示は少佐が調整できます。

---

# 11. 下部情報契約

| 画面上の意味 | `Name` | 種類 | 機能 |
|---|---|---|---|
| 操作結果 | `statusLabel` | `Label` | コピー、保存、エラー等 |
| ヒント表示切替 | `showBeginnerHintsCheckBox` | `CheckBox` | ヒント表示・非表示 |
| 使い方のヒント | `beginnerHintsGroupBox` | `GroupBox` | 初心者向け説明 |
| アイコン説明切替 | `showIconMeaningCheckBox` | `CheckBox` | アイコン説明表示・非表示 |
| アイコンの意味 | `iconMeaningGroupBox` | `GroupBox` | 種類アイコンの説明 |
| アイコン説明配置先 | `iconMeaningPanel` | `Panel` / `FlowLayoutPanel` | 種類別説明を並べる |
| ホバー説明 | `actionHoverDescriptionLabel` | `Label` | アイコン操作の詳しい説明 |
| ToolTip | `actionToolTip` | `ToolTip` | アイコンへマウスを乗せた時の説明 |

---

# 12. MenuStrip契約

## ファイル

| `Name` | 日本語表示例 |
|---|---|
| `fileToolStripMenuItem` | ファイル |
| `exportToolStripMenuItem` | 登録内容を書き出す |
| `importToolStripMenuItem` | 登録内容を読み込む |
| `exitToolStripMenuItem` | 終了 |

## 登録

| `Name` | 日本語表示例 |
|---|---|
| `registerToolStripMenuItem` | 登録 |
| `addGroupToolStripMenuItem` | グループ追加 |
| `addFileToolStripMenuItem` | ファイル追加 |
| `addFolderToolStripMenuItem` | フォルダー追加 |
| `addUrlToolStripMenuItem` | URL追加 |
| `addTemplateToolStripMenuItem` | テンプレート追加 |

## 編集

| `Name` | 日本語表示例 |
|---|---|
| `editToolStripMenuItem` | 編集 |
| `openSelectedToolStripMenuItem` | 開く |
| `copySelectedToolStripMenuItem` | コピー |
| `editSelectedToolStripMenuItem` | 編集 |
| `detailSelectedToolStripMenuItem` | 詳細 |
| `deleteSelectedToolStripMenuItem` | 登録をごみ箱へ |
| `moveUpToolStripMenuItem` | 上へ移動 |
| `moveDownToolStripMenuItem` | 下へ移動 |
| `saveManualOrderToolStripMenuItem` | 並び順を保存 |
| `revertUnsavedOrderToolStripMenuItem` | 保存前に戻す |

## 表示

| `Name` | 日本語表示例 |
|---|---|
| `viewToolStripMenuItem` | 表示 |
| `showBeginnerHintsToolStripMenuItem` | 使い方のヒント |
| `showIconMeaningToolStripMenuItem` | アイコンの意味 |
| `actionButtonDisplayModeToolStripMenuItem` | 操作ボタン表示 |
| `beginnerActionButtonsToolStripMenuItem` | 初心者向け文字表示 |
| `compactIconButtonsToolStripMenuItem` | コンパクトアイコン表示 |
| `showThumbnailsToolStripMenuItem` | サムネイル表示 |
| `manualOrderToolStripMenuItem` | 手動並び順 |
| `sortByNameToolStripMenuItem` | 名前順 |
| `sortByTypeToolStripMenuItem` | 種類順 |
| `sortByCreatedAtToolStripMenuItem` | 追加順 |
| `sortByUpdatedAtToolStripMenuItem` | 更新順 |

## ツール・ヘルプ

| `Name` | 日本語表示例 |
|---|---|
| `toolsToolStripMenuItem` | ツール |
| `settingsToolStripMenuItem` | 設定 |
| `trashToolStripMenuItem` | 登録のごみ箱 |
| `backupToolStripMenuItem` | バックアップ |
| `repairShortcutsToolStripMenuItem` | ショートカット修復 |
| `helpToolStripMenuItem` | ヘルプ |
| `usageGuideToolStripMenuItem` | 使い方 |
| `aboutToolStripMenuItem` | バージョン情報 |

MenuStripはBeginner／Compact共通のため、表示モード用Tagを付けません。

---

# 13. 右クリックメニュー契約

| `Name` | 日本語表示例 |
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

# 14. ToolTip・アクセシビリティ契約

アイコンだけの操作には、次を設定します。

- `ToolTip on actionToolTip`
- `AccessibleName`
- 必要に応じて `AccessibleDescription`

例：

```text
Name = addFileIconButton
ToolTip = ファイルを追加
AccessibleName = ファイルを追加
AccessibleDescription = 現在のグループへファイルを登録します
```

画像や記号だけで意味を判断させません。

---

# 15. 表示モード設定契約

```csharp
public enum ActionButtonDisplayMode
{
    BeginnerText,
    CompactIcon
}
```

## `BeginnerText`

- `view:Beginner`を表示
- `view:Compact`を非表示
- `rightActionHostPanel`を文字ボタン向けの幅にする
- MenuStripと共通部品は表示

## `CompactIcon`

- `view:Beginner`を非表示
- `view:Compact`を表示
- `rightActionHostPanel`をコンパクト向けの幅にする
- MenuStripと共通部品は表示
- ToolTipとAccessibleNameを有効にする

Viewerプロファイルと実行時表示の分類は同じTagを基準にします。

---

# 16. 文字サイズ設定契約

```csharp
public enum UiFontSizeMode
{
    Small,
    Standard,
    Large
}
```

基準：

```text
Standard = Yu Gothic UI 10pt
```

文字サイズ変更時に調整できるもの：

- Font
- ボタン内Padding
- ボタン最小高さ
- 入力欄高さ
- DataGridView行高
- 見出し高さ
- MinimumSize

文字サイズ変更で変更しないもの：

- 親子関係
- 左・中央・右の構造
- 表示モードTag
- 機能の意味

---

# 17. SettingsForm契約

## 表示

| `Name` | 日本語での意味 |
|---|---|
| `typeDisplayModeComboBox` | 種類表示方法 |
| `settingsShowBeginnerHintsCheckBox` | ヒント表示 |
| `settingsShowIconMeaningCheckBox` | アイコン説明表示 |
| `actionButtonDisplayModeComboBox` | Beginner／Compact切替 |
| `uiFontSizeModeComboBox` | 小さめ／標準／大きめ |
| `itemVisualModeComboBox` | 一覧／サムネイル切替 |
| `showThumbnailsCheckBox` | サムネイル表示 |
| `thumbnailSizeComboBox` | サムネイルサイズ |

## D&D・並び順

| `Name` | 日本語での意味 |
|---|---|
| `confirmTitleOnDropAddCheckBox` | D&D追加時タイトル確認 |
| `confirmGroupDropCopyMoveCheckBox` | グループドロップ確認 |
| `enableItemDragReorderCheckBox` | D&D並び替え |
| `manualReorderInputModeComboBox` | 右ドラッグのみ／左右許可 |
| `defaultItemSortModeComboBox` | 既定の並び方 |
| `unsavedOrderBehaviorComboBox` | 毎回確認／自動保存／自動破棄 |

## バックアップ・削除

| `Name` | 日本語での意味 |
|---|---|
| `autoBackupEnabledCheckBox` | 自動バックアップ |
| `maxBackupCountNumericUpDown` | バックアップ保持数 |
| `confirmBeforeDeleteCheckBox` | 削除前確認 |
| `moveDeletedItemsToTrashCheckBox` | 登録のごみ箱へ移動 |
| `trashRetentionAutoRadioButton` | 指定日数後に自動削除 |
| `trashRetentionManualOnlyRadioButton` | 手動削除のみ |
| `trashRetentionDaysNumericUpDown` | 保持日数。既定30日 |
| `trashRetentionExplanationLabel` | 元ファイルは削除しない説明 |

---

# 18. RecycleBinForm契約

| 画面上の意味 | `Name` |
|---|---|
| 元ファイルは削除しない説明 | `trashMeaningNoticeLabel` |
| 登録のごみ箱一覧 | `trashGridView` |
| 登録を元に戻す | `restoreRegistrationButton` |
| ごみ箱から登録を削除 | `removeRegistrationFromTrashButton` |
| 登録情報をすべて削除 | `emptyRegistrationTrashButton` |
| 閉じる | `closeButton` |

常時表示する説明：

```text
ここで削除されるのは、ContextBinderへの登録情報だけです。
元のファイル、フォルダー、画像、動画は削除されません。
```

---

# 19. Designer作業確認

1. 共通部品に表示モード用Tagを付けていないか確認する
2. Beginner専用Panelへ `view:Beginner` を付ける
3. Compact専用Panelへ `view:Compact` を付ける
4. 共通Panel内の切替部品へ個別Tagを付ける
5. WinFormsUiViewerのAllで全体構造を確認する
6. Beginnerで文字ボタンだけが出ることを確認する
7. CompactでアイコンPanelだけが出ることを確認する
8. `Name`を契約表と照合する
9. イベント処理はまだ書かない

---

# 20. Codexへ機能接続を依頼するときの基準

```text
Designerで配置済みのMainFormを作り直さず、docs/UI_CONTROL_CONTRACT.md のNameとTagへ接続してください。

- Beginner／Compact表示分離のために新しいUserControlを導入しないでください。
- view:Beginner と view:Compact のTag契約を維持してください。
- WinFormsUiViewerのAll／Beginner／Compactプロファイルを壊さないでください。
- 同じ機能の文字ボタン、アイコンボタン、MenuStripは同じPresenter処理へ接続してください。
- 表示切替、検索、並び替え、保存、ごみ箱処理はPresenter／Serviceへ分離してください。
- Designer管理のName、親子関係、Dock、Anchorを無断で変更しないでください。
- 正確なSize、Margin、Paddingを変更する場合は、実画面と文字サイズ3段階で検証してください。
- UI変更が必要な場合は、変更前に理由と対象を報告してください。
```
