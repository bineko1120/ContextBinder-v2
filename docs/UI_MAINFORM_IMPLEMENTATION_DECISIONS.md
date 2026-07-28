# ContextBinder v2 MainForm機能実装前の確定事項

この文書は、Visual Studio Designerで作成した `MainFormDesignDraft` を機能実装へ接続するときの追加実行基準です。

既存の次の文書と併せて参照します。

- `docs/UI_SCREEN_MAP.md`
- `docs/UI_CONTROL_CONTRACT.md`
- `docs/UI_THEME_GUIDE.md`

この文書と古い記述が矛盾する場合は、この文書の確定事項を優先し、既存文書側は後続作業で同期します。

---

# 1. 現在の基準コミット

Designer配置の確認基準：

```text
3c28e3e WIP: build designer-owned MainForm layout
```

グループ追加ボタンの表示Tag補正：

```text
cf8f50b Fix MainForm display mode tag contract
```

`3c28e3e` 時点では、グループ追加の文字ボタンとアイコンボタンのTagが逆になっていました。

正式契約：

```text
addGroupButton
Tag = view:Beginner

addGroupIconButton
Tag = view:Compact
```

現在は通常の `.cs` 側で実行時補正しています。CodexがDesigner内部名を整理するときは、Designer側のTagも正式契約へ合わせ、補正コードが不要になったか確認します。

---

# 2. DesignerとCodexの責任分担

## 2.1 少佐が後からDesignerで触れる状態を維持する

画面に見える主要部品は、原則としてVisual Studio Designerで編集できるコントロールとして保持します。

対象：

- MainFormのPanel、SplitContainer、Button、Label、MenuStrip、StatusStrip
- 初心者向け説明領域
- 種類アイコン説明領域
- 操作ログ領域
- 操作ログの開閉ボタン
- 将来追加するHelpFormや設定画面

避けること：

- MainFormの主要部品を実行時コードだけで大量生成する
- Designer管理の親子関係を無断で組み替える
- テーマや機能処理を `.Designer.cs` へ直接書く
- CodexがDesignerレイアウトを全面再生成する

## 2.2 Codexが調整できる範囲

少佐の画面確認を前提に、次はCodexが調整できます。

- 内部 `Name` の整理
- 正確なSize、Margin、Padding
- DPI・文字サイズ別の最小値
- SplitContainerの初期位置、最小サイズ、保存・復元
- イベント接続
- 表示状態の同期
- ToolTip、AccessibleName、AccessibleDescription
- テーマ適用

ただし、内部名を変更した場合は、この文書または `UI_CONTROL_CONTRACT.md` に次を残します。

- 旧Name
- 新Name
- 画面上の表示
- 元々その部品を置いた目的
- 接続する機能
- Beginner／Compact／共通の分類

---

# 3. 現在のDesigner部品の意図と内部名整理

Designerが自動生成した日本語名や `label2`、`comboBox1` は、配置意図を維持したまま英語の契約名へ整理してよいものとします。

## 3.1 並び替え領域

| 現在のName | 推奨Name | 元々の設置意図 |
|---|---|---|
| `並び方パネル` | `beginnerOrderPanel` | Beginner表示で、並び方選択、上下移動、保存、保存前へ戻す、未保存表示を文字つきで提供する |
| `並び方アイコンパネル` | `compactOrderPanel` | Compact表示で、同じ並び替え機能をアイコン中心で提供する |
| `並び方ラベル` | `beginnerSortModeLabel` | Beginner表示の「並び方」見出し |
| `label2` | `compactSortModeLabel` | Compact表示の「並び方」見出し |
| `並び順コンボボックス` | `beginnerSortModeComboBox` | Beginner表示で並び方を選択する |
| `comboBox1` | `compactSortModeComboBox` | Compact表示で並び方を選択する |
| `orderUnsavedStatusLabel_beginner` | `beginnerOrderUnsavedStatusLabel` | Beginner表示で、手動順に未保存変更があることを伝える |
| `orderUnsavedStatusLabel_compact` | `compactOrderUnsavedStatusLabel` | Compact表示で、同じ未保存状態を伝える |

`beginnerOrderPanel` と `compactOrderPanel` は同じPresenter／状態モデルへ接続し、表示部品だけを分けます。

## 3.2 MenuStrip

日本語の `Text` は維持し、内部Nameだけ役割に合わせて整理します。

例：

| 現在のName | 推奨Name | 元々の設置意図 |
|---|---|---|
| `ファイルToolStripMenuItem` | `fileToolStripMenuItem` | 読み込み、書き出し等のファイル系操作をまとめる |
| `登録ToolStripMenuItem` | `registerToolStripMenuItem` | グループ、ファイル、フォルダー、URL、テンプレートの登録入口 |
| `編集ToolStripMenuItem` | `editToolStripMenuItem` | 選択項目の操作と並び替え操作をまとめる |
| `編集ToolStripMenuItem1` | `editSelectedToolStripMenuItem` | 選択中の登録内容を編集する |
| `表示ToolStripMenuItem` | `viewToolStripMenuItem` | 表示モード、ヒント、種類アイコン説明、操作ログ表示を切り替える |
| `設定ToolStripMenuItem` | `toolsToolStripMenuItem` | 画面表示は「ツール」。設定、ごみ箱、バックアップ、修復をまとめる |
| `ヘルプToolStripMenuItem` | `helpToolStripMenuItem` | 使い方、操作アイコン、種類アイコン、バージョン情報を開く |
| `uRL追加ToolStripMenuItem` | `addUrlToolStripMenuItem` | URL登録を開始する |

内部名整理だけを理由に、表示順、表示文言、親メニュー、機能分類を勝手に変更しません。

---

# 4. 下部情報領域

従来の「状態、ヒント、操作結果、アイコン説明、ホバー説明を全部まとめる」設計は採用しません。

下部は次の3種類へ分離します。

1. 初心者向け説明
2. 種類アイコンの見方
3. 操作ログ

Compact操作アイコンの常設説明欄は置きません。

---

# 5. 初心者向け説明

## 5.1 表示条件

- 初回セットアップでBeginner表示を選んだ利用者には、初期状態で表示する
- 利用者がすぐ非表示にできるボタンを説明領域内に置く
- 非表示後はMenuStripの「表示」から再表示できる
- 表示状態を設定へ保存する
- Compact表示でも、利用者がMenuStripから明示的にONにした場合は表示可能とする

## 5.2 推奨コントロール

```text
beginnerHintsPanel
├─ beginnerHintsTitleLabel
├─ beginnerHintsTextLabel
└─ hideBeginnerHintsButton
```

MenuStrip：

```text
showBeginnerHintsToolStripMenuItem
Text = 初心者向け説明
CheckOnClick = true
```

設定：

```text
ShowBeginnerHints
```

## 5.3 内容

初心者向け説明は、現在の画面で次に行える操作を短く説明する場所です。

例：

- 左側でグループを選ぶ
- 右側の追加ボタンから登録する
- 検索フォームは必要な時だけ開く
- 詳しい使い方はヘルプを開く

長い操作説明はMainFormへ詰め込まず、HelpFormへ置きます。

---

# 6. 種類アイコンの見方

## 6.1 操作アイコンとは分ける

種類アイコン：

- フォルダー
- 一般ファイル
- 画像
- 動画
- URL
- テンプレート

操作アイコン：

- 追加
- 開く
- コピー
- 編集
- 詳細
- ごみ箱
- 上下移動
- 並び順保存
- 保存前へ戻す

この2種類を「アイコンの意味」として混在させません。

## 6.2 MainFormへ表示するもの

MainForm下部へ表示できるのは、初心者向けの「種類アイコンの見方」だけです。

```text
typeIconLegendPanel
├─ typeIconLegendTitleLabel
├─ typeIconLegendFlowPanel
└─ hideTypeIconLegendButton
```

表示条件：

- Beginnerを初回選択した場合は初期表示
- 領域内のボタンですぐ非表示にできる
- MenuStripの「表示」から再表示できる
- 表示状態を設定へ保存する

MenuStrip：

```text
showTypeIconLegendToolStripMenuItem
Text = 種類アイコンの見方
CheckOnClick = true
```

設定：

```text
ShowTypeIconLegend
```

## 6.3 Compact操作アイコン

Compact操作アイコンは、MainForm下部へ一覧説明を常設しません。

必須：

- 全アイコンボタンにToolTip
- AccessibleName
- 必要に応じてAccessibleDescription

詳しい一覧はHelpForm内へ置きます。

---

# 7. ToolTipとホバー説明

## 7.1 採用

`ToolTip`を採用します。

- アイコンへマウスを置くと対象の近くへ表示
- MainFormの常設領域を消費しない
- Compact操作アイコンでは常に設定する
- 種類アイコンにも種類名のToolTipを設定できる

推奨値：

```text
InitialDelay = 400～500ms
AutoPopDelay = 8000～12000ms
ShowAlways = true
```

## 7.2 不採用

固定したLabelを書き換えるホバー説明欄は採用しません。

```text
actionHoverDescriptionLabel
```

新規実装では使用しません。

---

# 8. 操作ログ

## 8.1 画面構造

操作ログは別ウィンドウではなく、MainForm中央の項目一覧の下へ置きます。

推奨構造：

```text
itemAreaPanel
├─ searchAndViewPanel
├─ beginnerOrderPanel / compactOrderPanel
└─ contentAndLogSplitContainer
   ├─ Panel1
   │  └─ itemGridView
   └─ Panel2
      └─ operationLogPanel
         ├─ operationLogHeaderPanel
         │  ├─ operationLogTitleLabel
         │  ├─ saveOperationLogButton
         │  ├─ clearOperationLogButton
         │  └─ collapseOperationLogButton
         └─ operationLogView
```

`contentAndLogSplitContainer`：

```text
Orientation = Horizontal
Dock = Fill
IsSplitterFixed = false
```

## 8.2 開閉

- 初期状態は閉じる
- 項目一覧の下端に細い操作ログ見出しを残す
- `▲`相当のボタンで開く
- 開いた状態では`▼`相当のボタンで閉じる
- 表示中は境界を上下へドラッグして高さを調整できる
- 最後に開いていた高さを保存する
- MainForm全体が小さすぎる場合は、項目一覧の最小高を優先する

推奨Name：

```text
toggleOperationLogButton
```

表示文字はテーマや視認性に応じて `▲`／`▼` または「ログを開く／閉じる」を使えます。

## 8.3 表示内容

利用者向け操作ログは、その起動中の操作履歴です。

例：

| 時刻 | 種別 | 操作 | 対象 | 結果 |
|---|---|---|---|---|
| 14:22:10 | 情報 | ファイル追加 | sample.pdf | 成功 |
| 14:22:34 | 情報 | コピー | 定型文A | 成功 |
| 14:23:02 | 警告 | 登録をごみ箱へ | 画像素材 | 成功 |

原則として記録しないもの：

- テンプレート本文
- コピーした本文
- ファイル内容
- 認証情報
- 秘密情報
- 必要以上の完全パス

## 8.4 保存方針

初期値：

```text
操作ログ自動保存 = OFF
```

通常時：

- 起動中だけメモリへ保持
- アプリ終了で消える
- 「操作ログを保存」ボタンで利用者が任意の場所へ保存できる

設定で自動保存をONにした場合：

- 日付別または起動単位で保存する
- 保存先、保持日数、削除方法を設定画面で確認できるようにする
- 初期状態で無断保存しない

推奨設定：

```text
AutoSaveOperationLog = false
OperationLogFolder
OperationLogRetentionDays
```

## 8.5 操作ログ保存ボタン

```text
saveOperationLogButton
```

手動保存形式の初期候補：

- UTF-8テキスト
- CSV

利用者が自分で内容を確認できる形式とします。

---

# 9. 内部エラー診断ログ

利用者向け操作ログと、開発者向け診断ログを分離します。

## 9.1 保存条件

内部例外、起動失敗、保存失敗、データ破損疑い等が発生した場合は、診断情報をローカルへ保存します。

- 通常操作ログの自動保存設定がOFFでも保存する
- アプリが外部へ自動送信しない
- 利用者へ、問題報告時に診断ファイルを添付してほしいことを案内する
- 保存場所を開くボタンを用意する
- 利用者が削除できる

## 9.2 利用者から内部を見えにくくする方法

単なるパスワード付きZIPやアプリ内共通鍵は、解析されれば読めるため、正式な秘匿手段として扱いません。

推奨は公開鍵方式です。

```text
アプリ側：暗号化用の公開鍵だけを保持
開発者側：復号用の秘密鍵を別管理
```

診断パッケージ作成時：

1. ランダムな共通鍵を生成
2. 診断内容をAES-GCM等で暗号化
3. 共通鍵を開発者の公開鍵で暗号化
4. 暗号化データ、暗号化済み共通鍵、形式バージョンを1ファイルへ格納

これにより、アプリ利用者や第三者は通常の方法では内容を読めず、開発者だけが秘密鍵で復号できます。

重要：

- 秘密鍵をアプリ、GitHub、配布ZIPへ含めない
- 復号ツールと秘密鍵は開発者環境だけで管理する
- 暗号化前に、不要な個人情報や本文を収集しない
- 暗号化は過剰収集を正当化しない
- 診断ファイルを送るかどうかは利用者が決める

## 9.3 診断ログへ含める候補

- アプリバージョン
- Windowsバージョン
- .NETランタイム
- 発生日時
- 例外型
- スタックトレース
- 直前の内部処理名
- 設定のうち問題再現に必要な非機密項目
- データ形式バージョン
- 破損箇所を特定するためのID

原則として含めないもの：

- テンプレート本文
- クリップボード内容
- ファイル本体
- URLのクエリや認証情報
- APIキー、トークン、パスワード
- 利用者が明示許可していない登録内容の全文

## 9.4 利用者向け表示

例：

```text
内部エラーが発生したため、診断ファイルを保存しました。
問題を報告する際は、このファイルを添付してください。

[保存場所を開く] [ファイル名をコピー] [閉じる]
```

診断ファイルの表示名は、利用者へ威圧感を与えない日本語名にします。

---

# 10. DataGridViewの利用者調整

`itemGridView`は次を利用者が直接調整できるようにします。

- 列見出し境界のドラッグで列幅変更
- 行境界のドラッグで行高変更
- 列境界のダブルクリックで内容に合わせる
- 表示メニューまたは右クリックから初期値へ戻す

実装条件：

```text
AllowUserToResizeColumns = true
AllowUserToResizeRows = true
```

列幅：

- 自動保存
- 再起動後に復元
- DPIを考慮した論理サイズで保存

行高：

- 既定行高を保存
- 個別変更された行だけ保存
- サムネイル表示時は最小行高を設定

常時AutoSizeを有効にして、利用者の手動指定を上書きしません。

---

# 11. テーマ実装順

## 現在実装するもの

```text
Cat
```

- 現在作成済みの猫アイコンを使用
- まず主要機能が正しく動くことを優先
- Designer内の配置を猫テーマで確認する

## 後から実装するもの

```text
Refined
```

- 洗練された配色と専用アイコンを後から制作
- 機能コードや画面配置を作り直さず差し替えられる構造にする

アイコンは機能キーで参照します。

```text
ActionIconKey.AddFile
ActionIconKey.Edit
ActionIconKey.MoveUp
```

素材例：

```text
Assets/Icons/Cat/
Assets/Icons/Refined/
```

Refined素材が未完成でも、テーマ識別とアイコンカタログの構造だけは初期実装へ含めて構いません。

---

# 12. 調整用ファイル

少佐が後からDesigner以外でも調整できるよう、次へ集約します。

```text
UiTexts.cs
UiLayoutSettings.cs
UiThemeSettings.cs
ActionIconCatalog.cs
```

## `UiTexts.cs`

- 表示文言
- ToolTip
- ステータス文言
- 空状態
- 操作ログの操作名
- エラー案内

## `UiLayoutSettings.cs`

- Margin、Padding
- ボタンサイズ
- アイコン論理サイズ
- 右側操作パネル幅
- DataGridView既定列幅・行高
- 操作ログ初期高・最小高
- Splitter最小値
- MinimumSize

## `UiThemeSettings.cs`

- 背景色
- 文字色
- フォント
- 枠線
- 選択色
- 警告色
- ボタン状態別外観

## `ActionIconCatalog.cs`

- 機能キーとテーマ別画像の対応
- DPI・表示サイズ別の画像生成とキャッシュ

---

# 13. 機能実装時の停止条件

Codexは次の場合に無制限に設計を変更せず、作業を止めて報告します。

- Designerの親子関係を大きく変更する必要がある
- MainFormを別方式で全面再構築する必要がある
- 操作ログと一覧を上下分割すると既存機能が失われる
- 内部名整理で機能対応を判別できない部品がある
- 既存MainFormとMainFormDesignDraftの統合方針に重大な矛盾がある
- 診断ログの暗号鍵管理方法が確定しないまま配布実装へ進む必要がある

---

# 14. 受け入れ条件

- `addGroupButton`がBeginner、`addGroupIconButton`がCompactで表示される
- Beginner／Compactの切替で、同じ機能が同じPresenter処理へ接続される
- 内部名変更前後の対応と設置意図が文書化される
- 初心者向け説明と種類アイコン説明を個別に非表示・再表示できる
- Compact操作アイコンはToolTipで説明される
- 操作ログは初期非表示で、下端ボタンから開閉できる
- 操作ログ表示中は境界をドラッグして高さを変えられる
- 通常操作ログは初期状態で自動保存されない
- 手動保存ボタンがある
- 自動保存は利用者が設定で明示的にONにした場合だけ有効
- 内部エラー時は診断ログがローカル保存され、外部へ自動送信されない
- 診断ログは公開鍵方式を前提とし、秘密鍵を配布物へ含めない
- DataGridViewの列幅・行高を利用者が調整できる
- 列幅等がDPIを考慮して保存・復元される
- MainFormの主要部品を後からDesignerで編集できる
- 猫テーマで機能を完成させた後、洗練テーマを機能変更なしで追加できる
