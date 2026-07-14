# ContextBinder v2 仕様書

## 0. この文書について

この文書は、ContextBinder v2 の開発用仕様書である。

ContextBinder v2 は、v1 の「初心者でも分かりやすい」思想を継承しつつ、C# WinForms で再設計する。
ファイル、フォルダ、画像、動画、URL、テンプレート文をグループ単位で整理し、すぐに開く・コピーするための Windows 常駐型ツールを目指す。

公開配布時の README には、本仕様書の詳細な内部設計までは含めない。
公開 README は利用者向け説明に限定し、開発用の詳細設計・JSON構造・Codex向け実装指示は同梱しない。

---

## 1. 目的

ContextBinder v2 の目的は、よく使う「作業の入口」をひとつの画面にまとめることである。

対象にするものは以下。

- フォルダ
- 通常ファイル
- 画像ファイル
- 動画ファイル
- URL
- テンプレート文

主な利用シーンは以下。

- TRPG素材、シナリオ資料、募集文、部屋URLの管理
- 動画編集素材、画像、動画、編集フォルダの管理
- 開発用フォルダ、GitHub、Codex指示文、プロンプトの管理
- Discord、メール、問い合わせ、支援申請などの定型文管理
- 日常的に使うファイル、フォルダ、URLの整理

---

## 2. 基本方針

### 2.1 UI方針

- v1 の「初心者でも分かりやすい」画面思想を継承する。
- 画面を見れば主要操作が分かるようにする。
- 右クリック操作は追加するが、右クリックだけに依存しない。
- 主要操作は右側ボタンからも実行できるようにする。
- 下部に操作説明、ステータス表示、アイコン凡例を置く。
- 成功通知はステータス表示、確認や失敗は MessageBox を使う。
- 猫アイコンを使用し、作者製ツールであることが分かる見た目にする。
- 固定UIは Visual Studio Designer で編集できる構成を優先する。
- `UiLayoutSettings.cs` は動的生成UIや実行時補助に限定し、固定UIの主な調整場所にはしない。
- MainFormには上部メニューバーを追加できる構成にする。
- 操作ボタンは、初心者向けの文字つき表示と、コンパクトなアイコン中心表示を切り替えられるようにする。
- コンパクト表示では ToolTip とホバー説明を使い、操作意味が分かるようにする。
- 画像/動画は将来的にサムネイル表示へ対応する。まず画像サムネイルを優先し、動画サムネイルは段階的に実装する。

### 2.2 操作方針

- 登録操作はドラッグ＆ドロップを主導線とする。
- 右側の追加ボタンは補助導線として残す。
- D&D追加時のタイトル確認はオプション化し、初期値はOFFとする。
- 同一グループ内の重複登録は参照先で判定する。
- 同じファイル名、フォルダ名、タイトルでも、参照先が違えば登録可能とする。
- 他グループに同じ参照先があっても、別グループの別項目として扱う。

### 2.3 保存方針

- ユーザー向け表記では「データ」だけを単独で使わず、「登録内容と設定」と表記する。
- 登録元のファイルそのものはコピーしない。
- 保存するのは、登録項目、参照先、テンプレート本文、グループ、設定、バックアップ、ごみ箱、削除履歴である。
- 保存場所は初回起動時に選べる。
- 標準保存、アプリフォルダ内保存、任意フォルダ保存に対応する。
- アプリフォルダ内保存 / 任意フォルダ保存では、AppData に不要なファイルを作らない。

---

## 3. 技術スタック

- 言語：C#
- UI：WinForms
- 実行環境：Windows 11
- 中央一覧：DataGridView
- グループ一覧：ListBox
- 右クリック：ContextMenuStrip
- タスクトレイ：NotifyIcon
- クリップボード操作：Clipboard
- 保存形式：JSON
- 配布形式：BOOTH向け単一ZIPを基本とする
- 配布方針：self-contained / single-file publish を検討

---

## 4. 配布方針

BOOTHでは、基本的に単一ZIPで配布する。

```text
ContextBinder_v2.zip
```

ZIP内の想定構成。

```text
ContextBinder_v2/
  ContextBinder.exe
  はじめに読んでね.txt
  licenses/
    licenses.txt
```

初期配布では、インストーラーは必須としない。

- ユーザーはZIPを解凍する。
- `ContextBinder.exe` を起動する。
- 初回起動時に保存場所と基本設定を選択する。

将来的に以下を検討する。

- インストーラー版
- アンインストール対応

準MVPとして、インストーラーなしのZIP配布でも利用しやすくするため、以下を設定画面で優先対応する。

- スタートメニュー登録 / 登録解除
- Windows起動時自動起動 / 自動起動解除
- 起動時最小化、またはタスクトレイ格納での起動
- アプリ本体の場所が変わった場合のショートカット修復

これらは管理者権限を要求しないユーザー単位ショートカットを基本とする。
レジストリRunキーやタスクスケジューラは第一候補にしない。
ZIP配布ではアプリ本体の場所が移動される可能性があるため、アプリ内から登録、解除、修復できる導線を用意する。

---

## 5. 作成されるフォルダとファイル

### 5.1 ユーザー向け表記

UIやREADMEでは、以下の表記を使う。

| 用途 | 表記 |
|---|---|
| 保存場所 | 登録内容と設定の保存場所 |
| バックアップ | 登録内容のバックアップ |
| エクスポート | 登録内容を書き出す |
| インポート | 登録内容を読み込む |
| 保存フォルダ | 登録内容と設定フォルダ |

### 5.2 実フォルダ名

実際に作成されるフォルダ名は英数字にする。

```text
ContextBinder_Data
```

公開READMEでは、必要最低限として以下を説明する。

- 登録したファイルやフォルダそのものはコピーしない。
- 保存されるのは、参照先、タイトル、グループ、テンプレート文、表示設定などである。
- 大切な登録内容はエクスポートして別ドライブやクラウドにも保存できる。

---

## 6. 保存場所モード

### 6.1 通常の場所に保存

初心者おすすめの標準保存方式。

```text
%AppData%\ContextBinder\
  contextbinder.store.json
  settings.json
  backups/
  trash/
```

特徴。

- Windowsのアプリ用フォルダに保存する。
- 通常はこちらを推奨する。
- アプリ本体を移動しても登録内容と設定が残る。

### 6.2 このアプリのフォルダに保存

ポータブル運用向け。

```text
ContextBinder_v2/
  ContextBinder.exe
  contextbinder.storage.json
  ContextBinder_Data/
    contextbinder.store.json
    settings.json
    backups/
    trash/
```

特徴。

- アプリと同じフォルダ内に保存する。
- AppDataには保存しない。
- フォルダごとバックアップ・移動できる。
- 書き込み不可の場所に置かれている場合は警告する。

### 6.3 自分で選んだ場所に保存

別ドライブ・クラウド同期フォルダ向け。

アプリフォルダ側。

```text
ContextBinder_v2/
  ContextBinder.exe
  contextbinder.storage.json
```

指定先側。

```text
D:\MyContextBinder\
  contextbinder.store.json
  settings.json
  backups/
  trash/
```

特徴。

- 登録内容と設定はユーザー指定フォルダに保存する。
- 保存先を記録する小さな設定ファイルだけをアプリフォルダに作成する。
- AppDataには保存しない。
- アプリフォルダに書き込めない場合は、別の方法を案内する。

### 6.4 上級者向け将来候補

起動引数による保存先指定を将来候補とする。

```text
ContextBinder.exe --data-dir "D:\ContextBinder"
```

この場合は、AppDataにもアプリフォルダにも保存先設定を作らず、指定先のみを使用する。

---

## 7. 初回起動セットアップ

初回起動時にセットアップ画面を表示する。

### 7.1 最初の画面

```text
ContextBinderへようこそ

このツールは、ファイル・フォルダ・URL・テンプレート文を
グループごとにまとめて、すぐ開く/コピーできるツールです。

まずは保存場所と使いやすさ設定を選びます。

[初心者おすすめ設定で始める]
[カスタム設定を選ぶ]
```

### 7.2 保存場所選択

```text
登録内容と設定の保存場所を選んでください

○ 通常の場所に保存（おすすめ）
  Windowsがアプリ用に用意している場所に保存します。
  どれを選べばよいか分からない場合はこちらを選んでください。

○ このアプリのフォルダに保存
  ContextBinder.exe と同じフォルダ内に保存します。
  フォルダごとUSBや別ドライブに置きたい人向けです。
  AppDataには保存しません。

○ 自分で選んだ場所に保存
  登録内容と設定の保存先を自分で指定します。
  別ドライブやクラウド同期フォルダに保存したい人向けです。
  AppDataには保存しません。
```

### 7.3 保存されるものの説明

```text
保存されるもの：
・登録したファイル、フォルダ、URLの参照先
・登録したテンプレート文
・グループ名と並び順
・表示設定や操作設定
・自動バックアップ
・ごみ箱、削除履歴

保存されないもの：
・登録元のファイルそのもの
・登録元の画像や動画そのもの
・登録元のフォルダの中身
```

### 7.4 初心者おすすめ設定

初心者おすすめ設定は以下。

| 設定 | 値 |
|---|---|
| 種類表示 | アイコン＋文字 |
| D&D追加時タイトル確認 | OFF |
| 追加結果ステータス表示 | ON |
| 重複時に既存項目へジャンプ | ON |
| Ctrl/Shiftドロップショートカット | OFF |
| 通常グループドロップ時確認 | ON |
| D&D並び替え | ON |
| 外部D&D：ファイル/画像/動画/フォルダ | ON |
| 外部D&D：URLテキスト | ON |
| 外部D&D：テンプレート本文 | ON |
| 閉じるボタンでタスクトレイ | ON |
| 右クリック詳細操作 | ON |
| 自動バックアップ | ON |
| 削除前確認 | ON |
| 削除時ごみ箱移動 | ON |

### 7.5 サンプルデータ

初回起動時に、サンプルデータを作成するか選べる。

```text
サンプルデータを作成しますか？
使い方を確認するためのサンプルグループを作成します。
後から簡単に削除できます。
```

サンプルグループ名。

```text
はじめての使い方
```

---

## 8. メイン画面

### 8.1 基本構成

```text
上：メニューバー
左：グループ一覧
中央：項目一覧
右：操作ボタン
下：説明文 / ステータス / アイコン凡例
```

上部メニューバーは `MenuStrip` を使用する。
メニュー構成の候補は以下。

- ファイル
- 登録
- 編集
- 表示
- ツール
- ヘルプ

### 8.2 レイアウト案

```text
┌─────────────────────────────────────────────┐
│ ContextBinder v2                              │
├──────────────┬────────────────────┬─────────┤
│ グループ一覧 │ 項目一覧             │ 操作    │
│              │                    │         │
│ TRPG         │ 種類 タイトル 内容   │ 追加    │
│ 宣伝用       │ 🐱  宣伝画像         │ 編集    │
│ 生成AI       │ 🐱  Codex依頼文      │ 開く    │
│              │ 🐱  募集文テンプレ   │ コピー  │
│              │                    │ 詳細    │
│              │                    │ 削除    │
├──────────────┴────────────────────┴─────────┤
│ ファイル・フォルダ・URL・テキストをここにD&Dで登録できます。 │
│ 猫フォルダ / 猫ファイル / 猫画像 / 猫動画 / URL / テンプレ │
└─────────────────────────────────────────────┘
```

### 8.3 中央一覧

中央一覧には DataGridView を使用する。

基本列。

| 列 | 内容 |
|---|---|
| 種類アイコン | 猫アイコン |
| 種類文字 | フォルダ/ファイル/画像/動画/URL/テンプレート |
| タイトル | 表示名 |
| 内容/参照先 | パス、URL、またはテンプレート概要 |
| 状態 | 存在しないファイルの警告など |

検索が全体検索モードの場合、グループ列を一時表示してもよい。

### 8.3.1 項目の並び替えと表示用ソート

項目一覧の並び順は、以下の3層に分けて扱う。

| 種類 | 内容 |
|---|---|
| 保存済み手動並び順 | ユーザーが明示的に「並び順を保存」した正式な並び |
| 編集中の手動並び順 | 右ドラッグ、上下移動、選択範囲ソートで変更中の未保存の並び |
| 表示用ソート | 名前順、種類順、追加順、更新順など、一時的な見え方 |

名前順、種類順、追加順、更新順へ切り替えても、保存済み手動並び順は破壊しない。
手動並び順へ戻した場合は、最後に保存した手動並び順へ戻る。

#### 並び替えUI

MainFormには、次の操作を置く。

```text
並び方：[手動並び順 ▼]

[↑ 上へ]
[↓ 下へ]
[並び順を保存]
[保存前に戻す]

● 並び順に未保存の変更があります
```

`● 並び順に未保存の変更があります` は、未保存の変更がある時だけ表示する。
変更がない時は非表示にする。

#### 右ドラッグ並び替え

項目の手動並び替えは、誤操作を避けるため既定では右ボタンドラッグで行う。

| 操作 | 挙動 |
|---|---|
| 右クリックして動かさず離す | 右クリックメニューを表示 |
| 右ボタンを押したままドラッグ | 項目の手動並び替え、またはグループへのコピー/移動 |
| 左ドラッグで外部から中央一覧へドロップ | 新規登録 |
| 左ドラッグで項目を外部へ出す | 外部アプリへファイル/URL/本文を渡す |

右クリックと右ドラッグは、Windowsのドラッグ開始距離を超えた場合だけドラッグ扱いにする。
タッチパッド利用者のため、上へ/下へボタンは常に残す。

#### 保存と復元

D&Dや上下ボタンで並び替えても、すぐに `SortOrder` を正式保存しない。
変更は編集中の手動並び順として保持する。

- `並び順を保存`: 編集中の並び順を保存済み手動並び順として確定する
- `保存前に戻す`: 編集中の並び順を破棄し、最後に保存した手動並び順へ戻す

名前順などの表示用ソート中に右ドラッグや上下移動を行った場合は、その時点の表示順を編集中の手動並び順へコピーし、未保存状態にする。
`この表示順を手動編集` のような別ボタンは設けない。

#### 未保存確認

並び順に未保存の変更があり、その変更が失われる可能性がある操作では、既定で確認を表示する。

対象例：

- アプリ終了
- 保存内容の再読み込み
- インポートによる置き換え
- 対象グループの削除
- 初期化

確認文の例：

```text
並び順に保存していない変更があります。

[保存して続ける]
[変更を破棄して続ける]
[キャンセル]

□ 次回からこの選択を自動的に適用する
```

「次回からこの選択を自動的に適用する」を選んだ場合は、SettingsFormの詳細設定から戻せるようにする。
既定値は「毎回確認する」。

#### 選択範囲だけ並び替え

複数選択した項目だけを、名前順、種類順、追加順、更新順などで並び替えられるようにする。
選択範囲ソートの結果も、まず編集中の手動並び順へ反映する。
正式保存は `並び順を保存` を押した時だけ行う。

---

### 8.4 右側ボタン

主要操作は右側ボタンに配置する。

- フォルダ追加
- ファイル追加
- URL追加
- テンプレート追加
- 選択した項目を開く
- 選択した本文/URL/パスをコピー
- 編集
- 詳細
- 選択解除
- 上に移動
- 下に移動
- 削除

削除ボタンは危険操作として目立つ色にしてよい。

操作ボタン表示モードを用意する。

| モード | 内容 | 用途 |
|---|---|---|
| BeginnerText | 文字つきボタン | 初期値。初心者向け |
| CompactIcon | アイコンボタン中心 | 省スペース。ToolTip/ホバー説明を使う |
| TextAndIcon | アイコン＋文字 | 将来候補 |

設定画面で切り替えられるようにする。

### 8.5 下部説明

下部に短い説明を表示する。

```text
ファイル・フォルダ・URL・選択テキストをここにドラッグ＆ドロップすると登録できます。
項目を左のグループへドラッグするとコピー/移動できます。
右クリックで詳細操作を表示できます。
```

### 8.6 ステータス表示

成功系の通知はステータス表示を使う。

例。

- コピーしました
- 3件を開きました
- 項目を追加しました
- 既に登録されている項目を表示しました
- 2件をスキップしました

### 8.7 サムネイル表示

画像/動画項目は、将来的にサムネイル表示へ対応する。

候補モード。

```csharp
public enum ItemVisualMode
{
    List,
    ThumbnailList,
    LargeThumbnail
}
```

初期実装では画像サムネイルを優先する。
動画サムネイルは次段階とし、Windows Shellサムネイルの利用を優先する。
FFmpeg同梱はライセンス、配布サイズ、速度への影響があるため慎重に判断する。

サムネイル読み込みに失敗した場合は、通常の種類アイコンへフォールバックする。
大量の画像でUIが固まらないよう、キャッシュや非同期読み込みを検討する。

---

## 9. アイコン仕様

### 9.1 アイコン方針

アイコンは作者制作の猫アイコンを使用する。

テイスト。

- v1継承の可愛い猫スタンプ風
- v2では少し洗練
- 背景透過
- 32px表示でも判別できる
- 統一感を持たせる
- 作者製ツールと分かる個性を残す

### 9.2 必要アイコン

| アイコン | 用途 |
|---|---|
| icon_cat_folder | フォルダ |
| icon_cat_file | 通常ファイル |
| icon_cat_image | 画像 |
| icon_cat_video | 動画 |
| icon_cat_url | URL |
| icon_cat_template | テンプレート |
| icon_app | アプリアイコン |
| icon_tray | タスクトレイ |

### 9.3 素材権利

公開READMEまたは利用規約に以下を記載する。

```text
ContextBinder本体、同梱アイコン、画像素材の無断転載・再配布・素材としての流用を禁止します。
```

---

## 10. 項目種類

### 10.1 種類

```csharp
public enum BinderItemType
{
    Folder,
    File,
    Image,
    Video,
    Url,
    Template
}
```

### 10.2 画像ファイル判定

以下の拡張子は Image として扱う。

- `.png`
- `.jpg`
- `.jpeg`
- `.gif`
- `.bmp`
- `.webp`
- `.tiff`

### 10.3 動画ファイル判定

以下の拡張子は Video として扱う。

- `.mp4`
- `.mov`
- `.avi`
- `.mkv`
- `.webm`
- `.wmv`
- `.m4v`

### 10.4 通常ファイル扱い

以下は通常ファイルとして扱う。

- `.txt`
- `.pdf`
- `.docx`
- `.xlsx`
- `.zip`
- `.exe`
- その他、画像・動画に該当しないファイル

---

## 11. 種類表示モード

```csharp
public enum TypeDisplayMode
{
    IconAndText,
    IconOnly,
    TextOnly,
    Hidden
}
```

| モード | 内容 | 初期値 |
|---|---|---|
| IconAndText | アイコンと文字を両方表示 | ○ |
| IconOnly | アイコンのみ表示 | |
| TextOnly | 文字のみ表示 | |
| Hidden | 種類表示を非表示 | |

Hidden は分かりづらくなるため非推奨とする。
選択時には確認ダイアログを表示する。

---

## 12. データモデル

### 12.1 ContextBinderStore

```csharp
public sealed class ContextBinderStore
{
    public int Version { get; set; } = 2;
    public List<BinderGroup> Groups { get; set; } = new();
    public AppSettings Settings { get; set; } = new();
    public List<DeletedItemRecord> Trash { get; set; } = new();
    public List<DeletedItemRecord> DeleteHistory { get; set; } = new();
}
```

### 12.2 BinderGroup

```csharp
public sealed class BinderGroup
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string Name { get; set; } = "";
    public int SortOrder { get; set; }
    public List<BinderItem> Items { get; set; } = new();
}
```

### 12.3 BinderItem

```csharp
public sealed class BinderItem
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string Title { get; set; } = "";
    public BinderItemType Type { get; set; }
    public string PathOrUrl { get; set; } = "";
    public string TemplateBody { get; set; } = "";
    public int SortOrder { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
```

### 12.4 AppSettings

```csharp
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

public enum TrashRetentionMode
{
    AutoDeleteAfterDays,
    ManualOnly
}

public sealed class AppSettings
{
    public bool FirstRunCompleted { get; set; } = false;

    public TypeDisplayMode TypeDisplayMode { get; set; } = TypeDisplayMode.IconAndText;

    public bool ConfirmTitleOnDropAdd { get; set; } = false;
    public bool ShowStatusAfterDropAdd { get; set; } = true;
    public bool FocusExistingItemOnDuplicate { get; set; } = true;

    public bool EnableGroupDropModifierShortcuts { get; set; } = false;
    public bool ConfirmGroupDropCopyMove { get; set; } = true;

    public bool EnableItemDragReorder { get; set; } = true;
    public ManualReorderInputMode ManualReorderInputMode { get; set; } = ManualReorderInputMode.RightDragOnly;
    public UnsavedOrderBehavior UnsavedOrderBehavior { get; set; } = UnsavedOrderBehavior.Ask;

    public bool EnableExternalFileDropOut { get; set; } = true;
    public bool EnableExternalUrlTextDragOut { get; set; } = true;
    public bool EnableExternalTemplateTextDragOut { get; set; } = true;

    public bool CloseButtonMinimizesToTray { get; set; } = true;
    public bool StartMinimizedToTray { get; set; } = false;
    public bool AutoStartWithWindows { get; set; } = false;
    public bool EnableContextMenuDetails { get; set; } = true;
    public bool ShowBeginnerHints { get; set; } = true;
    public bool ShowIconLegend { get; set; } = true;

    public bool AutoBackupEnabled { get; set; } = true;
    public int MaxBackupCount { get; set; } = 20;

    public bool ConfirmBeforeDelete { get; set; } = true;
    public bool MoveDeletedItemsToTrash { get; set; } = true;
    public TrashRetentionMode TrashRetentionMode { get; set; } = TrashRetentionMode.AutoDeleteAfterDays;
    public int TrashRetentionDays { get; set; } = 30;
    public DateTimeOffset? LastTrashExpirationCheckAt { get; set; }

    public SearchScope DefaultSearchScope { get; set; } = SearchScope.CurrentGroup;
    public bool SearchTemplateBody { get; set; } = true;
    public BinderItemType? DefaultTypeFilter { get; set; } = null;

    public ItemSortMode DefaultItemSortMode { get; set; } = ItemSortMode.Manual;
}
```

`TrashRetentionDays` は、設定画面では1日以上の値に制限する。
初期値は30日とする。

---

### 12.5 SearchScope

```csharp
public enum SearchScope
{
    CurrentGroup,
    AllGroups
}
```

### 12.6 ItemSortMode

```csharp
public enum ItemSortMode
{
    Manual,
    Name,
    Type,
    CreatedAt,
    UpdatedAt
}
```

### 12.7 BinderItem.SortOrder

既に `SortOrder` または類似の手動並び順フィールドがある場合はそれを使う。
なければ `BinderItem` に以下を追加する。

```csharp
public int SortOrder { get; set; }
```

意味。

- Manual並び順で使う。
- 自動ソート時には破壊しない。
- ユーザーがD&D、上下移動、現在順を保存した時に更新する。

### 12.8 DeletedItemRecord

登録のごみ箱に入った項目は、元グループ情報と削除日時を持つ。

```csharp
public sealed class DeletedItemRecord
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string OriginalGroupId { get; set; } = "";
    public string OriginalGroupName { get; set; } = "";
    public BinderItem Item { get; set; } = new();

    public DateTimeOffset DeletedAt { get; set; } = DateTimeOffset.UtcNow;
}
```

日時は内部的にはUTC基準で保持する。
表示時はローカル時刻へ変換する。

---


## 13. 登録操作

### 13.1 D&Dファースト

ContextBinder v2 は、登録操作をD&D中心に設計する。

中央一覧へ以下をドラッグ＆ドロップできる。

- フォルダ
- 通常ファイル
- 画像ファイル
- 動画ファイル
- URL
- 選択テキスト

右側追加ボタンは補助操作として残す。

### 13.2 D&D登録時のタイトル

初期設定では、自動タイトルで即登録する。

| 種類 | 自動タイトル |
|---|---|
| Folder | フォルダ名 |
| File | 拡張子なしファイル名 |
| Image | 拡張子なしファイル名 |
| Video | 拡張子なしファイル名 |
| Url | ページタイトル取得、失敗時URL |
| Template | 先頭30文字 |

オプションで、D&D追加時にタイトル確認を表示できる。

### 13.3 URLとテキスト

- URL形式のテキストは Url として登録する。
- URLではないテキストは Template として登録する。

---

## 14. 重複登録ルール

表示タイトル、ファイル名、フォルダ名の重複は許可する。

同一グループ内で重複登録を禁止する対象は、実体の参照先が同じ項目のみとする。

対象。

- Folder
- File
- Image
- Video
- Url

同じファイル名でもパスが違えば登録可能。

```text
main.png   C:\ProjectA\main.png
main.png   C:\ProjectB\main.png
```

他グループに同じ `PathOrUrl` が存在する場合は、別グループ内の別項目として登録を許可する。

重複時は以下を行う。

1. 「既に登録されています」と表示する。
2. 既存行を選択する。
3. DataGridViewを該当行までスクロールする。
4. 行を一時的にハイライトする。
5. ステータスに表示する。

Template は本文が同じでも登録可能とする。
ただし、同一グループ内でタイトルが完全一致する場合は、将来的に警告表示を検討する。

---

## 15. D&D設計

### 15.1 内部D&D形式

内部D&Dでは、独自形式を使用する。

```text
ContextBinder.ItemRefs.v2
```

内容。

```json
{
  "sourceGroupId": "group-id",
  "itemIds": ["item-id-1", "item-id-2"]
}
```

### 15.2 外部D&D形式

ドラッグ開始時に、DataObjectへ複数形式を設定する。

#### File / Image / Video / Folder

- `ContextBinder.ItemRefs.v2`
- `DataFormats.FileDrop`
- `DataFormats.UnicodeText`

#### Url

- `ContextBinder.ItemRefs.v2`
- `DataFormats.UnicodeText`
- `DataFormats.Text`

#### Template

- `ContextBinder.ItemRefs.v2`
- `DataFormats.UnicodeText`
- `DataFormats.Text`

内部の受け取り側は `ContextBinder.ItemRefs.v2` を最優先で解釈する。

### 15.3 中央一覧へドロップ

外部からのドロップは新規登録として扱う。

```text
ファイル / フォルダ / URL / 選択テキスト
→ 現在選択中のグループへ追加
```

内部項目の並び替えは、既定では右ボタンドラッグで行う。
左ドラッグは、外部D&Dや外部アプリへの受け渡しと衝突しやすいため、初期設定では手動並び替えに使わない。

設定で `LeftOrRightDrag` を選んだ場合のみ、左ドラッグでも内部並び替えを許可する。

---

### 15.4 左グループ一覧へドロップ

内部項目が左グループ一覧へドロップされた場合、コピー/移動として扱う。

| 操作 | 挙動 |
|---|---|
| Ctrl + ドロップ | コピー |
| Shift + ドロップ | 移動 |
| 通常ドロップ | コピー/移動確認 |

ただし、`EnableGroupDropModifierShortcuts == false` の場合は、Ctrl/Shiftを押していても確認ダイアログを表示する。

### 15.5 外部へドラッグ

- File / Image / Video / Folder は外部へ FileDrop として渡す。
- Url はURL文字列として渡す。
- Template は本文テキストとして渡す。

受け取り側アプリが対応していない場合は何も起きない。
確実に使う場合は右クリックまたはボタンからコピーする。

---

## 16. 右クリックメニュー

項目一覧を右クリックすると、種類に応じたメニューを表示する。

### 16.1 File / Image / Video

```text
開く
置いてあるフォルダを開く
エクスプローラーで選択して開く
パスをコピー
タイトルをコピー
---
編集
複製
他グループへコピー
他グループへ移動
---
詳細
削除
```

### 16.2 Folder

```text
フォルダを開く
パスをコピー
タイトルをコピー
---
編集
複製
他グループへコピー
他グループへ移動
---
詳細
削除
```

### 16.3 Url

```text
URLを開く
URLをコピー
タイトルをコピー
---
編集
複製
他グループへコピー
他グループへ移動
---
詳細
削除
```

### 16.4 Template

```text
本文をコピー
タイトルをコピー
---
編集
複製
他グループへコピー
他グループへ移動
---
詳細
削除
```

---

## 17. 詳細ダイアログ

右クリックまたは詳細ボタンから表示する。

### 17.1 ファイル系詳細

表示内容。

- タイトル
- 種類
- パス
- 所属グループ
- ファイル存在チェック
- 作成日時
- 更新日時

操作ボタン。

- 開く
- 置いてあるフォルダを開く
- パスをコピー
- 情報をまとめてコピー

### 17.2 URL詳細

表示内容。

- タイトル
- 種類
- URL
- 所属グループ
- 作成日時
- 更新日時

操作ボタン。

- URLを開く
- URLをコピー
- 情報をまとめてコピー

### 17.3 テンプレート詳細

表示内容。

- タイトル
- 種類
- 所属グループ
- 本文プレビュー
- 文字数
- 作成日時
- 更新日時

操作ボタン。

- 本文をコピー
- 編集
- 情報をまとめてコピー

---

## 18. ファイルを開く処理

File / Image / Video / Folder を開く場合は、Windows標準の関連付けに従う。

C#実装方針。

```csharp
Process.Start(new ProcessStartInfo
{
    FileName = item.PathOrUrl,
    UseShellExecute = true
});
```

既定アプリが設定されている場合は、そのアプリで開く。
既定アプリが設定されていない場合は、Windows側の「どのアプリで開きますか」画面、または同等の挙動に委ねる。

アプリ側では独自の関連付け管理は行わない。

---

## 19. 置いてあるフォルダを開く

ファイルの場合。

```text
explorer.exe /select,"C:\path\file.png"
```

フォルダの場合。

```text
そのフォルダを開く
```

ファイルが存在しない場合は、親フォルダが存在すれば親フォルダを開く選択肢を検討する。

---

## 20. ファイル存在チェック

対象。

- Folder
- File
- Image
- Video

対象外。

- Url
- Template

URLの生存チェックは初期版では行わない。
Templateは常に正常扱いとする。

表示。

| 状態 | 表示 |
|---|---|
| 存在する | 通常表示 |
| 存在しない | 警告アイコン、灰色表示 |
| 未チェック | 通常表示または未確認表示 |

右クリックに「存在チェックを再実行」を追加してもよい。

---

## 21. 検索・絞り込み

### 21.1 検索範囲

検索範囲を選べる。

- 現在のグループ
- 全体

### 21.2 絞り込み

種類で絞り込める。

- すべて
- フォルダ
- ファイル
- 画像
- 動画
- URL
- テンプレート

### 21.3 検索対象

- タイトル
- パス/URL
- テンプレート本文
- 種類
- グループ名（全体検索時）

検索中も右クリック操作、開く、コピーなどの基本操作を利用できる。

---

## 22. 削除、安全策、Undo、ごみ箱

### 22.1 削除前確認

削除前に確認する。

例。

```text
選択した5件の登録を、ContextBinderのごみ箱へ移動します。
元のファイル、フォルダー、画像、動画は削除されません。
```

### 22.2 Undo

初期版では直前1回のUndoを目標とする。

対象候補。

- 項目削除
- グループ削除
- 項目移動
- 項目並び替え

### 22.3 登録のごみ箱

ContextBinder v2 のごみ箱は、Windowsのごみ箱ではなく、アプリ内の「登録のごみ箱」として扱う。

重要な説明：

```text
ここで削除されるのは、ContextBinderへの登録情報だけです。
元のファイル、フォルダー、画像、動画は削除されません。
```

この説明は、削除確認、ごみ箱画面上部、設定画面、ごみ箱を空にする確認で表示する。

### 22.4 表記ルール

実ファイル削除と誤認されないよう、UI文言は次を優先する。

| 避けたい表記 | 推奨表記 |
|---|---|
| 削除 | 登録をごみ箱へ |
| 復元 | 登録を元に戻す |
| 完全削除 | ごみ箱から登録を削除 |
| ごみ箱を空にする | ごみ箱内の登録情報をすべて削除 |

削除確認の例：

```text
選択した3件の登録を、ContextBinderの登録のごみ箱へ移動します。

元のファイルやフォルダーは削除されません。

[登録をごみ箱へ]
[キャンセル]
```

ContextBinder v2 は、元ファイルそのものを削除する機能を持たない。

### 22.5 保持期限

登録のごみ箱は、次の保持モードを持つ。

| モード | 内容 |
|---|---|
| 指定日数後に自動削除 | `DeletedAt` から指定日数を過ぎた登録情報を削除する |
| 手動でのみ削除 | ユーザーが削除するまで保持する |

初期値：

```text
モード：指定日数後に自動削除
保持日数：30日
```

期限切れチェックは次のタイミングで行う。

- ContextBinder起動時
- 起動中は1日1回

ContextBinderを起動していない間は削除処理は走らない。
起動後のチェックで期限切れ登録があれば、ごみ箱から登録情報だけを削除する。
元のファイル、フォルダー、画像、動画は削除しない。

期限切れ削除が行われた場合は、ステータス表示で通知する。

```text
期限を過ぎた登録3件を、ごみ箱から自動削除しました。元のファイルは削除されていません。
```

長時間起動し続ける場合も、1日1回だけ確認すればよい。
毎分チェックなどは行わない。

### 22.6 復元

登録をごみ箱から復元する場合：

```text
元グループが存在する
→ 元グループへ復元

元グループが削除済み
→ 未分類へ復元
```

一度復元した登録を再度ごみ箱へ入れる場合は、その時点で `DeletedAt` を更新する。
過去の削除日時は引き継がない。

### 22.7 グループ削除時

グループ削除時は、登録情報がどうなるかを明確にする。

```text
グループ「○○」を削除します。

中の登録をどうしますか？
[未分類へ移動]
[登録のごみ箱へ移動]
[キャンセル]

元のファイルやフォルダーは削除されません。
```

---


## 23. バックアップ

### 23.1 基本

自動バックアップを行う。

ただし、自動バックアップは同じPC内の保険である。
別ドライブやクラウドへ保存したいユーザー向けに手動エクスポートを用意する。

### 23.2 タイミング

バックアップタイミング。

- 起動時
- 保存前
- アプリ終了時
- インポート前

### 23.3 保存先

保存場所モードに従う。

標準モード。

```text
%AppData%\ContextBinder\backups\
```

アプリフォルダ保存。

```text
ContextBinder_Data\backups\
```

カスタム保存。

```text
指定フォルダ\backups\
```

### 23.4 保持数

初期値は最大20件。

設定画面で変更可能にする。

---

## 24. インポート / エクスポート

### 24.1 エクスポート

登録内容を書き出せる。

- 全体エクスポート
- 保存先をユーザーが選択
- 別ドライブやクラウドへの保管を推奨する説明を表示

### 24.2 インポート

初期版では以下に対応する。

- 現在の登録内容を置き換える
- 現在の登録内容へ追加する

追加インポート時は重複チェックを行う。

### 24.3 表記

ユーザー向けには「データをエクスポート」ではなく「登録内容を書き出す」と表記する。

---

## 25. 設定画面

設定画面を用意する。

### 25.1 表示

- 種類表示：アイコン＋文字 / アイコンのみ / 文字のみ / 非表示
- アイコン凡例を表示
- 初心者向け説明を表示
- 操作ボタン表示モード：初心者向け / コンパクト
- 項目表示モード：通常一覧 / サムネイル一覧 / 大きめサムネイル
- サムネイルサイズ

### 25.2 D&D

- D&D追加時にタイトルを確認する
- Ctrl+グループドロップでコピー
- Shift+グループドロップで移動
- 通常グループドロップ時に確認する
- 項目のD&D並び替えを有効にする
- URLを外部D&Dでテキストとして渡す
- テンプレートを外部D&Dで本文として渡す

### 25.3 検索

- 検索対象：現在のグループ / 全体
- テンプレート本文も検索対象にする

### 25.4 バックアップ

- 自動バックアップを有効にする
- 保持数
- バックアップフォルダを開く
- 登録内容を書き出す

### 25.5 削除と復元

```text
削除と復元

削除した登録：
○ 30日後に自動削除する
○ 自動削除せず、手動でのみ削除する

保存日数：[ 30 ] 日

※削除されるのはContextBinderへの登録情報だけです。
  元のファイル、フォルダー、画像、動画は削除されません。
```

自動削除OFF時は、保存日数入力を無効化する。

---

### 25.6 常駐

- 閉じるボタンでタスクトレイに格納する
- 起動時に最小化
- Windows起動時に自動起動
- Windows起動時自動起動を解除する
- スタートメニューに登録する
- スタートメニュー登録を解除する
- スタートメニュー / 自動起動ショートカットを修復する

スタートメニュー登録とWindows起動時自動起動は準MVP対象とする。
実装時は、UIから直接ショートカットを操作せず、`ShortcutService` または `StartupRegistrationService` のようなServiceへ分離する。
保存先はユーザー単位のStart Menu Programs配下、およびStartupフォルダを基本とする。
AppSettingsには必要に応じて以下を追加する。

- `RegisterStartMenuShortcut`
- `AutoStartWithWindows`
- `StartMinimizedToTray`
- `LastShortcutTargetPath`

ただし、実際のショートカット有無と settings.json の値がずれる可能性があるため、起動時または設定画面表示時に実ファイル状態も確認する。

---

## 26. タスクトレイ

ContextBinder v2 は常駐アプリとして動作する。

- 最小化でタスクトレイへ格納
- 閉じるボタンでも設定によりタスクトレイへ格納
- タスクトレイアイコン右クリックでメニュー表示

```text
開く
終了
```

終了時は NotifyIcon を確実に破棄し、ゴーストアイコンを残さない。

---

## 27. 空状態ガイド

### 27.1 グループなし

```text
まずはグループを作成してください。
例：TRPG、仕事、動画素材、テンプレート
```

### 27.2 項目なし

```text
まだ項目がありません。
ファイル・フォルダ・URL・テキストをここにドラッグ＆ドロップすると登録できます。
または右側の追加ボタンから登録できます。
```

---

## 28. ツールチップ

主要ボタンにツールチップを設定する。

例。

```text
フォルダ追加：
フォルダを選んで現在のグループへ登録します。

選択した項目を開く：
選択中のファイル・フォルダ・URLをまとめて開きます。
テンプレートはコピーされます。
```

---

## 29. README方針

公開READMEには利用者が困らない最低限を書く。

書くもの。

- 起動方法
- 初回設定
- 保存場所の説明
- 登録されるもの/されないもの
- バックアップとエクスポートの案内
- 利用規約
- アイコン素材の無断流用禁止

書かないもの。

- 内部設計
- JSONスキーマ詳細
- D&D独自形式
- Codex向け指示
- 詳細な実装方針

---

## 30. 利用規約・権利表記

READMEまたは利用規約に以下を記載する。

```text
ContextBinder本体、同梱アイコン、画像素材の無断転載・再配布・素材としての流用を禁止します。
```

必要に応じて、以下も記載する。

- 個人利用/商用利用の範囲
- 再配布禁止
- 改変・解析・素材流用禁止
- 免責事項

---

## 31. フォーム一覧

### 31.1 MVP必須

- MainForm
- FirstRunSetupForm
- SettingsForm
- ItemEditForm
- TemplateEditForm
- ItemDetailForm
- CopyMoveDialog

### 31.2 初期に欲しい

- RecycleBinForm
- ImportExportDialog

### 31.3 後からでもよい

- BackupRestoreDialog

---

## 32. サービス構成

WinFormsのイベントハンドラに処理を集約しすぎない。

推奨構成。

```text
ContextBinder/
  Models/
    BinderItem.cs
    BinderGroup.cs
    ContextBinderStore.cs
    AppSettings.cs

  Services/
    StoreService.cs
    BackupService.cs
    StorageLocationService.cs
    FileTypeDetector.cs
    ItemActionService.cs
    DragDropService.cs
    ClipboardService.cs
    SearchService.cs
    TrashService.cs

  Forms/
    MainForm.cs
    FirstRunSetupForm.cs
    SettingsForm.cs
    ItemEditForm.cs
    TemplateEditForm.cs
    ItemDetailForm.cs
    CopyMoveDialog.cs
    RecycleBinForm.cs
    ImportExportDialog.cs

  Assets/
    icon_cat_folder.png
    icon_cat_file.png
    icon_cat_image.png
    icon_cat_video.png
    icon_cat_url.png
    icon_cat_template.png
```

### 32.1 役割

| クラス | 役割 |
|---|---|
| StoreService | JSON保存/読み込み |
| BackupService | バックアップ/復元 |
| StorageLocationService | 保存場所決定 |
| FileTypeDetector | 種類判定 |
| ItemActionService | 開く、コピー、詳細操作 |
| DragDropService | D&Dデータ作成/解釈 |
| ClipboardService | クリップボード操作 |
| SearchService | 検索/絞り込み |
| TrashService | ごみ箱/削除履歴 |
| ItemOrderingService | 手動並び順の正規化、上へ移動、下へ移動、D&D並び替え、現在順の保存 |
| ItemSortService | SortModeに応じた表示順作成、選択範囲内ソート |

### 32.2 項目並び替えService方針

MainFormに並び替え処理を直書きしない。

候補：

- `ItemOrderingService`
- `ItemSortService`

責務：

- 保存済み手動並び順の正規化
- 編集中の手動並び順の作成と保持
- 右ドラッグD&D並び替え
- 上へ移動
- 下へ移動
- 保存前に戻す
- 並び順を保存
- 一つ前の保存済み手動並び順への復元
- 表示用ソートの作成
- 表示用ソート中に手動変更した場合のドラフト化
- 選択範囲内ソート
- 未保存確認の要否判定

### 32.3 追加テスト候補

並び替え：

- Manual順で表示される
- Name順へ切り替えてもSortOrderは壊れない
- Type順へ切り替えてもSortOrderは壊れない
- 手動並び順へ戻すと保存済み手動順に戻る
- 右ドラッグ後は未保存状態になる
- 上へ移動で編集中の手動並び順が更新される
- 下へ移動で編集中の手動並び順が更新される
- 並び順を保存するとSortOrderが更新される
- 保存前に戻すと最後に保存した手動順へ戻る
- 未保存変更がない時は未保存表示が出ない
- 未保存変更がある時だけ未保存表示が出る
- 未保存確認で「次回から自動適用」を選ぶと設定へ保存される
- 選択範囲だけ名前順にできる
- 選択外の項目位置が不必要に崩れない

登録のごみ箱：

- 登録のごみ箱へ移動しても元ファイルは削除されない
- DeletedAtが保存される
- 30日を過ぎた登録情報だけ自動削除される
- 手動削除のみ設定では期限切れ削除されない
- 起動時チェックで期限切れ登録が削除される
- 起動中1日1回チェックで期限切れ登録が削除される
- 1日以内に同じ期限チェックが繰り返し走らない
- 復元時に元グループがなければ未分類へ戻る

---


## 33. MVP範囲

### 33.1 MVP必須

- C# WinForms
- DataGridView
- グループ管理
- D&D追加
- 右側ボタン追加
- 右クリックメニュー
- テンプレート登録/コピー
- 猫アイコン
- JSON保存
- 保存場所選択
- 重複チェック
- タスクトレイ
- 初回セットアップ
- 設定画面
- 自動バックアップ
- 削除前確認

### 33.2 MVPに入れてもよい

- 検索
- 絞り込み
- ファイル存在チェック
- 詳細ダイアログ
- インポート/エクスポート
- ツールチップ
- 空状態ガイド
- ごみ箱
- Undo

### 33.3 後回し

- 自動アップデート
- テーマ変更
- タグ
- クラウド同期
- 複雑なテンプレート変数置換
- URL生存チェック
- 複数プロファイル
- プラグイン機構
- グローバルホットキー

---

## 34. 非目標

v2初期版では以下は対象外とする。

- クラウド同期
- 自動アップデート
- 複雑な権限管理
- 複数ユーザープロファイル
- プラグイン機構
- 実ファイルの複製管理
- 登録元ファイルのバックアップ

ContextBinderは参照先を管理するツールであり、実ファイルのバックアップツールではない。

---

## 35. 将来拡張候補

- テンプレート変数置換
  - `{{date}}`
  - `{{time}}`
  - `{{clipboard}}`
- タグ
- 最近使った項目
- ピン留め
- グループ単位エクスポート
- アイコン差し替え機能
- テーマ変更
- キーボードショートカット
- URL生存チェック
- グローバルホットキー
- インストーラー版
