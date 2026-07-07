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

`MainForm` と `FirstRunSetupForm` は、Visual Studio の WinForms Designer で開きやすいように `.Designer.cs` と `InitializeComponent()` を追加しています。

Designerで比較的触りやすい範囲は次の通りです。

- `MainForm.Designer.cs`
  - 右側ボタン群の並び、余白、幅、高さ
  - 下部の「使い方のヒント」エリアの外枠、余白、高さ
  - 下部の「アイコンの意味」エリアの外枠、余白、高さ
  - グループ一覧、中央一覧、右側ボタン列の大まかな列幅
- `FirstRunSetupForm.Designer.cs`
  - 上部ステップ表示の高さや余白
  - 中央のステップ表示エリアの外枠や余白
  - 下部の「戻る」「次へ」「キャンセル」ボタン列
  - フォーム全体の初期サイズ、最小サイズ

Designerで触らない方がよい範囲は次の通りです。

- `MainForm.cs`
  - 保存、読み込み、追加、削除、右クリック、タスクトレイなどの処理
  - 下部説明の表示ON/OFFを `settings.json` に保存する処理
  - アイコンの意味を実際の猫アイコンから作る処理
- `FirstRunSetupForm.cs`
  - ステップごとの中身を切り替える処理
  - 保存場所選択、使いやすさ設定、確認画面の生成処理
  - 初回セットアップの結果を保存するための値を作る処理
- `Services` / `Models`
  - 登録内容と設定の保存、読み込み、保存場所解決などのアプリ本体ロジック

安全に調整しやすい順番は次の通りです。

1. `UiTexts.cs` で文言を直す
2. `UiLayoutSettings.cs` で余白、幅、高さを少し直す
3. Visual Studio Designerで `.Designer.cs` 側の外枠やボタン配置を微調整する
4. 動的生成部分を変えたい場合だけ、`MainForm.cs` / `FirstRunSetupForm.cs` の変更をCodexに依頼する

Designerで表示されない、または触りにくい部分は、ステップごと・アイコンごとに動的生成している部分です。その場合は `UiTexts.cs` と `UiLayoutSettings.cs` を優先して調整してください。

## 今後の実装ルール

Codexに機能追加や修正を頼むときは、次のルールを守るよう伝えてください。

- `UiTexts.cs` の既存文言は、機能追加で必要な場合以外は勝手に書き換えない
- `UiLayoutSettings.cs` の余白、幅、高さは、表示崩れ修正以外で勝手に変えない
- MainForm 下部の「使い方のヒント」「アイコンの意味」エリアを機能追加で上書きしない
- ユーザーが調整する領域と、アプリの保存・登録ロジックを混ぜない
- 新機能を追加する場合も、初心者向け説明とアイコンの意味の表示ON/OFF設定を維持する
- `docs/ContextBinder_v2_SPEC.md` は一次仕様書として扱い、文言調整だけで変更しない
