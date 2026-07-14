# ContextBinder v2 UI画面設計マップ

この文書は、少佐が `MainFormDesignDraft` を Visual Studio Designer で組み立てるときに、**画面全体を想像するための設計図**です。

- 見た目、配置、余白、色、固定文言：少佐が決める
- ボタンを押したときの処理、保存、検証：Codexが実装する
- この文書：何をどの領域へ置くか考えるために使う
- [`UI_CONTROL_CONTRACT.md`](UI_CONTROL_CONTRACT.md)：各部品の正確な `Name` を確認するために使う

現在の実装状況は [`IMPLEMENTATION_MATRIX.md`](IMPLEMENTATION_MATRIX.md)、詳細仕様は [`ContextBinder_v2_SPEC.md`](ContextBinder_v2_SPEC.md) を参照してください。

---

# 1. まず知っておく言葉

Visual Studio Designer のツールボックスには英語名が多いため、この文書では次の意味で使います。

| 英語名 | 日本語での意味 | 何に使うか |
|---|---|---|
| `Panel` | 自由配置できる箱・領域 | 見出し、ボタン、説明などをまとめる |
| `FlowLayoutPanel` | 中身を順番に自動整列する箱 | 文字ボタンやアイコンを縦・横に並べる |
| `SplitContainer` | 境界を動かせる分割領域 | グループ一覧と項目一覧を左右に分ける |
| `TableLayoutPanel` | 行と列で揃える表形式の箱 | 「見出し＋右端の＋」など、部分的な整列に使う |
| `MenuStrip` | 画面上部のメニューバー | ファイル、登録、編集、表示、ツール、ヘルプ |
| `ToolStrip` | アイコン中心のツールバー | コンパクト操作を横並びにする |
| `ListBox` | 単純な一覧 | グループ一覧 |
| `DataGridView` | 表形式の一覧 | 登録項目一覧、サムネイル、状態表示 |
| `Label` | 文字表示 | 見出し、説明、ステータス、未保存警告 |
| `Button` | 押すボタン | 追加、開く、保存、削除など |
| `TextBox` | 文字入力欄 | 検索キーワード |
| `ComboBox` | 選択肢を開く欄 | 並び方、種類、検索範囲、表示方法 |
| `GroupBox` | 見出しつきの囲み | 使い方のヒント、アイコンの意味 |
| `ToolTip` | マウスを乗せた時の説明 | アイコンだけのボタンの意味を表示 |

## `Name` と `Text` の違い

- `Name`：Codexが機能接続に使う内部名。英語で統一する
- `Text`：画面に表示される文字。日本語で自由に決める

例：

```text
Name = addGroupButton
Text = グループ追加
```

アイコンボタンの場合：

```text
Name = addGroupIconButton
Text = ＋ または空欄
ToolTip = グループ追加
```

---

# 2. 同じ機能に複数の入口を置く

ContextBinderでは、同じ機能に複数の入口を置けます。

| 入口 | 主な用途 | 例 |
|---|---|---|
| 初心者向け文字ボタン | 初見でも意味が分かる | `グループ追加` |
| コンパクトなアイコンボタン | 省スペースで素早く操作する | グループ一覧右上の `＋` |
| `MenuStrip` | 一般的なWindowsツールとして操作する | `登録 > グループ追加` |
| 右クリックメニュー | 選択中の項目へ詳細操作を行う | `開く`、`コピー`、`編集` |

これらは別々の機能ではありません。

```text
初心者向け「グループ追加」ボタン ┐
グループ一覧右上の「＋」ボタン    ├→ 同じグループ追加処理
MenuStripの「グループ追加」       ┘
```

少佐は、それぞれを置く場所や見た目を自由に決められます。Codexは全入口を同じPresenter処理へ接続します。

---

# 3. 現在の参考デザイン

今まで考えてきた画面構成を、**参考案として残します**。固定仕様ではありません。

## 3.1 初心者向け文字ボタン表示

```text
┌──────────────────────────────────────────────────────────┐
│ ファイル  登録  編集  表示  ツール  ヘルプ              │ MenuStrip
├──────────────┬──────────────────────────┬────────────────┤
│ グループ  ＋ │ 検索・絞り込み・並び方   │ 操作           │
│──────────────│──────────────────────────│ [グループ追加] │
│ TRPG         │ 項目一覧                 │ [ファイル追加] │
│ 動画編集     │                          │ [フォルダ追加] │
│ 開発         │                          │ [URL追加]      │
│              │                          │ [開く]         │
│              │                          │ [コピー]       │
│              │                          │ [編集]         │
│              │                          │ [登録をごみ箱] │
│              │──────────────────────────│                │
│              │ 状態・ヒント・アイコン説明│                │
└──────────────┴──────────────────────────┴────────────────┘
```

## 3.2 コンパクトアイコン表示

コンパクトモードでは、アイコンは**常に見える状態**にします。マウスを乗せた時だけ説明を表示します。

```text
┌──────────────────────────────────────────────────────────┐
│ ファイル  登録  編集  表示  ツール  ヘルプ              │
├──────────────┬───────────────────────────────────────────┤
│ グループ  ＋ │ 検索 🔍　種類▼　並び方▼　表示▼             │
│──────────────│ ＋📄 ＋📁 ＋URL ＋文   開く コピー 編集 🗑 │
│ TRPG         │───────────────────────────────────────────│
│ 動画編集     │ 項目一覧                                  │
│ 開発         │                                           │
│              │ ↑ ↓ 保存 戻す                             │
│              │───────────────────────────────────────────│
│              │ 状態・ヒント・アイコン説明                 │
└──────────────┴───────────────────────────────────────────┘
```

- 初心者向けでは文字ボタン領域を表示する
- コンパクト向けでは文字ボタン領域を非表示にする
- コンパクトアイコンは、操作対象の近くに配置する
- `MenuStrip` はどちらのモードでも表示する

---

# 4. 推奨するコンテナ構造

次の構造にすると、画面を広げても崩れにくく、初心者向け／コンパクト表示も切り替えやすくなります。

```text
MainFormDesignDraft
├─ mainMenuStrip                    上部メニューバー
└─ mainSplitContainer               左右を分割する大枠
   ├─ Panel1
   │  └─ groupAreaPanel             グループ領域全体
   │     ├─ groupHeaderPanel        「グループ」見出しと＋
   │     └─ groupListBox            グループ一覧
   │
   └─ Panel2
      └─ workAreaPanel              項目・操作領域全体
         ├─ beginnerActionPanel     初心者向け文字ボタン
         └─ itemAreaPanel           項目一覧側の領域
            ├─ searchAndViewPanel   検索・絞り込み・並び方
            ├─ itemToolbarPanel     追加・操作・並び替えアイコン
            ├─ bottomInformationPanel 状態・ヒント・凡例
            └─ itemGridView         項目一覧
```

## 4.1 各コンテナの役割

| `Name` | 日本語での意味 | 種類の候補 | 主な役割 | 配置例 |
|---|---|---|---|---|
| `mainMenuStrip` | 上部メニューバー | `MenuStrip` | ファイル、登録、編集、表示、ツール、ヘルプ | `Dock = Top` |
| `mainSplitContainer` | 左右分割の大枠 | `SplitContainer` | 左をグループ、右を項目・操作に分ける | `Dock = Fill` |
| `groupAreaPanel` | グループ領域全体 | `Panel` | 見出しとグループ一覧をまとめる | `Dock = Fill` |
| `groupHeaderPanel` | グループ見出し領域 | `Panel` / `TableLayoutPanel` | 「グループ」と＋アイコンを置く | `Dock = Top` |
| `workAreaPanel` | 右側作業領域全体 | `Panel` | 項目領域と初心者ボタン領域をまとめる | `Dock = Fill` |
| `beginnerActionPanel` | 初心者向け操作領域 | `Panel` / `FlowLayoutPanel` | 文字つき操作ボタンをまとめる | `Dock = Right` が参考 |
| `itemAreaPanel` | 項目一覧側の領域 | `Panel` | 検索、一覧、並び替え、下部情報をまとめる | `Dock = Fill` |
| `searchAndViewPanel` | 検索・表示操作領域 | `Panel` | 検索、絞り込み、並び方、表示方法 | `Dock = Top` |
| `itemToolbarPanel` | 項目操作ツール領域 | `Panel` / `FlowLayoutPanel` / `ToolStrip` | 追加、開く、コピー、編集、並び替え | `Dock = Top` |
| `bottomInformationPanel` | 下部情報領域 | `Panel` | ステータス、ヒント、アイコンの意味 | `itemAreaPanel` 内で `Dock = Bottom` |

### `bottomInformationPanel` の重要点

`bottomInformationPanel` はフォーム直下ではなく、`itemAreaPanel` の中に入れる案です。

そうすると、中央の項目領域の下だけに表示でき、左のグループ領域や右の初心者向け操作領域には干渉しません。

```text
左：グループ │ 中央：項目一覧
              │ 中央下：bottomInformationPanel
              │ 右：初心者向け操作
```

## 4.2 小さなコンパクト操作用コンテナ

アイコンをまとめて表示・非表示にしたい場合だけ、次の小さな箱を使います。

| `Name` | 日本語での意味 | 中へ置く例 |
|---|---|---|
| `groupCompactActionPanel` | グループ用アイコン領域 | `addGroupIconButton` |
| `itemCompactActionPanel` | 項目操作アイコン領域 | 追加、開く、コピー、編集、ごみ箱 |
| `orderCompactActionPanel` | 並び替えアイコン領域 | 上、下、保存、保存前に戻す |

これらは必須ではありません。アイコンを直接 `groupHeaderPanel` や `itemToolbarPanel` に置いても構いません。

---

# 5. 各領域へ置くもの

## 5.1 上部メニューバー

`mainMenuStrip` に置く参考構成です。

```text
ファイル
  登録内容を書き出す
  登録内容を読み込む
  終了

登録
  グループ追加
  ファイル追加
  フォルダー追加
  URL追加
  テンプレート追加

編集
  開く
  コピー
  編集
  詳細
  登録をごみ箱へ
  上へ移動
  下へ移動
  並び順を保存
  保存前に戻す

表示
  初心者向け文字ボタン
  コンパクトアイコンボタン
  一覧 / サムネイル
  並び方
  ヒント
  アイコンの意味

ツール
  設定
  登録のごみ箱
  バックアップ
  ショートカット修復

ヘルプ
  使い方
  バージョン情報
```

正確な `Name` は `UI_CONTROL_CONTRACT.md` の「MenuStrip」を参照します。

---

## 5.2 グループ領域

### 目的

- グループを一覧表示する
- 現在のグループを選択する
- 新しいグループを追加する
- 将来、名前変更、削除、並び替えを行う

### 置く部品の例

| 画面上の意味 | `Name` | 種類 | 機能 |
|---|---|---|---|
| 「グループ」見出し | `groupTitleLabel` | `Label` | 領域名を表示する |
| グループ一覧 | `groupListBox` | `ListBox` | グループを選択する |
| グループ追加・文字 | `addGroupButton` | `Button` | 初心者向けのグループ追加 |
| グループ追加・＋ | `addGroupIconButton` | `Button` | コンパクト表示のグループ追加 |

### 表示例

```text
グループ                         ＋
────────────────────────────────
TRPG
動画編集
開発
```

`addGroupIconButton` の画面表示は `＋` だけでも構いません。

```text
ToolTip：グループ追加
AccessibleName：グループ追加
```

---

## 5.3 検索・絞り込み・表示方法

### 目的

項目を探す、表示対象を絞る、一時的な並び方を変更する領域です。

### 置く部品の例

| 画面上の意味 | `Name` | 種類 | 機能 |
|---|---|---|---|
| 検索文字入力 | `searchTextBox` | `TextBox` | タイトル、パス、URL、本文などを検索する |
| 検索実行・文字 | `searchButton` | `Button` | 検索を実行する |
| 検索実行・アイコン | `searchIconButton` | `Button` | コンパクト表示で検索する |
| 検索解除 | `clearSearchButton` | `Button` | 検索条件を消す |
| 検索範囲 | `searchScopeComboBox` | `ComboBox` | 現在のグループ / 全体を選ぶ |
| 種類絞り込み | `typeFilterComboBox` | `ComboBox` | フォルダー、画像、URLなどで絞る |
| 並び方 | `sortModeComboBox` | `ComboBox` | 手動、名前、種類、追加、更新順を選ぶ |
| 表示方法 | `itemVisualModeComboBox` | `ComboBox` | 一覧、サムネイル一覧、大きいサムネイルを選ぶ |

### 表示例

```text
検索：[　　　　　　　　　] [検索] [解除]
範囲：[現在のグループ▼]　種類：[すべて▼]
並び方：[手動並び順▼]　表示：[通常一覧▼]
```

配置順や行分けは自由です。

---

## 5.4 項目の追加

### 置く入口の例

| 機能 | 文字ボタン `Name` | アイコンボタン `Name` | MenuStrip `Name` |
|---|---|---|---|
| ファイル追加 | `addFileButton` | `addFileIconButton` | `addFileToolStripMenuItem` |
| フォルダー追加 | `addFolderButton` | `addFolderIconButton` | `addFolderToolStripMenuItem` |
| URL追加 | `addUrlButton` | `addUrlIconButton` | `addUrlToolStripMenuItem` |
| テンプレート追加 | `addTemplateButton` | `addTemplateIconButton` | `addTemplateToolStripMenuItem` |

初心者向け表示例：

```text
[ファイル追加]
[フォルダー追加]
[URL追加]
[テンプレート追加]
```

コンパクト表示例：

```text
[＋📄] [＋📁] [＋URL] [＋文]
```

アイコンの絵は後から変更して構いません。

---

## 5.5 項目一覧

### 主な部品

| 画面上の意味 | `Name` | 種類 | 機能 |
|---|---|---|---|
| 登録項目一覧 | `itemGridView` | `DataGridView` | 選択グループの項目を表形式で表示する |

### 列の候補

| 日本語の列 | `Name` | 内容 |
|---|---|---|
| 種類アイコン | `itemIconColumn` | 猫アイコン |
| 種類 | `itemTypeColumn` | フォルダー、画像、URLなど |
| タイトル | `itemTitleColumn` | 登録時の表示名 |
| 内容・参照先 | `itemReferenceColumn` | パス、URL、テンプレート概要 |
| 状態 | `itemStatusColumn` | ファイルが存在しない等の警告 |
| サムネイル | `itemThumbnailColumn` | 画像・動画のプレビュー |

列の順番、幅、表示・非表示は少佐が決められます。

---

## 5.6 選択項目の操作

| 機能 | 文字ボタン `Name` | アイコンボタン `Name` | 機能説明 |
|---|---|---|---|
| 開く | `openButton` | `openIconButton` | ファイル、フォルダー、URLを開く。テンプレートはコピーする |
| コピー | `copyButton` | `copyIconButton` | パス、URL、テンプレート本文をコピーする |
| 編集 | `editButton` | `editIconButton` | タイトルや参照先を変更する |
| 詳細 | `detailButton` | `detailIconButton` | 登録内容を変更せず確認する |
| 登録をごみ箱へ | `deleteButton` | `deleteIconButton` | ContextBinderの登録情報だけをごみ箱へ移動する |

`deleteButton` の内部名は既存互換のため維持しますが、画面表示は「登録をごみ箱へ」など、元ファイルを消さないと分かる文言にできます。

---

## 5.7 並び替え・ソート

### 画面例

```text
並び方：[手動並び順 ▼]

[↑ 上へ]
[↓ 下へ]
[並び順を保存]
[保存前に戻す]

● 並び順に未保存の変更があります
```

### 置く部品の例

| 画面上の意味 | 文字ボタン `Name` | アイコンボタン `Name` | 機能 |
|---|---|---|---|
| 上へ移動 | `moveUpButton` | `moveUpIconButton` | 選択項目を1段上へ移動する |
| 下へ移動 | `moveDownButton` | `moveDownIconButton` | 選択項目を1段下へ移動する |
| 並び順を保存 | `saveManualOrderButton` | `saveManualOrderIconButton` | 編集中の並び順を正式保存する |
| 保存前に戻す | `revertUnsavedOrderButton` | `revertUnsavedOrderIconButton` | 最後に保存した並び順へ戻す |
| 未保存表示 | `orderUnsavedStatusLabel` | `Label` | 未保存変更がある時だけ表示する |

### 動作の考え方

- 既定では右ドラッグで手動並び替えする
- 上へ / 下へボタンでも移動できる
- 移動しただけでは正式保存しない
- `並び順を保存` を押した時だけ保存する
- `保存前に戻す` で最後に保存した手動順へ戻す
- 未保存表示は、変更がある時だけ表示する
- 名前順、種類順、追加順、更新順へ切り替えても保存済み手動順は消さない

---

## 5.8 下部情報

`bottomInformationPanel` は、中央の `itemAreaPanel` 内の下部へ置く参考案です。

### 置く部品の例

| 画面上の意味 | `Name` | 種類 | 機能 |
|---|---|---|---|
| 操作結果 | `statusLabel` | `Label` | 「コピーしました」などを表示する |
| 使い方表示切替 | `showBeginnerHintsCheckBox` | `CheckBox` | ヒントの表示・非表示を切り替える |
| 使い方のヒント | `beginnerHintsGroupBox` | `GroupBox` | 初心者向け説明を表示する |
| アイコン説明切替 | `showIconMeaningCheckBox` | `CheckBox` | アイコン説明の表示・非表示を切り替える |
| アイコンの意味 | `iconMeaningGroupBox` | `GroupBox` | 猫アイコン等の意味を表示する |
| アイコン説明配置先 | `iconMeaningPanel` | `Panel` / `FlowLayoutPanel` | 種類別アイコン説明を並べる |
| ホバー説明 | `actionHoverDescriptionLabel` | `Label` | アイコンへマウスを乗せた時の補助説明。任意 |

表示内容が多い場合は、`TabControl` で「状態」「使い方」「アイコンの意味」を分ける案もあります。

---

# 6. 初心者向けとコンパクト向けの切替

## 初心者向け

- `beginnerActionPanel` を表示する
- 文字つきボタンを表示する
- コンパクト用アイコン領域を非表示にする
- `MenuStrip` は表示する

## コンパクト向け

- `beginnerActionPanel` を非表示にする
- `groupCompactActionPanel`、`itemCompactActionPanel`、`orderCompactActionPanel` などを表示する
- アイコン自体は常に表示する
- マウスを乗せた時だけToolTipや説明文を表示する
- `MenuStrip` は表示する

Designer作業中は両方見える状態で配置して構いません。表示切替処理はCodexが後から接続します。

---

# 7. 外枠を作る順番

一度に全機能を置かず、次の順で進めると想像しやすくなります。

1. `mainMenuStrip` を上部へ置く
2. `mainSplitContainer` を残り全体へ置く
3. 左側へ `groupAreaPanel` を置く
4. `groupHeaderPanel` と `groupListBox` を置く
5. 右側へ `workAreaPanel` を置く
6. `beginnerActionPanel` を右へ置く
7. 残りへ `itemAreaPanel` を置く
8. `searchAndViewPanel` を上へ置く
9. `itemToolbarPanel` を上へ置く
10. `bottomInformationPanel` を下へ置く
11. 残った中央へ `itemGridView` を置く
12. 最後に各ボタン、入力欄、ラベルを置く

## `Dock` の参考

| 対象 | 参考設定 |
|---|---|
| `mainMenuStrip` | `Top` |
| `mainSplitContainer` | `Fill` |
| `groupAreaPanel` | `Fill` |
| `groupHeaderPanel` | `Top` |
| `groupListBox` | `Fill` |
| `workAreaPanel` | `Fill` |
| `beginnerActionPanel` | `Right` |
| `itemAreaPanel` | `Fill` |
| `searchAndViewPanel` | `Top` |
| `itemToolbarPanel` | `Top` |
| `bottomInformationPanel` | `Bottom` |
| `itemGridView` | `Fill` |

これは参考値です。少佐が作りたい見た目に合わせて変更できます。

---

# 8. SettingsFormの主なカテゴリ

MainForm完成後に作る画面です。

| 日本語カテゴリ | 主な内容 |
|---|---|
| 表示 | 文字ボタン / アイコン、一覧 / サムネイル、ヒント、アイコンの意味 |
| D&D | タイトル確認、右ドラッグ、コピー・移動確認 |
| 検索・並び順 | 既定範囲、既定の並び方、未保存時の確認方法 |
| バックアップ・削除 | 自動バックアップ、登録のごみ箱、保持日数 |
| 常駐・起動 | タスクトレイ、自動起動、スタートメニュー |
| 保存場所 | 現在の保存先、移行、フォルダーを開く |

ごみ箱関連には、次の説明を常時表示します。

```text
削除されるのはContextBinderへの登録情報だけです。
元のファイル、フォルダー、画像、動画は削除されません。
```

---

# 9. その他の画面

| 画面 | 日本語での役割 | 現在の状態 |
|---|---|---|
| `FirstRunSetupForm` | 初回設定画面 | 基本機能あり |
| `ItemEditForm` | ファイル・URL等の編集画面 | 基本機能あり |
| `TemplateEditForm` | テンプレート本文編集画面 | 専用画面は未完成 |
| `ItemDetailForm` | 登録内容の詳細確認画面 | 簡易表示のみ |
| `CopyMoveDialog` | 他グループへのコピー・移動画面 | 未実装 |
| `RecycleBinForm` | 登録のごみ箱画面 | 未実装 |
| `ImportExportDialog` | 読み込み・書き出し画面 | 未実装 |
| `BackupRestoreDialog` | バックアップ復元画面 | 未実装 |
| `AboutForm` | バージョン・作者・権利情報 | 未実装 |

---

# 10. 少佐が今作る範囲

現在は `MainFormDesignDraft` の見た目を作る段階です。

最低限、次の外枠を作れば十分です。

- `mainMenuStrip`
- `mainSplitContainer`
- `groupAreaPanel`
- `groupHeaderPanel`
- `groupListBox`
- `workAreaPanel`
- `beginnerActionPanel`
- `itemAreaPanel`
- `searchAndViewPanel`
- `itemToolbarPanel`
- `bottomInformationPanel`
- `itemGridView`

ボタンはまだ動かなくて構いません。

少佐が決めるもの：

- 配置
- サイズ
- 色
- 文言
- 余白
- グルーピング
- アイコン
- 初心者向けとコンパクト向けの見せ方

Codexが後から行うもの：

- 各入口を同じ機能へ接続
- 初心者向け／コンパクト表示切替
- 保存
- D&D
- ToolTipの補助
- Presenter / Serviceとの接続
- テスト

---

# 11. UI完成後にCodexへ伝えること

```text
ContextBinder v2 のUIをVisual Studio Designerで作成しました。

- Designer管理のText / Size / Location / Margin / Padding / Dock / Anchorを変更しないでください。
- docs/UI_SCREEN_MAP.md と docs/UI_CONTROL_CONTRACT.md のName契約に従ってください。
- 文字ボタン、アイコンボタン、MenuStripなど、同じ機能の複数入口を同じPresenter処理へ接続してください。
- 現在配置済みのUIへ、既存機能をPresenter / Service経由で接続してください。
- 未実装機能の入口は、実装されるまでDisabledのまま維持してください。
- UI変更が必要な場合は、変更前に理由と対象を報告してください。
```
