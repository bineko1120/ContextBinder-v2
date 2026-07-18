# ContextBinder v2 UI画面設計マップ

この文書は、少佐が `MainFormDesignDraft` を Visual Studio Designer で組み立てるときに使う、現在の画面設計図です。

- 少佐：画面構造、配置、操作導線、表示文言を決める
- ChatGPT：要件整理、デザイン設計、受け入れ条件、Codex向け指示を作る
- Codex：機能接続、表示切替、保存、検証、最終的な寸法調整を行う
- `WinFormsUiViewer.exe`：Designerで作ったBeginner／Compact表示を個別に確認する

各部品の正確な内部名は [`UI_CONTROL_CONTRACT.md`](UI_CONTROL_CONTRACT.md)、テーマ方針は [`UI_THEME_GUIDE.md`](UI_THEME_GUIDE.md) を参照してください。

---

# 1. 現在の確定方針

## 1.1 Designerは配置用の正本

Visual Studio Designerでは、主に次を決めます。

- コントロールの親子関係
- 左、中央、右、上、下の大まかな配置
- `Name`
- 仮の表示文言
- 大枠の `Dock` と `Anchor`
- Beginner用とCompact用の所属

正確な幅、高さ、`Margin`、`Padding`、文字切れ調整、テーマ別装飾は、配置後にCodexと実画面を確認しながら整えます。

基準フォントは暫定的に次とします。

```text
Yu Gothic UI 10pt
```

## 1.2 Beginner／CompactはMainForm内のPanelで分ける

現在は、Beginner表示とCompact表示のためにUserControlを分けません。

```text
MainForm内の共通Panel
├─ Beginner専用Panel
└─ Compact専用Panel
```

理由：

- MainFormのDesigner内で配置を直接確認できる
- UserControl編集後の再ビルド待ちを減らせる
- 親領域とのサイズ差やはみ出しを把握しやすい
- `WinFormsUiViewer.exe` のTagフィルターで各表示を確認できる

## 1.3 UserControl案の扱い

試行中に作成した次のUserControl類は、現在のMainForm設計の実行基準ではありません。

```text
BeginnerActionView
CompactActionView
BeginnerGroupAddView
CompactGroupAddView
groupAreaView
```

- 現在のMainFormへ新たに接続しない
- Codexは勝手に削除しない
- MainForm完成後、不要であることを確認してから整理する

---

# 2. WinFormsUiViewerによる表示確認

`WinFormsUiViewer.exe` は、コントロールの `Tag` を見てBeginner／Compactの表示プロファイルを切り替えます。

## 2.1 使用するTag

| 表示対象 | Tag |
|---|---|
| Beginner専用 | `view:Beginner` |
| Compact専用 | `view:Compact` |
| 両方に表示する共通部品 | 表示モード用Tagを付けない |

大文字・小文字を含め、この表記をそのまま使います。

## 2.2 表示プロファイル

| プロファイル | 表示内容 |
|---|---|
| `All` | Beginner、Compact、共通部品をすべて確認する |
| `Beginner` | 共通部品＋`view:Beginner`だけを確認する |
| `Compact` | 共通部品＋`view:Compact`だけを確認する |

Visual Studio Designerでは `Visible = false` にしても編集用に表示されることがあります。設計時の表示確認は `Visible` ではなく、WinFormsUiViewerのプロファイルを使います。

## 2.3 親PanelへTagを付ける

右側操作のように領域全体が表示モード専用なら、親PanelにTagを付けます。

```text
beginnerActionPanel
Tag = view:Beginner

itemCompactActionPanel
Tag = view:Compact
```

親Panelがプロファイルから除外されれば、中のボタンもまとめて非表示になるため、子ボタンすべてへ同じTagを付ける必要はありません。

## 2.4 共通Panel内の一部だけ切り替える

グループ見出しのように、共通Panel内へBeginnerボタンとCompactボタンが同居する場合は、各ボタンへTagを付けます。

```text
addGroupButton
Tag = view:Beginner

addGroupIconButton
Tag = view:Compact
```

---

# 3. 現在のMainForm構造

現在のDesigner構造を基準として、次の形で進めます。

```text
MainFormDesignDraft
├─ mainMenuStrip
└─ mainSplitContainer
   ├─ Panel1：グループ領域
   │  ├─ groupHeaderPanel
   │  │  ├─ groupTitleLabel
   │  │  ├─ addGroupButton            Tag: view:Beginner
   │  │  └─ addGroupIconButton        Tag: view:Compact
   │  └─ groupListBox
   │
   └─ Panel2：作業領域
      └─ workAreaPanel
         ├─ rightActionHostPanel
         │  ├─ beginnerActionPanel    Tag: view:Beginner
         │  └─ itemCompactActionPanel Tag: view:Compact
         │
         └─ itemAreaPanel
            ├─ searchAndViewPanel
            │  ├─ searchHeaderPanel
            │  │  ├─ searchSectionTitleLabel
            │  │  └─ toggleSearchPanelButton
            │  └─ searchAndViewContentPanel
            │     ├─ searchInputPanel
            │     ├─ searchScopePanel
            │     └─ typeFilterPanel
            │        ├─ typeFilterTitleLabel
            │        └─ typeFilterFlowPanel
            ├─ itemGridView
            └─ bottomInformationPanel
```

`itemGridView`など、まだDesignerへ置いていない部品は今後追加します。

---

# 4. 参考画面

## 4.1 Beginner表示

```text
┌─────────────────────────────────────────────────────────────┐
│ ファイル  登録  編集  表示  ツール  ヘルプ                 │
├──────────────┬────────────────────────────┬─────────────────┤
│ グループ一覧 │ 検索フォーム        [開く▼]│ 操作            │
│ [追加]       ├────────────────────────────┤ [グループ追加]  │
│              │ 項目一覧                   │ [ファイル追加]  │
│ TRPG         │                            │ [フォルダー追加]│
│ 動画編集     │                            │ [URL追加]       │
│ 開発         │                            │ [テンプレート]  │
│              │                            │                 │
│              │                            │ [開く]          │
│              │                            │ [コピー]        │
│              │                            │ [編集]          │
│              │                            │ [詳細]          │
│              │                            │                 │
│              │                            │ [登録をごみ箱へ]│
│              ├────────────────────────────┤                 │
│              │ 状態・ヒント・アイコン説明│                 │
└──────────────┴────────────────────────────┴─────────────────┘
```

## 4.2 Compact表示

Compact操作は上部へ横並びにせず、右側へ縦に配置します。

```text
┌───────────────────────────────────────────────────────┐
│ ファイル  登録  編集  表示  ツール  ヘルプ           │
├──────────────┬──────────────────────────────┬─────────┤
│ グループ  ＋ │ 検索フォーム          [開く▼]│ ＋書類  │
│──────────────│──────────────────────────────│ ＋Folder│
│ TRPG         │ 項目一覧                     │ ＋URL   │
│ 動画編集     │                              │ ＋文    │
│ 開発         │                              │─────────│
│              │                              │ 開く    │
│              │                              │ コピー  │
│              │                              │ 編集    │
│              │                              │ 詳細    │
│              │                              │─────────│
│              │                              │ ごみ箱  │
│              │──────────────────────────────│         │
│              │ 状態・ヒント・アイコン説明  │         │
└──────────────┴──────────────────────────────┴─────────┘
```

上部には、検索、検索範囲、絞り込み、並び方、表示方法などを置きます。追加、開く、コピー、編集、ごみ箱などの主要操作は右側へ分離し、上部での誤クリックを減らします。

---

# 5. 各領域の役割

## 5.1 `mainMenuStrip`

日本語：上部メニューバー。

```text
ファイル / 登録 / 編集 / 表示 / ツール / ヘルプ
```

- Beginner／Compactの両方で共通表示
- 表示モード用Tagは付けない
- `Dock = Top`

## 5.2 `mainSplitContainer`

日本語：左のグループ領域と右の作業領域を分ける境界。

- 利用者が実行中に境界を左右へドラッグできる
- グループ一覧の幅を利用者が調整できる
- `Dock = Fill`
- `IsSplitterFixed = false`
- 最小幅、初期幅、前回幅の保存は後でCodexが調整する

## 5.3 グループ領域

### `groupHeaderPanel`

日本語：グループ一覧の見出しと追加操作を置く帯。

共通：

```text
groupTitleLabel
```

Beginner専用：

```text
addGroupButton
Tag = view:Beginner
```

Compact専用：

```text
addGroupIconButton
Tag = view:Compact
```

### `groupListBox`

日本語：登録グループの一覧。

- Beginner／Compact共通
- `Dock = Fill`を基本とする
- Splitterを動かした時に横幅が追従する

## 5.4 `workAreaPanel`

日本語：項目一覧と右側操作をまとめる右側全体。

```text
workAreaPanel
├─ itemAreaPanel
└─ rightActionHostPanel
```

## 5.5 `rightActionHostPanel`

日本語：右側操作パネルの共通置き場所。

```text
rightActionHostPanel
├─ beginnerActionPanel
└─ itemCompactActionPanel
```

- `rightActionHostPanel`自体は共通でTagを付けない
- 子Panelは同じ位置へ置き、どちらも `Dock = Fill` を基本とする
- 実行時とViewerでは片方だけ表示する
- Beginner時は広め、Compact時は狭めに幅を変更する
- 正確な幅はMainForm完成後にCodexが調整する

### `beginnerActionPanel`

日本語：文字つき操作ボタンを縦に並べる領域。

```text
Tag = view:Beginner
```

主な内容：

- グループ追加
- ファイル追加
- フォルダー追加
- URL追加
- テンプレート追加
- 開く
- コピー
- 編集
- 詳細
- 登録をごみ箱へ

### `itemCompactActionPanel`

日本語：コンパクト操作を右側へ縦に並べる領域。

```text
Tag = view:Compact
```

主な内容：

- ファイル追加アイコン
- フォルダー追加アイコン
- URL追加アイコン
- テンプレート追加アイコン
- 開く
- コピー
- 編集
- 詳細
- 登録をごみ箱へ

追加、選択項目操作、危険操作の間には、見出しや区切りを入れます。ごみ箱操作は他のボタンから離します。

---

# 6. 検索フォーム

## 6.1 通常時は見出しだけ表示

検索フォームは、起動直後には閉じた状態とします。

```text
検索フォーム                              [開く▼]
```

絞り込み中は、閉じた状態でも条件が有効であることを表示します。

```text
検索フォーム  ●絞り込み中                 [開く▼] [解除]
```

## 6.2 開いた状態

```text
検索フォーム                              [閉じる▲]
検索：[　　　　　　　　　　　　　] [検索] [解除]
検索範囲：[現在のグループ▼]
絞り込み種類
□すべて □フォルダー □ファイル □画像 □動画 □URL □テンプレート
```

## 6.3 コンテナ構造

```text
searchAndViewPanel
├─ searchHeaderPanel
│  ├─ searchSectionTitleLabel
│  └─ toggleSearchPanelButton
└─ searchAndViewContentPanel
   ├─ searchInputPanel
   ├─ searchScopePanel
   └─ typeFilterPanel
      ├─ typeFilterTitleLabel
      └─ typeFilterFlowPanel
```

## 6.4 自動的に下へ伸びる挙動

```text
ウィンドウ幅が狭くなる
↓
種類チェックボックスが折り返す
↓
typeFilterFlowPanelの高さが増える
↓
typeFilterPanelの高さが増える
↓
searchAndViewContentPanelの高さが増える
↓
searchAndViewPanel全体が下へ伸びる
```

Designerでは構造と大まかな設定を作り、必要な高さ再計算はCodexが補強します。

## 6.5 種類絞り込み

種類はComboBoxではなく、複数選択できるCheckBox群を使います。

```text
□すべて
□フォルダー
□ファイル
□画像
□動画
□URL
□テンプレート
```

- 個別種類はOR条件
- 検索キーワードとはAND条件
- 左寄せでまとまりを保つ
- 横幅が不足した時は次の行へ折り返す
- 「すべて」は全選択・一部選択・未選択を表せる3状態を検討する

---

# 7. 項目一覧と並び替え

## 7.1 `itemGridView`

日本語：選択グループの登録項目一覧。

基本列候補：

- 種類アイコン
- 種類
- タイトル
- 内容・参照先
- 状態
- サムネイル

`Dock = Fill`を基本とし、ウィンドウの拡大・縮小では主にこの領域が伸縮します。

## 7.2 並び替え

必要な操作：

```text
並び方：[手動並び順▼]
[上へ]
[下へ]
[並び順を保存]
[保存前に戻す]
● 並び順に未保存の変更があります
```

- 既定では右ドラッグで手動並び替え
- 上下ボタンでも移動可能
- 移動しただけでは正式保存しない
- 保存済み手動順と一時的な名前順等を分ける
- 未保存状態は変更がある時だけ表示する

並び替え操作を右側へ入れるか、項目一覧付近へ置くかは、配置確認後に決定します。追加・編集操作と密着させず、誤クリックしにくい区画へ分けます。

---

# 8. 下部情報

`bottomInformationPanel`は `itemAreaPanel` の中へ置きます。

これにより、左のグループ領域や右の操作領域へ横幅を干渉させず、中央の下だけに表示できます。

置くもの：

- `statusLabel`：コピー、保存、エラー等の操作結果
- 使い方のヒント
- アイコンの意味
- コンパクト操作へマウスを乗せた時の説明

表示内容が多い場合は、タブや折りたたみを検討します。

---

# 9. ウィンドウと文字サイズ

## 9.1 ウィンドウ拡縮

ウィンドウサイズを変えても、文字を比例拡大・比例縮小しません。

- フォントサイズは維持する
- 左右の基本領域は最小幅を持つ
- 中央の項目一覧が主に伸縮する
- 文字切れが起きる前にMinimumSizeで縮小を止める

## 9.2 文字サイズ設定

将来、設定画面で次を選べるようにします。

```text
小さめ / 標準 / 大きめ
```

基準：

```text
標準 = Yu Gothic UI 10pt
```

文字サイズ変更時は、必要に応じてボタン高さ、入力欄高さ、DataGridView行高、最小ウィンドウサイズを一緒に調整します。

---

# 10. アイコン方針

- `＋`、`↑`、`↓`、`…`など単純操作は記号を利用可能
- ファイル、フォルダー、コピー、編集、ごみ箱等は統一した画像アイコンを優先
- 絵文字はDesigner上の仮表示として利用可能
- 猫テーマでも機能アイコンの意味を壊さない
- 猫らしさは背景、枠、角丸、ホバー、見出し、空状態等で表現する
- 小さな操作アイコンへ複雑な猫装飾を詰め込まない

---

# 11. Designerで今行うこと

1. 共通部品を配置する
2. Beginner専用部品を `beginnerActionPanel` 等へ入れる
3. Compact専用部品を `itemCompactActionPanel` 等へ入れる
4. 正確な `Name` を付ける
5. 専用Panelまたは専用部品へ表示Tagを付ける
6. WinFormsUiViewerの `All`、`Beginner`、`Compact` で確認する
7. 正確なサイズ、余白、フォント階層は後回しにする
8. ボタンのイベント処理はまだ書かない

---

# 12. Codexが後から行うこと

- Beginner／Compactの実行時表示切替
- `rightActionHostPanel`のモード別幅調整
- 検索フォームの開閉
- 種類フィルターの複数選択処理
- 折り返しに応じた高さ再計算
- SplitContainer幅の保存・復元
- 正確なSize、Margin、Padding
- 文字サイズ3段階
- テーマ適用
- ToolTip、AccessibleName
- Presenter / Serviceとの接続
- 高DPI、日本語文字切れ、最小ウィンドウサイズの検証

---

# 13. UI完成後にCodexへ伝える基準

```text
ContextBinder v2 のUIをVisual Studio Designerで作成しました。

- MainForm内のPanel構造を現在の正本としてください。
- Beginner／Compactの表示分離にUserControlを新規導入しないでください。
- view:Beginner と view:Compact のTag契約を維持してください。
- Designer管理のName、親子関係、Location、Size、Dock、Anchorを無断で変更しないでください。
- 同じ機能の文字ボタン、アイコンボタン、MenuStripは同じPresenter処理へ接続してください。
- WinFormsUiViewerのAll／Beginner／Compact表示が維持されるようにしてください。
- UI変更が必要な場合は、変更前に理由と対象を報告してください。
```
