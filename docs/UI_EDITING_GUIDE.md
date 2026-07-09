# UI編集ガイド

ContextBinder v2 は C# WinForms アプリです。旧 v1 の VB.NET WinForms と同じく Visual Studio Designer で画面を編集する方針です。

今後の固定UIは、原則として Designer で見たまま調整できる構成に寄せます。Codex は保存処理、イベント接続、Service 呼び出し、テストを担当し、Designerで作った見た目を勝手に作り直さない方針です。

## 重要方針

固定UIの見た目は、原則 Visual Studio Designer で編集します。

Designerで触る主なもの。

- `Text`
- `Size`
- `Location`
- `Margin`
- `Padding`
- `Dock`
- `Anchor`
- `MenuStrip`
- `ToolStrip`
- `GroupBox`
- `Panel`
- `Button`
- `Label`
- `CheckBox`
- `ComboBox`
- `DataGridView` の表示列

固定のボタン幅、フォーム上の固定余白、見出し位置、メニュー構成などは Designer で触る対象です。

## 文言を直す場所

Designerで見えてほしい固定文言は、Designerの `Text` プロパティを優先します。

次のような実行時文言は、次のファイルへ集約します。

- `src/ContextBinder/UiTexts.cs`

`UiTexts.cs` の用途。

- ToolTip
- エラー文
- 確認メッセージ
- 動的サマリー
- ステータス表示
- 実行時に変わる文言
- 設定値や種類の表示名マッピング

## UiLayoutSettings.cs の扱い

`UiLayoutSettings.cs` は、ユーザーが主に触る場所ではありません。

次のような、Designerで直接置けない実行時レイアウト補助に限定します。

- `src/ContextBinder/UiLayoutSettings.cs`

`UiLayoutSettings.cs` の用途。

- 動的生成されるUIの既定値
- サムネイルの既定サイズ
- 実行時に増減するパネルの最小サイズ
- アイコン説明を動的生成する場合の間隔
- Designerで直接置けない実行時レイアウト補助

固定のボタン幅、固定余白、フォーム上の見出し位置などは、できるだけDesignerで調整してください。

## Visual Studio Designerで触れる範囲

現在の `MainForm` と `FirstRunSetupForm` は `partial class` ですが、PR #4時点の土台では画面部品の多くがコードで動的に作られています。
今後、ユーザーが作る固定UIはDesigner管理へ移し、Codexは `docs/UI_CONTROL_CONTRACT.md` の `Name` に従って接続します。

安全に調整しやすい順番は次の通りです。

1. Designerで固定UIの見た目、文言、余白、サイズ、配置を直す
2. `docs/UI_CONTROL_CONTRACT.md` の `Name` と一致しているか確認する
3. ToolTip、エラー文、動的サマリーだけ `UiTexts.cs` を直す
4. 動的UIの既定サイズだけ、必要に応じて `UiLayoutSettings.cs` を直す
5. イベント接続や保存処理はCodexに依頼する

保存処理や登録処理を持つ `Services` や `Models` は、文言や見た目の調整では触らないでください。

## 今後の実装ルール

Codexに機能追加や修正を頼むときは、次のルールを守るよう伝えてください。

- `UiTexts.cs` の既存文言は、機能追加で必要な場合以外は勝手に書き換えない
- 固定UIの `Text`、`Size`、`Location`、`Margin`、`Padding`、`Dock`、`Anchor` を必要なく上書きしない
- `UiLayoutSettings.cs` は動的UI補助に限定し、固定UI調整の主な場所として扱わない
- MainForm 下部の「使い方のヒント」「アイコンの意味」エリアを機能追加で上書きしない
- MenuStrip、ToolStrip、GroupBox、Panel、Button、DataGridView列は、Designerで置かれたものを優先する
- ユーザーが調整する領域と、アプリの保存・登録ロジックを混ぜない
- 新機能を追加する場合も、初心者向け説明とアイコンの意味の表示ON/OFF設定を維持する
- `docs/ContextBinder_v2_SPEC.md` は一次仕様書として扱い、文言調整だけで変更しない
