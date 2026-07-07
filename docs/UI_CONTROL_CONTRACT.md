# UI接続契約

この文書は、ユーザーが Visual Studio Designer で画面を作り直すときに、Codex が機能コードを安全に接続するための契約です。

ユーザーは見た目、文言、余白、サイズ、配置をDesignerで調整します。
Codexはここに定義された `Name` を使って、イベント、保存処理、サービス呼び出し、テストを接続します。

## 基本ルール

- `Text`、`Size`、`Margin`、`Padding`、`Dock`、`Anchor` はDesignerで調整してよい。
- `Name` はCodexが接続に使うため、原則変更しない。
- `Name` を変えたい場合は、この文書とコード接続を同時に更新する。
- CodexはDesigner管理UIを勝手に作り直さない。
- 未実装ボタンは配置してよい。実装前は `Enabled = false`、またはクリック時に「今後実装予定」と表示する。
- 機能追加時は、既存のDesigner配置を維持し、必要なイベント接続だけ追加する。

## MainForm コントロール契約

今後MainFormをDesigner-firstで作り直す場合は、以下の `Name` を使ってください。
現行コードには `_groupListBox` のようなprivateフィールド名が残っていますが、今後の安定名はこの表を優先します。

| Name | 種類の目安 | 用途 | 必須 | 未実装でも置いてよいか | Name変更 | Codexが接続するイベント |
|---|---|---|---|---|---|---|
| `groupListBox` | `ListBox` | グループ一覧を表示し、選択中グループを決める | 必須 | いいえ | 不可 | `SelectedIndexChanged` |
| `itemGridView` | `DataGridView` | 選択グループ内の項目一覧を表示する | 必須 | いいえ | 不可 | `CellDoubleClick`, `MouseDown`, `DragEnter`, `DragDrop`, `SelectionChanged` |
| `addGroupButton` | `Button` | グループを追加する | 必須 | いいえ | 不可 | `Click` |
| `addFileButton` | `Button` | ファイル参照を追加する | 必須 | いいえ | 不可 | `Click` |
| `addFolderButton` | `Button` | フォルダー参照を追加する | 必須 | いいえ | 不可 | `Click` |
| `addUrlButton` | `Button` | URL項目を追加する | 必須 | いいえ | 不可 | `Click` |
| `addTemplateButton` | `Button` | テンプレート文を追加する | 必須 | いいえ | 不可 | `Click` |
| `openButton` | `Button` | 選択項目を開く。テンプレートは本文をコピーする | 必須 | いいえ | 不可 | `Click` |
| `copyButton` | `Button` | 選択項目のパス、URL、本文をコピーする | 必須 | いいえ | 不可 | `Click` |
| `editButton` | `Button` | 選択項目を編集する | 必須 | いいえ | 不可 | `Click` |
| `detailButton` | `Button` | 選択項目の詳細を表示する | 任意 | はい | 不可 | `Click` |
| `deleteButton` | `Button` | 選択項目を削除、またはアプリ内ごみ箱へ移動する | 必須 | いいえ | 不可 | `Click` |
| `statusLabel` | `Label` | 保存、読み込み、操作結果を表示する | 必須 | いいえ | 不可 | 直接イベントなし。CodexがTextを更新 |
| `settingsButton` | `Button` | 設定画面を開く | 任意 | はい | 不可 | `Click` |
| `searchTextBox` | `TextBox` | 検索キーワードを入力する | 任意 | はい | 不可 | `TextChanged`, `KeyDown` |
| `searchButton` | `Button` | 検索を実行する | 任意 | はい | 不可 | `Click` |
| `searchScopeComboBox` | `ComboBox` | 検索範囲を「現在のグループ / 全体」から選ぶ | 任意 | はい | 不可 | `SelectedIndexChanged` |
| `typeFilterComboBox` | `ComboBox` | 種類で絞り込む | 任意 | はい | 不可 | `SelectedIndexChanged` |
| `clearSearchButton` | `Button` | 検索条件をクリアする | 任意 | はい | 不可 | `Click` |
| `trashButton` | `Button` | アプリ内ごみ箱を開く | 任意 | はい | 不可 | `Click` |
| `importButton` | `Button` | 登録内容を読み込む | 任意 | はい | 不可 | `Click` |
| `exportButton` | `Button` | 登録内容を書き出す | 任意 | はい | 不可 | `Click` |
| `showBeginnerHintsCheckBox` | `CheckBox` | 使い方のヒントの表示/非表示を切り替える | 必須 | いいえ | 不可 | `CheckedChanged` |
| `showIconMeaningCheckBox` | `CheckBox` | アイコンの意味の表示/非表示を切り替える | 必須 | いいえ | 不可 | `CheckedChanged` |
| `beginnerHintsGroupBox` | `GroupBox` | 使い方のヒントを表示する領域 | 必須 | いいえ | 不可 | 直接イベントなし。Visibleを更新 |
| `iconMeaningGroupBox` | `GroupBox` | アイコンの意味を表示する領域 | 必須 | いいえ | 不可 | 直接イベントなし。Visibleを更新 |
| `iconMeaningPanel` | `FlowLayoutPanel` または `Panel` | 種類別の猫アイコン説明を配置する領域 | 必須 | いいえ | 不可 | 直接イベントなし。Codexがアイコン説明を追加 |

## DataGridView 列の推奨Name

`itemGridView` の列はDesignerで作っても、Codex側で作っても構いません。
Designerで作る場合は、以下のNameを推奨します。

| Name | 用途 |
|---|---|
| `itemIconColumn` | 種類アイコン |
| `itemTypeColumn` | 種類文字 |
| `itemTitleColumn` | タイトル |
| `itemReferenceColumn` | 参照先、URL、テンプレートプレビュー |
| `itemStatusColumn` | 未確認などの状態 |

## ContextMenuStrip 契約

MainFormの項目右クリックメニューをDesignerで置く場合は、以下のNameを推奨します。

| Name | 種類 | 用途 | 実装状況 | Codexが接続するイベント |
|---|---|---|---|---|
| `itemContextMenuStrip` | `ContextMenuStrip` | 項目右クリックメニュー | 基本メニューは実装済み | `Opening` |
| `openToolStripMenuItem` | `ToolStripMenuItem` | 開く | 実装済み | `Click` |
| `copyToolStripMenuItem` | `ToolStripMenuItem` | コピー | 実装済み | `Click` |
| `editToolStripMenuItem` | `ToolStripMenuItem` | 編集 | 実装済み | `Click` |
| `detailToolStripMenuItem` | `ToolStripMenuItem` | 詳細 | 簡易実装 | `Click` |
| `deleteToolStripMenuItem` | `ToolStripMenuItem` | 削除 | 土台あり | `Click` |
| `openContainingFolderToolStripMenuItem` | `ToolStripMenuItem` | 参照先のフォルダーを開く | 未実装 | `Click` |
| `copyTitleToolStripMenuItem` | `ToolStripMenuItem` | タイトルをコピー | 未実装 | `Click` |

## SettingsForm 候補コントロール

設定画面は未実装ですが、準MVPで優先して作る対象です。
Designerで先に配置する場合は、以下のNameを使ってください。

### 常駐・起動

| Name | 種類の目安 | 用途 | 必須 | 未実装でも置いてよいか | Name変更 | Codexが接続するイベント |
|---|---|---|---|---|---|---|
| `autoStartWithWindowsCheckBox` | `CheckBox` | Windows起動時にContextBinderを自動起動するか | SettingsFormでは必須候補 | はい | 不可 | `CheckedChanged` または `Apply`時 |
| `startMinimizedCheckBox` | `CheckBox` | 自動起動時にMainFormを出さずタスクトレイ格納で起動するか | SettingsFormでは必須候補 | はい | 不可 | `CheckedChanged` または `Apply`時 |

### スタートメニュー

| Name | 種類の目安 | 用途 | 必須 | 未実装でも置いてよいか | Name変更 | Codexが接続するイベント |
|---|---|---|---|---|---|---|
| `registerStartMenuButton` | `Button` | スタートメニューにContextBinderのショートカットを作成する | SettingsFormでは必須候補 | はい | 不可 | `Click` |
| `unregisterStartMenuButton` | `Button` | スタートメニューからContextBinderのショートカットを削除する | SettingsFormでは必須候補 | はい | 不可 | `Click` |
| `repairShortcutsButton` | `Button` | アプリ本体の場所が変わった場合にショートカットのリンク先を修復する | SettingsFormでは必須候補 | はい | 不可 | `Click` |

## ショートカット機能の実装契約

スタートメニュー登録とWindows起動時自動起動は、UIから直接ファイル操作せず、Serviceへ分離します。

候補Service:

- `Services/ShortcutService.cs`
- `Services/StartupRegistrationService.cs`

責務:

- ユーザー単位のStart Menu Programs配下へショートカットを作成する。
- ユーザー単位のStart Menu Programs配下からショートカットを削除する。
- ユーザー単位のStartupフォルダへショートカットを作成する。
- ユーザー単位のStartupフォルダからショートカットを削除する。
- ショートカットの存在を確認する。
- ショートカットのリンク先が現在のexeと一致するか確認する。
- 壊れたショートカットを現在のexeパスへ修復する。

注意:

- 管理者権限を要求しない。
- 第一候補はユーザー単位ショートカットとする。
- レジストリRunキーやタスクスケジューラは第一候補にしない。
- ZIP配布ではアプリフォルダが移動される可能性があるため、修復導線を用意する。
- 解除ボタンを用意し、アンインストーラーなしでもユーザーが片付けられるようにする。

## Codexに機能接続を依頼するときのテンプレート

```text
Designerで配置済みのUIを作り直さず、docs/UI_CONTROL_CONTRACT.md のNameに接続してください。
Text / Size / Margin / Padding / Dock / Anchor は必要なく上書きしないでください。
未実装ボタンは、実装するまでDisabledまたは「今後実装予定」表示にしてください。
保存処理やファイル操作はServicesへ分離してください。
```

## ユーザーが先にDesignerで用意するとよいもの

1. MainFormの右側ボタン群と、下部の `beginnerHintsGroupBox` / `iconMeaningGroupBox`。
2. MainForm上部または中央上の検索UI一式。
3. SettingsFormの「常駐・起動」「スタートメニュー」「Windows起動時」カテゴリ。
4. ごみ箱、インポート/エクスポート、バックアップ復元は、ボタンだけ先に置いてDisabledにしておく。
