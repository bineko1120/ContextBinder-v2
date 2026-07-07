# UIアーキテクチャ

ContextBinder v2 のUI開発は、Visual Studio WinForms Designer-first を基本方針にします。C# WinFormsのまま維持し、VB.NET化、WPF移行、WebView2化は行いません。

## 責務分離

- `Form.cs` / `UserControl.cs`: イベント処理、画面状態の反映、Service呼び出し
- `.Designer.cs`: 固定UI、ボタン、ラベル、GroupBox、Panel、TableLayoutPanel、FlowLayoutPanel
- `Services`: 保存、読み込み、ファイル操作、クリップボード、検索などの処理
- `Models`: JSON保存対象のデータ構造
- `UiTexts.cs`: ToolTip、エラー文、確認メッセージ、動的サマリー、設定値の表示名
- `UiLayoutSettings.cs`: 動的エリアの既定サイズ、アイコンサイズ、計算が必要な高さ/幅

## 新しい画面を追加するとき

1. FormまたはUserControlを追加する
2. 固定UIはDesignerで配置する
3. イベント処理だけ `.cs` 本体に書く
4. 保存処理やデータ処理はServiceへ置く
5. Designerで編集した `Text` / `Size` / `Margin` / `Padding` / `Dock` / `Anchor` を実行時コードで不用意に上書きしない
6. 動的に変える必要がある値だけ、専用メソッドで更新する
7. スモークテストを追加する

## 固定UIと動的UIの分け方

Designerで見える固定UIにするもの：

- フォームの外枠
- ボタン
- 固定ラベル
- GroupBox
- Panel
- TableLayoutPanel
- FlowLayoutPanel
- 固定の説明エリア
- 入力欄やチェックボックス

動的生成を許可するもの：

- 登録項目に応じて変わるDataGridView行
- 検索結果
- グループ一覧
- アイコン画像の読み込み
- 設定値に応じて変わる確認サマリー
- 実行時データに応じた警告表示
- 動的に増減する小さな表示部品

動的生成が必要な場合でも、Designer管理の親Panel / FlowLayoutPanel / TableLayoutPanelへ追加します。

## 未実装画面のルール

以下を実装するときはDesigner-firstを原則にします。

- `SettingsForm`
- `ItemEditForm`
- `TemplateEditForm`
- `ItemDetailForm`
- `CopyMoveDialog`
- `RecycleBinForm`
- `ImportExportDialog`
- `BackupRestoreDialog`
- 検索/絞り込みUI
- 保存場所変更/移行UI

各画面は、固定UIを `.Designer.cs`、処理を `.cs`、保存やデータ操作をService、動的文言を `UiTexts.cs` に分けます。

## CodexがUI追加時に守るルール

- 既存のDesigner管理UIを作り直さない
- Designerでユーザーが調整した文言、余白、サイズ、配置を不用意に上書きしない
- MainForm下部の「使い方のヒント」「アイコンの意味」を機能追加で消さない
- 表示/非表示設定を維持する
- 保存処理やデータ処理をFormイベントへ詰め込まない
- `docs/ContextBinder_v2_SPEC.md` を一次仕様書として扱う

## テスト方針

- Form / UserControl の引数なしコンストラクタでDesigner初期化できることを確認する
- Designer用コンストラクタで保存ファイルや外部リソースを作らないことを確認する
- 通常の実行時コンストラクタで既存機能が動くことを確認する
- ShowBeginnerHints / ShowIconLegend の保存と読み込みを確認する
- 保存場所モードの既存テストを壊さない
