# ContextBinder v2

ContextBinder v2 は開発中の Windows 常駐型ツールです。
ファイル、フォルダー、画像、動画、URL、テンプレート文をグループごとに登録し、必要なときに開く・コピーするための WinForms アプリとして実装しています。

## 起動方法

Visual Studio で `ContextBinder_v2.sln` を開き、`ContextBinder` プロジェクトを起動してください。

コマンドラインから確認する場合は、次のコマンドを使います。

```powershell
dotnet build .\ContextBinder_v2.sln
dotnet run --project .\src\ContextBinder\ContextBinder.csproj
dotnet run --project .\tests\ContextBinder.Tests\ContextBinder.Tests.csproj
```

## 現在できること

- グループの追加
- グループの選択
- ファイル、フォルダー、URL、テンプレート文の手動追加
- 画像・動画ファイルの種類判定
- 一覧でのタイトル、参照先、種類の表示
- 同一グループ内の参照先重複チェック
- 登録内容と設定のJSON保存、起動時読み込み
- 右クリックメニューとタスクトレイ常駐の骨組み

## 登録内容と設定について

初期状態では、登録内容と設定は Windows のアプリ用フォルダーに保存されます。
登録元のファイル、フォルダー、画像、動画そのものはコピーしません。保存するのは参照先、URL、テンプレート文、グループ、表示設定などです。

将来的に、アプリフォルダー内保存や任意フォルダー保存へ切り替えられる構成にする予定です。

## 開発中の注意

このリポジトリはMVPの土台です。D&D、設定画面、詳細画面、バックアップ、ごみ箱、インポート・エクスポートなどは段階的に実装します。

猫アイコンなどの素材は未同梱です。アイコンが無い状態でもビルドできるよう、現時点ではWindows標準アイコンまたはテキスト表示を使います。
