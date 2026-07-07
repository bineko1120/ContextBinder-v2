# UI編集ガイド

ContextBinder v2 は C# WinForms アプリです。旧 v1 の VB.NET WinForms と同じく Visual Studio で開けますが、現在の画面はコードで組み立てている部分が多いため、まずは下記の編集用ファイルから調整してください。

## 文言を直す場所

文言だけを直す場合は、次のファイルを編集してください。

- `src/ContextBinder/UiTexts.cs`

主に次の文言をまとめています。

- 初回セットアップ画面のステップ名、見出し、説明
- 保存場所の説明
- 使いやすさ設定の説明
- 確認画面の見出しと項目名
- MainForm 下部の「使い方のヒント」
- MainForm 下部の「アイコンの意味」
- 主要ボタン名と ToolTip
- アイコン説明文

## 軽微なレイアウトを直す場所

余白、幅、高さなどを少し調整したい場合は、次のファイルを編集してください。

- `src/ContextBinder/UiLayoutSettings.cs`

ここでは、初回セットアップ画面や MainForm 下部説明エリアのサイズをまとめています。大きく変更すると文字切れや表示崩れにつながるため、少しずつ変更して `dotnet build` と起動確認を行ってください。

## Visual Studio Designerで触れる範囲

現在の `MainForm` と `FirstRunSetupForm` は `partial class` ですが、画面部品の多くはコードで動的に作っています。そのため、WinForms Designerで完全に編集する構成ではありません。

安全に調整しやすい順番は次の通りです。

1. `UiTexts.cs` で文言を直す
2. `UiLayoutSettings.cs` で余白、幅、高さを少し直す
3. 必要な場合だけ `MainForm.cs` / `FirstRunSetupForm.cs` のレイアウト生成メソッドをCodexに修正してもらう

保存処理や登録処理を持つ `Services` や `Models` は、文言や見た目の調整では触らないでください。

## 今後の実装ルール

Codexに機能追加や修正を頼むときは、次のルールを守るよう伝えてください。

- `UiTexts.cs` の既存文言は、機能追加で必要な場合以外は勝手に書き換えない
- `UiLayoutSettings.cs` の余白、幅、高さは、表示崩れ修正以外で勝手に変えない
- MainForm 下部の「使い方のヒント」「アイコンの意味」エリアを機能追加で上書きしない
- ユーザーが調整する領域と、アプリの保存・登録ロジックを混ぜない
- 新機能を追加する場合も、初心者向け説明とアイコンの意味の表示ON/OFF設定を維持する
- `docs/ContextBinder_v2_SPEC.md` は一次仕様書として扱い、文言調整だけで変更しない
