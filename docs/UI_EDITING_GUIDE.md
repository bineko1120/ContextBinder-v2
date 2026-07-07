# UI編集ガイド

ContextBinder v2 は C# WinForms アプリです。VB.NETではありません。Visual Studio の WinForms Designer で、C#のまま画面を編集します。

## Designerで開く方法

1. Visual Studioで `ContextBinder_v2.sln` を開く
2. ソリューションエクスプローラーで `src/ContextBinder/Forms/` を開く
3. `MainForm.cs`、`FirstRunSetupForm.cs`、または各 `UserControl.cs` を右クリックする
4. 「デザイナーの表示」を選ぶ
5. または対象ファイルを開いた状態で `Shift + F7` を押す

## Designerで触ってよいもの

- `Text`
- `Size`
- `Margin`
- `Padding`
- `Dock`
- `Anchor`
- GroupBoxやPanelの配置
- 説明文ラベル
- ボタンの表示名
- TableLayoutPanel / FlowLayoutPanel の余白や行/列比率

ToolTip文言は `src/ContextBinder/UiTexts.cs` にまとまっているため、Designerで直接触るより `UiTexts.cs` を編集する方が安全です。

## Designerで触らない方がよいもの

- `Name`
- イベントハンドラ
- DataGridViewのバインド処理
- 保存処理
- `Services`
- `Models`
- `Program.cs`
- `StoreService`
- `StorageLocationService`
- JSONの構造
- アイコン読み込み処理

これらを変えると、保存/読み込み、初回セットアップ、タスクトレイ、設定反映が壊れる可能性があります。

## Designerで触れる範囲

`MainForm` は、次の範囲をDesignerで調整しやすい構成にしています。

- 右側ボタン群
- 下部の「使い方のヒント」
- 下部の「アイコンの意味」
- グループ一覧、中央一覧、右側ボタン列の大まかな配置

`FirstRunSetupForm` は、外枠と各ステップをDesignerで見やすい構成にしています。

- 上部ステップ表示
- 中央のステップ表示エリア
- 下部の「戻る」「次へ」「キャンセル」ボタン列
- Step 1 `StartModeStepControl`
- Step 2 `StorageLocationStepControl`
- Step 3 `UsabilitySettingsStepControl`
- Step 4 `ConfirmStartStepControl`

Step 4の確認内容、MainFormのアイコン画像、登録項目の一覧行など、実行時データで変わる部分はコードで更新します。

## UiTexts.cs と Designer Text の使い分け

方針は次の通りです。

- Designerで見えてほしい固定文言は、Designerの `Text` プロパティを優先する
- ToolTip、エラー文、確認メッセージ、動的サマリーは `UiTexts.cs` を優先する
- 設定値の表示名マッピングは `UiTexts.cs` に置く
- Codexは実行時にDesignerの `Text` を必要以上に上書きしない

現在は移行途中のため、一部の固定文言も `UiTexts.cs` からDesigner管理コントロールへ初期設定しています。文言だけを変えたい場合は、まず `UiTexts.cs` を確認してください。

## UiLayoutSettings.cs の使い方

`src/ContextBinder/UiLayoutSettings.cs` は、実行時に計算が必要な高さ/幅、動的エリアの最小高さ、アイコンサイズなどをまとめる場所です。

固定の余白や固定サイズは、可能な範囲でDesigner側の `Margin` / `Padding` / `Size` を調整してください。

## Codexに依頼するときの注意文

CodexにUI追加や修正を頼むときは、次の文を添えると安全です。

```text
既存のDesigner管理UIを壊さないでください。
Designerで調整した Text / Size / Margin / Padding / Dock / Anchor を、必要なく上書きしないでください。
固定UIは可能な限り .Designer.cs / UserControl で管理してください。
処理ロジックは .cs 本体に分離してください。
```

## 今後の実装ルール

- `UiTexts.cs` の文言は、機能追加で必要な場合以外は勝手に書き換えない
- `UiLayoutSettings.cs` の余白、高さ、幅は、既存UI崩れ修正以外で勝手に変えない
- MainForm下部の説明/アイコンの意味エリアは、機能追加で上書きしない
- ユーザーが調整する領域と、アプリロジック領域を混ぜない
- 新機能を追加する場合も、既存の説明表示ON/OFF設定を維持する
