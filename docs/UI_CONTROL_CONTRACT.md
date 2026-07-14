# ContextBinder v2 UI接続契約

この文書は、少佐が Visual Studio Designer で作ったUIへ、Codexが安全に機能を接続するための契約です。

- 少佐は見た目、配置、色、余白、固定文言を決める
- Codexは `Name` 契約に従って処理を接続する
- 同じ機能に、文字ボタン・アイコンボタン・MenuStripなど複数の入口を置いてよい
- 複数の入口は、内部では同じPresenter処理を呼ぶ
- 英語の `Name` は内部名であり、画面表示は日本語で自由に決める

画面全体の考え方は [`UI_SCREEN_MAP.md`](UI_SCREEN_MAP.md) を参照してください。

---

# 1. `Name` の読み方

英語名は、基本的に次の組み合わせです。

```text
動作 + 対象 + 部品種類
```

例：

```text
addGroupIconButton
```

を分けると、

```text
add       = 追加する
group     = グループ
IconButton = アイコンボタン
```

つまり「グループ追加用のアイコンボタン」です。

## 1.1 よく使う動作名

| 英語 | 日本語 |
|---|---|
| `add` | 追加する |
| `open` | 開く |
| `copy` | コピーする |
| `edit` | 編集する |
| `detail` | 詳細を表示する |
| `delete` | 登録をごみ箱へ移動する |
| `search` | 検索する |
| `clear` | 解除・クリアする |
| `moveUp` | 上へ移動する |
| `moveDown` | 下へ移動する |
| `save` | 保存する |
| `revert` | 保存前へ戻す |
| `restore` | 元に戻す |
| `import` | 登録内容を読み込む |
| `export` | 登録内容を書き出す |
| `show` | 表示する |
| `hide` | 非表示にする |
| `repair` | 修復する |

## 1.2 よく使う対象名

| 英語 | 日本語 |
|---|---|
| `group` | グループ |
| `item` | 登録項目 |
| `file` | ファイル |
| `folder` | フォルダー |
| `url` | URL |
| `template` | テンプレート文 |
| `order` | 並び順 |
| `trash` | 登録のごみ箱 |
| `backup` | バックアップ |
| `settings` | 設定 |
| `thumbnail` | サムネイル |
| `status` | 状態・操作結果 |

## 1.3 よく使う部品種類

| 英語 | 日本語 |
|---|---|
| `Button` | 文字つきボタン |
| `IconButton` | アイコンボタン |
| `ToolStripButton` | ToolStrip内のアイコンボタン |
| `ToolStripMenuItem` | MenuStripや右クリック内のメニュー項目 |
| `Panel` | 部品をまとめる領域 |
| `Label` | 文字表示 |
| `TextBox` | 文字入力欄 |
| `ComboBox` | 選択肢を開く欄 |
| `ListBox` | 単純な一覧 |
| `GridView` | 表形式の一覧 |
| `CheckBox` | ON/OFF選択 |
| `RadioButton` | どれか1つを選択 |

---

# 2. 基本ルール

## 2.1 少佐が自由に変更してよいもの

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

## 2.2 変更するときに注意するもの

| 項目 | 理由 |
|---|---|
| `Name` | Codexが機能接続に使う |
| イベントハンドラ | Codexが同じ処理へ接続する |
| DataGridView列の `Name` | データ表示と関係する |
| Services / Models | 保存や実処理の本体 |

`Name` を変更したい場合は、この文書と機能接続を同時に更新します。

## 2.3 Codexが守ること

- Designerで作ったレイアウトを勝手に作り直さない
- `Text / Size / Location / Margin / Padding / Dock / Anchor` を不用意に上書きしない
- 同じ機能の複数入口へ、同じ処理を接続する
- ビジネスロジックを各ボタンへ重複実装しない
- 機能処理はPresenter / Serviceへ分離する
- UI変更が必要な場合は、理由と対象を先に報告する
- 未実装機能は `Enabled = false` または予定機能表示にする

---

# 3. 同じ機能に複数の入口を置くルール

| 入口の種類 | 命名規則 | 例 | 日本語での意味 |
|---|---|---|---|
| 初心者向け文字ボタン | `<action>Button` | `addGroupButton` | グループ追加の文字ボタン |
| コンパクトアイコンボタン | `<action>IconButton` | `addGroupIconButton` | グループ追加の＋アイコン |
| ToolStrip内アイコン | `<action>ToolStripButton` | `saveManualOrderToolStripButton` | ToolStrip内の並び順保存 |
| MenuStrip項目 | `<action>ToolStripMenuItem` | `addGroupToolStripMenuItem` | メニュー内のグループ追加 |
| 右クリック項目 | 用途が分かる名前 | `openContainingFolderToolStripMenuItem` | 置いてあるフォルダーを開く |

例：グループ追加

```text
addGroupButton
addGroupIconButton
addGroupToolStripMenuItem
    ↓
同じ「グループ追加」処理
```

Codex側では、すべて同じ処理へ接続します。

```csharp
addGroupButton.Click += AddGroupRequested;
addGroupIconButton.Click += AddGroupRequested;
addGroupToolStripMenuItem.Click += AddGroupRequested;
```

実際の追加処理はPresenter / Serviceに1つだけ作ります。

---

# 4. 表示モード契約

候補設定名：

```csharp
public enum ActionButtonDisplayMode
{
    BeginnerText,
    CompactIcon
}
```

## 4.1 `BeginnerText`：初心者向け文字表示

- 文字つきボタンを表示する
- コンパクトアイコンボタンは非表示にする
- `MenuStrip` は表示したまま

## 4.2 `CompactIcon`：コンパクトアイコン表示

- 文字つきボタンを非表示にする
- コンパクトアイコンボタンを表示する
- アイコン自体は常に表示する
- マウスを乗せた時だけToolTipや説明文を表示する
- `MenuStrip` は表示したまま
- 各アイコンへ `ToolTip` と `AccessibleName` を設定する

## 4.3 配置について

コンパクトアイコンは一つのPanelへまとめる必要はありません。

例：

- `addGroupIconButton` はグループ一覧の近く
- 追加系アイコンは項目一覧の近く
- 並び替えアイコンは並び方の近く
- 設定アイコンは画面端や見出し付近

配置は少佐が決めます。Codexは `Name` と表示切替だけを利用します。

---

# 5. 配置用コンテナ契約

配置用コンテナは、処理そのものではなく、部品をまとめる箱です。

## 5.1 推奨する大きなコンテナ

| `Name` | 日本語での意味 | 種類候補 | 役割 | 参考Dock | 必須度 |
|---|---|---|---|---|---|
| `mainSplitContainer` | 左右分割の大枠 | `SplitContainer` | 左をグループ、右を項目・操作に分ける | `Fill` | 推奨 |
| `groupAreaPanel` | グループ領域全体 | `Panel` | 見出しとグループ一覧をまとめる | `Fill` | 推奨 |
| `groupHeaderPanel` | グループ見出し領域 | `Panel` / `TableLayoutPanel` | 「グループ」と＋ボタンを置く | `Top` | 推奨 |
| `workAreaPanel` | 右側作業領域全体 | `Panel` | 項目領域と初心者向け操作をまとめる | `Fill` | 推奨 |
| `beginnerActionPanel` | 初心者向け文字操作領域 | `Panel` / `FlowLayoutPanel` | 文字つき操作ボタンをまとめる | `Right`参考 | 推奨 |
| `itemAreaPanel` | 項目一覧側の領域 | `Panel` | 検索、一覧、並び替え、下部情報をまとめる | `Fill` | 推奨 |
| `searchAndViewPanel` | 検索・表示操作領域 | `Panel` | 検索、種類、並び方、表示方法 | `Top` | 推奨 |
| `itemToolbarPanel` | 項目操作ツール領域 | `Panel` / `FlowLayoutPanel` / `ToolStrip` | 追加、開く、コピー、編集、並び替え | `Top` | 推奨 |
| `bottomInformationPanel` | 下部情報領域 | `Panel` | ステータス、ヒント、アイコン説明 | `Bottom` | 推奨 |

`bottomInformationPanel` は `itemAreaPanel` 内へ置く参考案です。中央の下だけに表示でき、左のグループ領域や右の初心者操作領域へ干渉しません。

## 5.2 コンパクト操作をまとめる小さなコンテナ

| `Name` | 日本語での意味 | 中へ置く例 | 必須度 |
|---|---|---|---|
| `groupCompactActionPanel` | グループ用アイコン領域 | `addGroupIconButton` | 任意 |
| `itemCompactActionPanel` | 項目操作アイコン領域 | 追加、開く、コピー、編集、ごみ箱 | 任意 |
| `orderCompactActionPanel` | 並び替えアイコン領域 | 上、下、保存、保存前に戻す | 任意 |

これらがなくても、アイコンボタンを `groupHeaderPanel` や `itemToolbarPanel` に直接置けます。

---

# 6. 共通コンポーネント

| `Name` | 日本語での意味 | 種類 | 機能 | 必須度 |
|---|---|---|---|---|
| `mainMenuStrip` | 上部メニューバー | `MenuStrip` | ファイル、登録、編集、表示、ツール、ヘルプ | 推奨 |
| `groupListBox` | グループ一覧 | `ListBox` | グループを表示・選択する | 必須 |
| `itemGridView` | 登録項目一覧 | `DataGridView` | 選択グループの項目を表形式で表示する | 必須 |
| `statusLabel` | 操作結果表示 | `Label` | 保存、コピー、エラーなどを表示する | 必須 |
| `actionToolTip` | アイコン説明 | `ToolTip` | アイコンへマウスを乗せた時に説明する | コンパクト表示で必須 |
| `actionHoverDescriptionLabel` | ホバー説明欄 | `Label` | アイコンの詳しい説明を画面上に表示する | 任意 |

---

# 7. グループ領域の契約

| 画面上の意味 | `Name` | 種類 | 機能 | 表示例 | 必須度 |
|---|---|---|---|---|---|
| 「グループ」見出し | `groupTitleLabel` | `Label` | 領域名を表示する | グループ | 任意 |
| グループ一覧 | `groupListBox` | `ListBox` | グループを表示・選択する | TRPG、動画編集 | 必須 |
| グループ追加・文字 | `addGroupButton` | `Button` | 初心者向けグループ追加 | グループ追加 | 初心者表示で推奨 |
| グループ追加・アイコン | `addGroupIconButton` | `Button` | コンパクト表示のグループ追加 | ＋ | コンパクト表示で推奨 |
| グループ追加・メニュー | `addGroupToolStripMenuItem` | `ToolStripMenuItem` | メニューからグループ追加 | 登録 > グループ追加 | 推奨 |

`addGroupIconButton` の推奨補助：

```text
ToolTip：グループ追加
AccessibleName：グループ追加
AccessibleDescription：新しいグループを作成します
```

---

# 8. 項目追加の入口契約

| 機能 | 日本語での意味 | 文字ボタン | アイコンボタン | MenuStrip |
|---|---|---|---|---|
| ファイル追加 | ファイルを現在のグループへ登録する | `addFileButton` | `addFileIconButton` | `addFileToolStripMenuItem` |
| フォルダー追加 | フォルダーを登録する | `addFolderButton` | `addFolderIconButton` | `addFolderToolStripMenuItem` |
| URL追加 | URLを登録する | `addUrlButton` | `addUrlIconButton` | `addUrlToolStripMenuItem` |
| テンプレート追加 | 定型文を登録する | `addTemplateButton` | `addTemplateIconButton` | `addTemplateToolStripMenuItem` |

アイコン例は固定ではありません。少佐が分かりやすい絵へ変更できます。

---

# 9. 選択項目の操作契約

| 機能 | 日本語での意味 | 文字ボタン | アイコンボタン | MenuStrip |
|---|---|---|---|---|
| 開く | ファイル、フォルダー、URLを開く | `openButton` | `openIconButton` | `openSelectedToolStripMenuItem` |
| コピー | パス、URL、本文をコピーする | `copyButton` | `copyIconButton` | `copySelectedToolStripMenuItem` |
| 編集 | タイトルや参照先を変更する | `editButton` | `editIconButton` | `editSelectedToolStripMenuItem` |
| 詳細 | 登録内容を変更せず確認する | `detailButton` | `detailIconButton` | `detailSelectedToolStripMenuItem` |
| 登録をごみ箱へ | ContextBinderの登録情報だけをごみ箱へ移す | `deleteButton` | `deleteIconButton` | `deleteSelectedToolStripMenuItem` |

`deleteButton` は既存接続との互換性のため `Name` を維持します。画面上の `Text` は「登録をごみ箱へ」など、元ファイルを消さないと分かる文言にします。

---

# 10. 検索・絞り込み契約

| 画面上の意味 | `Name` | 種類 | 機能 | 必須度 |
|---|---|---|---|---|
| 検索キーワード | `searchTextBox` | `TextBox` | タイトル、パス、URL、本文などを入力する | 推奨 |
| 検索実行・文字 | `searchButton` | `Button` | 検索を実行する | 任意 |
| 検索実行・アイコン | `searchIconButton` | `Button` | コンパクト表示で検索する | 任意 |
| 検索解除・文字 | `clearSearchButton` | `Button` | 検索条件を消す | 推奨 |
| 検索解除・アイコン | `clearSearchIconButton` | `Button` | コンパクト表示で検索解除する | 任意 |
| 検索範囲 | `searchScopeComboBox` | `ComboBox` | 現在のグループ / 全体を選ぶ | 推奨 |
| 種類絞り込み | `typeFilterComboBox` | `ComboBox` | フォルダー、画像、URLなどで絞る | 推奨 |

種類候補：

```text
すべて / フォルダー / ファイル / 画像 / 動画 / URL / テンプレート
```

---

# 11. 並び替え・ソート契約

## 11.1 基本コントロール

| 機能 | 日本語での意味 | 文字ボタン | アイコンボタン | ToolStrip / MenuStrip |
|---|---|---|---|---|
| 上へ移動 | 選択項目を1段上へ移動する | `moveUpButton` | `moveUpIconButton` | `moveUpToolStripButton` / `moveUpToolStripMenuItem` |
| 下へ移動 | 選択項目を1段下へ移動する | `moveDownButton` | `moveDownIconButton` | `moveDownToolStripButton` / `moveDownToolStripMenuItem` |
| 並び順を保存 | 編集中の並び順を正式保存する | `saveManualOrderButton` | `saveManualOrderIconButton` | `saveManualOrderToolStripButton` / `saveManualOrderToolStripMenuItem` |
| 保存前に戻す | 未保存変更を捨て、最後の保存順へ戻す | `revertUnsavedOrderButton` | `revertUnsavedOrderIconButton` | `revertUnsavedOrderToolStripButton` / `revertUnsavedOrderToolStripMenuItem` |

共通部品：

| 画面上の意味 | `Name` | 種類 | 機能 | 必須度 |
|---|---|---|---|---|
| 並び方選択 | `sortModeComboBox` | `ComboBox` | 手動、名前、種類、追加、更新順を選ぶ | 推奨 |
| 未保存表示 | `orderUnsavedStatusLabel` | `Label` | 未保存変更がある時だけ表示する | 推奨 |
| 選択範囲ソート | `sortSelectedButton` | `Button` | 選択項目だけ指定順で並べる | 任意 |
| 選択範囲ソート方式 | `sortSelectedModeComboBox` | `ComboBox` | 名前順、種類順などを選ぶ | 任意 |
| 前の保存順へ戻す | `restorePreviousManualOrderButton` | `Button` | 一つ前に保存した手動順へ戻す | 将来候補 |
| 直前移動を戻す | `undoOrderMoveButton` | `Button` | 直前の並び替えだけ戻す | 将来候補 |

## 11.2 状態ルール

- 右ドラッグ、上下移動、選択範囲ソートは編集状態へ反映する
- 移動しただけでは `SortOrder` を正式保存しない
- `saveManualOrderButton` で正式保存する
- `revertUnsavedOrderButton` で最後に保存した手動順へ戻す
- `orderUnsavedStatusLabel` は未保存変更がある時だけ表示する
- 名前順などの表示用ソートは保存済み手動順を破壊しない

## 11.3 未保存確認

設定候補：

```csharp
public enum UnsavedOrderBehavior
{
    Ask,
    AutoSave,
    Discard
}
```

日本語での意味：

| 値 | 日本語 |
|---|---|
| `Ask` | 毎回確認する |
| `AutoSave` | 自動的に保存する |
| `Discard` | 自動的に破棄する |

確認対象：

- アプリ終了
- 保存内容の再読み込み
- インポートによる置き換え
- 対象グループ削除
- 初期化

---

# 12. 下部情報契約

| 画面上の意味 | `Name` | 種類 | 機能 | 必須度 |
|---|---|---|---|---|
| 操作結果 | `statusLabel` | `Label` | コピー、保存、エラーなどを表示する | 必須 |
| ヒント表示切替 | `showBeginnerHintsCheckBox` | `CheckBox` | 使い方のヒントを表示・非表示にする | 推奨 |
| 使い方のヒント | `beginnerHintsGroupBox` | `GroupBox` | 初心者向け説明を表示する | 推奨 |
| アイコン説明切替 | `showIconMeaningCheckBox` | `CheckBox` | アイコン説明を表示・非表示にする | 推奨 |
| アイコンの意味 | `iconMeaningGroupBox` | `GroupBox` | 猫アイコン等の説明を表示する | 推奨 |
| アイコン説明配置先 | `iconMeaningPanel` | `Panel` / `FlowLayoutPanel` | 種類別説明を並べる | 推奨 |
| ホバー説明 | `actionHoverDescriptionLabel` | `Label` | アイコンにマウスを乗せた時の詳しい説明 | 任意 |

---

# 13. MenuStrip契約

## 13.1 ファイル

| `Name` | 日本語表示例 | 機能 |
|---|---|---|
| `fileToolStripMenuItem` | ファイル | ファイルメニューの親 |
| `exportToolStripMenuItem` | 登録内容を書き出す | エクスポート |
| `importToolStripMenuItem` | 登録内容を読み込む | インポート |
| `exitToolStripMenuItem` | 終了 | アプリ終了 |

## 13.2 登録

| `Name` | 日本語表示例 | 機能 |
|---|---|---|
| `registerToolStripMenuItem` | 登録 | 登録メニューの親 |
| `addGroupToolStripMenuItem` | グループ追加 | グループ追加 |
| `addFileToolStripMenuItem` | ファイル追加 | ファイル追加 |
| `addFolderToolStripMenuItem` | フォルダー追加 | フォルダー追加 |
| `addUrlToolStripMenuItem` | URL追加 | URL追加 |
| `addTemplateToolStripMenuItem` | テンプレート追加 | テンプレート追加 |

## 13.3 編集

| `Name` | 日本語表示例 | 機能 |
|---|---|---|
| `editToolStripMenuItem` | 編集 | 編集メニューの親 |
| `openSelectedToolStripMenuItem` | 開く | 選択項目を開く |
| `copySelectedToolStripMenuItem` | コピー | 選択項目をコピー |
| `editSelectedToolStripMenuItem` | 編集 | 選択項目を編集 |
| `detailSelectedToolStripMenuItem` | 詳細 | 詳細表示 |
| `deleteSelectedToolStripMenuItem` | 登録をごみ箱へ | 登録情報を移動 |
| `undoToolStripMenuItem` | 元に戻す | 直前操作を戻す |
| `moveUpToolStripMenuItem` | 上へ移動 | 上へ移動 |
| `moveDownToolStripMenuItem` | 下へ移動 | 下へ移動 |
| `sortSelectedToolStripMenuItem` | 選択範囲を並び替え | 選択範囲ソート |
| `saveManualOrderToolStripMenuItem` | 並び順を保存 | 並び順確定 |
| `revertUnsavedOrderToolStripMenuItem` | 保存前に戻す | 未保存変更破棄 |

## 13.4 表示

| `Name` | 日本語表示例 | 機能 |
|---|---|---|
| `viewToolStripMenuItem` | 表示 | 表示メニューの親 |
| `showBeginnerHintsToolStripMenuItem` | 使い方のヒント | ヒント表示切替 |
| `showIconMeaningToolStripMenuItem` | アイコンの意味 | アイコン説明切替 |
| `actionButtonDisplayModeToolStripMenuItem` | 操作ボタン表示 | 表示モードの親 |
| `beginnerActionButtonsToolStripMenuItem` | 初心者向け文字表示 | 文字ボタン表示 |
| `compactIconButtonsToolStripMenuItem` | コンパクトアイコン表示 | アイコン表示 |
| `showThumbnailsToolStripMenuItem` | サムネイル表示 | サムネイル切替 |
| `sortModeToolStripMenuItem` | 並び方 | 並び方の親 |
| `manualOrderToolStripMenuItem` | 手動並び順 | 手動順表示 |
| `sortByNameToolStripMenuItem` | 名前順 | 名前順表示 |
| `sortByTypeToolStripMenuItem` | 種類順 | 種類順表示 |
| `sortByCreatedAtToolStripMenuItem` | 追加順 | 追加順表示 |
| `sortByUpdatedAtToolStripMenuItem` | 更新順 | 更新順表示 |

## 13.5 ツール・ヘルプ

| `Name` | 日本語表示例 | 機能 |
|---|---|---|
| `toolsToolStripMenuItem` | ツール | ツールメニューの親 |
| `settingsToolStripMenuItem` | 設定 | 設定画面を開く |
| `trashToolStripMenuItem` | 登録のごみ箱 | ごみ箱画面を開く |
| `backupToolStripMenuItem` | バックアップ | バックアップ画面を開く |
| `repairShortcutsToolStripMenuItem` | ショートカット修復 | リンク先修復 |
| `helpToolStripMenuItem` | ヘルプ | ヘルプメニューの親 |
| `usageGuideToolStripMenuItem` | 使い方 | ガイドを開く |
| `aboutToolStripMenuItem` | バージョン情報 | About画面を開く |

---

# 14. DataGridView列契約

| 日本語の列 | `Name` | 種類の目安 | 機能 |
|---|---|---|---|
| 種類アイコン | `itemIconColumn` | `DataGridViewImageColumn` | 猫アイコンを表示する |
| 種類 | `itemTypeColumn` | `DataGridViewTextBoxColumn` | フォルダー、画像、URLなどを表示する |
| タイトル | `itemTitleColumn` | `DataGridViewTextBoxColumn` | 登録名を表示する |
| 内容・参照先 | `itemReferenceColumn` | `DataGridViewTextBoxColumn` | パス、URL、テンプレート概要を表示する |
| 状態 | `itemStatusColumn` | `DataGridViewTextBoxColumn` | 存在しないファイル等を表示する |
| サムネイル | `itemThumbnailColumn` | `DataGridViewImageColumn` | 画像・動画のプレビューを表示する |

列の順序、幅、表示・非表示は少佐が調整できます。

---

# 15. 右クリックメニュー契約

| `Name` | 日本語表示例 | 機能 |
|---|---|---|
| `itemContextMenuStrip` | 項目右クリックメニュー | メニュー本体 |
| `openToolStripMenuItem` | 開く | 開く |
| `copyToolStripMenuItem` | コピー | コピー |
| `editToolStripMenuItem` | 編集 | 編集 |
| `detailToolStripMenuItem` | 詳細 | 詳細表示 |
| `deleteToolStripMenuItem` | 登録をごみ箱へ | 登録情報を移動 |
| `openContainingFolderToolStripMenuItem` | 置いてあるフォルダーを開く | 親フォルダーを開く |
| `copyTitleToolStripMenuItem` | タイトルをコピー | タイトルコピー |

右クリックして動かさず離した場合はメニュー、右ボタンでドラッグした場合は並び替えとして扱います。

---

# 16. ToolTipとアクセシビリティ

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

# 17. SettingsForm契約

## 17.1 表示

| `Name` | 日本語での意味 |
|---|---|
| `typeDisplayModeComboBox` | 種類表示方法 |
| `settingsShowBeginnerHintsCheckBox` | ヒント表示 |
| `settingsShowIconMeaningCheckBox` | アイコン説明表示 |
| `actionButtonDisplayModeComboBox` | 初心者向け / コンパクト切替 |
| `itemVisualModeComboBox` | 一覧 / サムネイル切替 |
| `showThumbnailsCheckBox` | サムネイル表示を使う |
| `thumbnailSizeComboBox` | サムネイルサイズ |

## 17.2 D&D・並び順

| `Name` | 日本語での意味 |
|---|---|
| `confirmTitleOnDropAddCheckBox` | D&D追加時タイトル確認 |
| `enableGroupDropModifierShortcutsCheckBox` | Ctrl / Shiftショートカット |
| `confirmGroupDropCopyMoveCheckBox` | グループドロップ確認 |
| `enableItemDragReorderCheckBox` | D&D並び替えを使う |
| `manualReorderInputModeComboBox` | 右ドラッグのみ / 左右許可 |
| `defaultItemSortModeComboBox` | 既定の並び方 |
| `unsavedOrderBehaviorComboBox` | 毎回確認 / 自動保存 / 自動破棄 |

## 17.3 バックアップ・削除

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
| `emptyTrashButton` | ごみ箱内の登録情報をすべて削除 |

## 17.4 常駐・起動

| `Name` | 日本語での意味 |
|---|---|
| `minimizeToTrayOnCloseCheckBox` | 閉じるボタンでタスクトレイへ |
| `autoStartWithWindowsCheckBox` | Windows起動時に自動起動 |
| `startMinimizedCheckBox` | 自動起動時に最小化 |
| `registerStartMenuButton` | スタートメニュー登録 |
| `unregisterStartMenuButton` | スタートメニュー登録解除 |
| `repairShortcutsButton` | ショートカット修復 |

---

# 18. RecycleBinForm契約

| 画面上の意味 | `Name` | 機能 |
|---|---|---|
| 元ファイルは削除しない説明 | `trashMeaningNoticeLabel` | 注意書きを常時表示する |
| 登録のごみ箱一覧 | `trashGridView` | 削除済み登録を一覧表示する |
| 登録を元に戻す | `restoreRegistrationButton` | 元グループへ復元する |
| ごみ箱から登録を削除 | `removeRegistrationFromTrashButton` | 登録情報だけを削除する |
| 登録情報をすべて削除 | `emptyRegistrationTrashButton` | ごみ箱を空にする |
| 閉じる | `closeButton` | 画面を閉じる |

`trashGridView` に欲しい列：

- 種類
- タイトル
- 元のグループ
- 参照先または内容
- ごみ箱へ入れた日時
- 自動削除予定日

手動削除のみの場合は「手動で削除するまで保持」と表示します。

---

# 19. サムネイル契約

| `Name` | 日本語での意味 |
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

# 20. 少佐がDesignerで配置するときの確認

1. `UI_SCREEN_MAP.md` で置きたい領域を確認する
2. 見た目と場所を決める
3. 文字ボタンへ `<action>Button` のNameを付ける
4. アイコンボタンへ `<action>IconButton` のNameを付ける
5. MenuStrip項目へ契約名を付ける
6. アイコンだけの操作へToolTipとAccessibleNameを付ける
7. 未実装機能はDisabledにしておく
8. イベント処理はまだ書かない

---

# 21. Codexへ機能接続を依頼するときの文面

```text
Designerで配置済みのUIを作り直さず、docs/UI_CONTROL_CONTRACT.md のNameに接続してください。

- Text / Size / Location / Margin / Padding / Dock / Anchorを不用意に上書きしないでください。
- 文字ボタン、アイコンボタン、ToolStrip、MenuStripなど、同じ機能の複数入口は同じPresenter処理へ接続してください。
- 各入口へ処理を重複実装しないでください。
- 保存、ファイル操作、並び替え、ごみ箱処理はServiceへ分離してください。
- 未実装機能は、実装するまでDisabledのまま維持してください。
- UI変更が必要な場合は、変更前に理由と対象を報告してください。
```
