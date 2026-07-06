using ContextBinder.Models;
using ContextBinder.Services;

namespace ContextBinder.Forms;

public sealed class FirstRunSetupForm : Form
{
    private const int ContentWidth = 760;
    private const int LastStepIndex = 3;

    private readonly StorageLocationService _storageLocationService;
    private readonly Label _stepLabel = new();
    private readonly Panel _contentPanel = new();
    private readonly Button _backButton = new();
    private readonly Button _nextButton = new();
    private readonly Button _cancelButton = new();

    private readonly RadioButton _recommendedSetupRadio = new();
    private readonly RadioButton _customSetupRadio = new();
    private readonly RadioButton _standardStorageRadio = new();
    private readonly RadioButton _portableStorageRadio = new();
    private readonly RadioButton _customStorageRadio = new();
    private readonly TextBox _customDirectoryTextBox = new();
    private readonly Button _browseButton = new();
    private readonly Label _storageDescriptionLabel = new();
    private readonly Label _storagePreviewLabel = new();

    private readonly CheckBox _editRecommendedSettingsCheckBox = new();
    private readonly ComboBox _typeDisplayModeComboBox = new();
    private readonly CheckBox _showBeginnerHintsCheckBox = new();
    private readonly CheckBox _showIconLegendCheckBox = new();
    private readonly CheckBox _showOperationStatusCheckBox = new();
    private readonly CheckBox _confirmTitleOnDropAddCheckBox = new();
    private readonly CheckBox _focusExistingItemOnDuplicateCheckBox = new();
    private readonly CheckBox _enableGroupDropModifierShortcutsCheckBox = new();
    private readonly CheckBox _confirmGroupDropCopyMoveCheckBox = new();
    private readonly CheckBox _enableItemDragReorderCheckBox = new();
    private readonly CheckBox _enableExternalFileDropOutCheckBox = new();
    private readonly CheckBox _enableExternalUrlTextDragOutCheckBox = new();
    private readonly CheckBox _enableExternalTemplateTextDragOutCheckBox = new();
    private readonly CheckBox _minimizeToTrayOnCloseCheckBox = new();
    private readonly CheckBox _enableContextMenuDetailsCheckBox = new();
    private readonly CheckBox _autoBackupEnabledCheckBox = new();
    private readonly NumericUpDown _maxBackupCountNumeric = new();
    private readonly CheckBox _confirmBeforeDeleteCheckBox = new();
    private readonly CheckBox _moveDeletedItemsToTrashCheckBox = new();
    private readonly CheckBox _searchTemplateBodyCheckBox = new();

    private readonly List<Control> _editableSettingsControls = [];
    private int _currentStep;

    public FirstRunSetupForm(StorageLocationService? storageLocationService = null)
    {
        _storageLocationService = storageLocationService ?? new StorageLocationService();

        Text = "ContextBinder 初回セットアップ";
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.Sizable;
        MaximizeBox = true;
        MinimizeBox = false;
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(900, 760);
        MinimumSize = new Size(860, 720);
        Font = SystemFonts.MessageBoxFont;

        InitializeSetupControls();
        InitializeStorageControls();
        InitializeSettingsControls();
        BuildLayout();
        RenderCurrentStep();
    }

    public StorageMode SelectedStorageMode { get; private set; } = StorageMode.Standard;

    public string CustomStorageDirectory { get; private set; } = string.Empty;

    public AppSettings SelectedAppSettings { get; private set; } = AppSettingsFactory.CreateRecommended();

    private void InitializeSetupControls()
    {
        _recommendedSetupRadio.Text = "初心者おすすめ設定で始める";
        _recommendedSetupRadio.Checked = true;
        _recommendedSetupRadio.CheckedChanged += (_, _) => RenderCurrentStep();

        _customSetupRadio.Text = "カスタム設定を選ぶ";
        _customSetupRadio.CheckedChanged += (_, _) => RenderCurrentStep();
    }

    private void InitializeStorageControls()
    {
        _standardStorageRadio.Text = "通常の場所に保存（おすすめ）";
        _standardStorageRadio.Checked = true;
        _standardStorageRadio.CheckedChanged += (_, _) => UpdateStoragePreview();

        _portableStorageRadio.Text = "このアプリのフォルダに保存";
        _portableStorageRadio.CheckedChanged += (_, _) => UpdateStoragePreview();

        _customStorageRadio.Text = "自分で選んだ場所に保存";
        _customStorageRadio.CheckedChanged += (_, _) => UpdateStoragePreview();

        _customDirectoryTextBox.Width = 560;
        _customDirectoryTextBox.TextChanged += (_, _) => UpdateStoragePreview();

        _browseButton.Text = "参照...";
        _browseButton.Width = 92;
        _browseButton.Click += BrowseButton_Click;
    }

    private void InitializeSettingsControls()
    {
        _typeDisplayModeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        _typeDisplayModeComboBox.Width = 180;
        _typeDisplayModeComboBox.Items.AddRange([
            "アイコン＋文字",
            "アイコンのみ",
            "文字のみ",
            "非表示（非推奨）"
        ]);

        _maxBackupCountNumeric.Minimum = 1;
        _maxBackupCountNumeric.Maximum = 100;
        _maxBackupCountNumeric.Width = 80;

        LoadSettingsIntoControls(AppSettingsFactory.CreateRecommended());
        _editRecommendedSettingsCheckBox.Text = "おすすめ設定を少し変更する";
        _editRecommendedSettingsCheckBox.CheckedChanged += (_, _) => UpdateSettingsEditability();

        RegisterSettingControl(_typeDisplayModeComboBox);
        RegisterSettingControl(_showBeginnerHintsCheckBox, "初心者向け説明を表示");
        RegisterSettingControl(_showIconLegendCheckBox, "アイコンの意味を表示");
        RegisterSettingControl(_showOperationStatusCheckBox, "操作結果ステータスを表示");
        RegisterSettingControl(_confirmTitleOnDropAddCheckBox, "D&D追加時にタイトルを確認");
        RegisterSettingControl(_focusExistingItemOnDuplicateCheckBox, "重複時に既存項目へジャンプ");
        RegisterSettingControl(_enableGroupDropModifierShortcutsCheckBox, "Ctrl/Shiftドロップショートカット");
        RegisterSettingControl(_confirmGroupDropCopyMoveCheckBox, "通常グループドロップ時に確認");
        RegisterSettingControl(_enableItemDragReorderCheckBox, "項目のD&D並び替え");
        RegisterSettingControl(_enableExternalFileDropOutCheckBox, "ファイル/画像/動画/フォルダを外部D&Dで渡す");
        RegisterSettingControl(_enableExternalUrlTextDragOutCheckBox, "URLを外部D&Dでテキストとして渡す");
        RegisterSettingControl(_enableExternalTemplateTextDragOutCheckBox, "テンプレート本文を外部D&Dで渡す");
        RegisterSettingControl(_minimizeToTrayOnCloseCheckBox, "閉じるボタンでタスクトレイに格納");
        RegisterSettingControl(_enableContextMenuDetailsCheckBox, "右クリック詳細操作を有効にする");
        RegisterSettingControl(_autoBackupEnabledCheckBox, "自動バックアップ");
        RegisterSettingControl(_maxBackupCountNumeric);
        RegisterSettingControl(_confirmBeforeDeleteCheckBox, "削除前に確認");
        RegisterSettingControl(_moveDeletedItemsToTrashCheckBox, "削除時にアプリ内ごみ箱へ移動");
        RegisterSettingControl(_searchTemplateBodyCheckBox, "テンプレート本文も検索対象にする");
    }

    private void RegisterSettingControl(Control control)
    {
        _editableSettingsControls.Add(control);
    }

    private void RegisterSettingControl(CheckBox checkBox, string text)
    {
        checkBox.Text = text;
        checkBox.Width = ContentWidth - 40;
        _editableSettingsControls.Add(checkBox);
    }

    private void BuildLayout()
    {
        TableLayoutPanel root = new()
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(16),
            ColumnCount = 1,
            RowCount = 3
        };
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));

        _stepLabel.Dock = DockStyle.Fill;
        _stepLabel.TextAlign = ContentAlignment.MiddleLeft;
        _stepLabel.Font = new Font(Font, FontStyle.Bold);

        _contentPanel.Dock = DockStyle.Fill;
        _contentPanel.BorderStyle = BorderStyle.FixedSingle;
        _contentPanel.Padding = new Padding(14);

        TableLayoutPanel buttonPanel = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 4
        };
        buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 96));
        buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 96));
        buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 96));

        _backButton.Text = "戻る";
        _backButton.Dock = DockStyle.Fill;
        _backButton.Click += (_, _) => MoveStep(-1);

        _nextButton.Dock = DockStyle.Fill;
        _nextButton.Click += NextButton_Click;

        _cancelButton.Text = "キャンセル";
        _cancelButton.Dock = DockStyle.Fill;
        _cancelButton.DialogResult = DialogResult.Cancel;

        buttonPanel.Controls.Add(_backButton, 1, 0);
        buttonPanel.Controls.Add(_nextButton, 2, 0);
        buttonPanel.Controls.Add(_cancelButton, 3, 0);

        root.Controls.Add(_stepLabel, 0, 0);
        root.Controls.Add(_contentPanel, 0, 1);
        root.Controls.Add(buttonPanel, 0, 2);
        Controls.Add(root);

        AcceptButton = _nextButton;
        CancelButton = _cancelButton;
    }

    private void RenderCurrentStep()
    {
        _contentPanel.Controls.Clear();
        _stepLabel.Text = CreateStepText();
        _backButton.Enabled = _currentStep > 0;
        _nextButton.Text = _currentStep == LastStepIndex ? "開始" : "次へ";

        Control page = _currentStep switch
        {
            0 => BuildStartModePage(),
            1 => BuildStoragePage(),
            2 => BuildSettingsPage(),
            _ => BuildConfirmationPage()
        };

        page.Dock = DockStyle.Fill;
        _contentPanel.Controls.Add(page);
    }

    private string CreateStepText()
    {
        string[] steps = [
            "1. 始め方",
            "2. 保存場所",
            "3. 使いやすさ設定",
            "4. 確認"
        ];

        return string.Join("  →  ", steps.Select((step, index) => index == _currentStep ? $"【{step}】" : step));
    }

    private Control BuildStartModePage()
    {
        FlowLayoutPanel panel = CreateVerticalPanel();
        panel.Controls.Add(CreateHeading("ContextBinderへようこそ"));
        panel.Controls.Add(CreateParagraph("""
このツールは、ファイル・フォルダ・URL・テンプレート文を
グループごとにまとめて、すぐ開く/コピーできるツールです。

まずは保存場所と使いやすさ設定を選びます。
"""));
        panel.Controls.Add(CreateOptionPanel(
            _recommendedSetupRadio,
            """
迷った場合はこちらを選んでください。
見やすさと安全性を優先した設定で始めます。
次の画面で保存場所を選べます。
おすすめ設定の内容は後で確認できます。
""",
            118));
        panel.Controls.Add(CreateOptionPanel(
            _customSetupRadio,
            """
表示、ドラッグ＆ドロップ、削除確認、バックアップなどを自分で選びます。
ある程度使い方を決めたい人向けです。
保存場所も次の画面で選べます。
""",
            100));

        return panel;
    }

    private Control BuildStoragePage()
    {
        FlowLayoutPanel panel = CreateVerticalPanel();
        panel.Controls.Add(CreateHeading("登録内容と設定の保存場所を選んでください"));
        panel.Controls.Add(CreateStorageOptionPanel(
            _standardStorageRadio,
            """
Windowsの標準的なアプリ用フォルダに、登録内容と設定を保存します。
迷った場合はこれを選んでください。
アプリ本体のフォルダを移動しても、登録内容と設定は維持されます。
"""));
        panel.Controls.Add(CreateStorageOptionPanel(
            _portableStorageRadio,
            """
アプリ本体と同じ場所に ContextBinder_Data フォルダを作り、登録内容と設定を保存します。
フォルダごとバックアップ・移動したい人向けです。
Program Files など書き込み権限が厳しい場所では失敗することがあります。
AppDataには保存しません。
"""));
        Panel customPanel = CreateStorageOptionPanel(
            _customStorageRadio,
            """
自分で選んだフォルダに登録内容と設定を保存します。
OneDrive、別ドライブ、外部ドライブなどを使いたい人向けです。
同期中、権限不足、外部ドライブ未接続には注意してください。
AppDataには保存しません。
""",
            150);
        FlowLayoutPanel customPathPanel = new()
        {
            Location = new Point(30, 112),
            Width = ContentWidth - 40,
            Height = 32,
            FlowDirection = FlowDirection.LeftToRight
        };
        customPathPanel.Controls.Add(_customDirectoryTextBox);
        customPathPanel.Controls.Add(_browseButton);
        customPanel.Controls.Add(customPathPanel);
        panel.Controls.Add(customPanel);

        _storageDescriptionLabel.Width = ContentWidth;
        _storageDescriptionLabel.Height = 64;
        _storageDescriptionLabel.Margin = new Padding(0, 8, 0, 0);
        _storagePreviewLabel.Width = ContentWidth;
        _storagePreviewLabel.Height = 48;
        _storagePreviewLabel.BorderStyle = BorderStyle.FixedSingle;
        _storagePreviewLabel.Padding = new Padding(8);
        panel.Controls.Add(_storageDescriptionLabel);
        panel.Controls.Add(_storagePreviewLabel);
        UpdateStoragePreview();
        return panel;
    }

    private Control BuildSettingsPage()
    {
        FlowLayoutPanel panel = CreateVerticalPanel();
        bool customSetup = _customSetupRadio.Checked;

        panel.Controls.Add(CreateHeading(customSetup ? "使いやすさ設定を選んでください" : "おすすめ設定の内容を確認してください"));
        panel.Controls.Add(CreateParagraph(customSetup
            ? "画面の表示、ドラッグ＆ドロップ、削除確認などを自分の使い方に合わせて選べます。"
            : "初心者おすすめ設定は、見やすさと安全性を優先した初期値です。内容を確認し、必要なら少しだけ変更できます。"));

        if (!customSetup)
        {
            _editRecommendedSettingsCheckBox.Width = ContentWidth;
            panel.Controls.Add(_editRecommendedSettingsCheckBox);
        }

        panel.Controls.Add(CreateSettingsCategoryPanel(
            "表示",
            "一覧の見え方と、画面下に出す説明を選びます。",
            new Control[]
            {
                CreateComboRow("種類表示", _typeDisplayModeComboBox, "一覧で「種類」をどう表示するか選べます。非表示は分かりづらくなるため非推奨です。"),
                CreateSettingRow(_showBeginnerHintsCheckBox, "画面下に、操作の意味を分かりやすく表示します。"),
                CreateSettingRow(_showIconLegendCheckBox, "猫アイコンが何を表しているか表示します。"),
                CreateSettingRow(_showOperationStatusCheckBox, "読み込みや保存などの結果を画面下に表示します。")
            }));

        panel.Controls.Add(CreateSettingsCategoryPanel(
            "ドラッグ＆ドロップ",
            "ファイルやURLを登録するとき、または他のアプリへ渡すときの動きを選びます。",
            new Control[]
            {
                CreateSettingRow(_confirmTitleOnDropAddCheckBox, "登録時にタイトルを確認したい場合に使います。"),
                CreateSettingRow(_focusExistingItemOnDuplicateCheckBox, "同じ内容がすでにある場合、その項目を見つけやすくします。"),
                CreateSettingRow(_enableGroupDropModifierShortcutsCheckBox, "グループへドラッグしたとき、Ctrl/Shiftキーでコピー・移動を切り替えられます。"),
                CreateSettingRow(_confirmGroupDropCopyMoveCheckBox, "グループへドロップしたとき、コピーか移動か迷わないよう確認します。"),
                CreateSettingRow(_enableItemDragReorderCheckBox, "一覧の順番を手で入れ替えられます。"),
                CreateSettingRow(_enableExternalFileDropOutCheckBox, "ファイル、画像、動画、フォルダーを他のアプリへドラッグして使いたい人向けです。"),
                CreateSettingRow(_enableExternalUrlTextDragOutCheckBox, "URLを他のアプリへドラッグして使いたい人向けです。"),
                CreateSettingRow(_enableExternalTemplateTextDragOutCheckBox, "テンプレート文を他のアプリへドラッグして使いたい人向けです。")
            }));

        panel.Controls.Add(CreateSettingsCategoryPanel(
            "閉じるときの動作",
            "アプリを閉じたとき、完全終了するか右下にしまうかを選びます。",
            new Control[]
            {
                CreateSettingRow(_minimizeToTrayOnCloseCheckBox, "完全終了せず、タスクトレイに格納します。")
            }));

        panel.Controls.Add(CreateSettingsCategoryPanel(
            "右クリックの便利機能",
            "項目を右クリックしたときに使える操作を増やします。",
            new Control[]
            {
                CreateSettingRow(_enableContextMenuDetailsCheckBox, "フォルダを開く、詳細を確認するなどの操作を追加します。")
            }));

        panel.Controls.Add(CreateSettingsCategoryPanel(
            "バックアップと削除",
            "登録内容と設定を守り、間違って消しにくくするための設定です。",
            new Control[]
            {
                CreateSettingRow(_autoBackupEnabledCheckBox, "登録内容と設定を自動でバックアップします。"),
                CreateComboRow("バックアップ保存数", _maxBackupCountNumeric, "残しておくバックアップの数です。初期値は20件です。"),
                CreateSettingRow(_confirmBeforeDeleteCheckBox, "削除前に確認して、間違って消しにくくします。"),
                CreateSettingRow(_moveDeletedItemsToTrashCheckBox, "すぐ完全削除せず、アプリ内のごみ箱であとから見直せます。")
            }));

        panel.Controls.Add(CreateSettingsCategoryPanel(
            "検索",
            "項目を探すとき、どこまで検索対象にするかを選びます。",
            new Control[]
            {
                CreateSettingRow(_searchTemplateBodyCheckBox, "テンプレート文の中身も検索対象にします。")
            }));

        UpdateSettingsEditability();
        return panel;
    }

    private Control BuildConfirmationPage()
    {
        FlowLayoutPanel panel = CreateVerticalPanel();
        panel.Controls.Add(CreateHeading("確認して開始"));
        panel.Controls.Add(CreateParagraph("選んだ内容を確認してください。「開始」を押すと、登録内容と設定の保存先を作成してContextBinderを起動します。"));

        AppSettings settings = CreateSettingsFromControls();
        panel.Controls.Add(CreateConfirmationSection("始め方", [
            _customSetupRadio.Checked ? "カスタム設定" : "初心者おすすめ設定"
        ]));
        panel.Controls.Add(CreateConfirmationSection("保存場所", [
            GetStorageModeDisplayName(GetSelectedStorageMode()),
            CreateStoragePreviewText()
        ]));
        panel.Controls.Add(CreateConfirmationSection("主な設定", BuildMainSettingsSummary(settings)));
        panel.Controls.Add(CreateConfirmationSection("作成されるもの", [
            "・contextbinder.store.json",
            "・settings.json",
            "・backups",
            "・trash"
        ]));
        panel.Controls.Add(CreateConfirmationSection("保存されるもの", [
            "・登録したファイル、フォルダ、URLの参照先",
            "・登録したテンプレート文",
            "・グループ名と並び順",
            "・表示設定や操作設定",
            "・自動バックアップ",
            "・ごみ箱、削除履歴"
        ]));
        panel.Controls.Add(CreateConfirmationSection("保存されないもの", [
            "・登録元のファイルそのもの",
            "・登録元の画像や動画そのもの",
            "・登録元のフォルダの中身"
        ]));
        return panel;
    }

    private static FlowLayoutPanel CreateVerticalPanel()
    {
        return new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoScroll = true,
            Padding = new Padding(0, 0, 8, 0)
        };
    }

    private static Label CreateHeading(string text)
    {
        return new Label
        {
            Text = text,
            Width = ContentWidth,
            Height = 36,
            Font = new Font(Control.DefaultFont, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleLeft
        };
    }

    private static Label CreateParagraph(string text)
    {
        return new Label
        {
            Text = text,
            Width = ContentWidth,
            AutoSize = true,
            MaximumSize = new Size(ContentWidth, 0),
            Margin = new Padding(0, 0, 0, 12)
        };
    }

    private static Panel CreateOptionPanel(RadioButton radioButton, string description, int height)
    {
        Panel panel = new()
        {
            Width = ContentWidth,
            Height = height,
            BorderStyle = BorderStyle.FixedSingle,
            Padding = new Padding(10),
            Margin = new Padding(0, 0, 0, 10)
        };

        radioButton.Location = new Point(10, 10);
        radioButton.Width = ContentWidth - 30;
        Label descriptionLabel = new()
        {
            Text = description,
            Location = new Point(30, 38),
            Width = ContentWidth - 50,
            Height = height - 48
        };
        panel.Controls.Add(radioButton);
        panel.Controls.Add(descriptionLabel);
        return panel;
    }

    private static Panel CreateStorageOptionPanel(RadioButton radioButton, string description, int height = 124)
    {
        return CreateOptionPanel(radioButton, description, height);
    }

    private static Panel CreateSettingsCategoryPanel(string title, string description, IReadOnlyCollection<Control> controls)
    {
        Panel sectionPanel = new()
        {
            Width = ContentWidth,
            BorderStyle = BorderStyle.FixedSingle,
            Padding = new Padding(10),
            Margin = new Padding(0, 6, 0, 12)
        };

        Label titleLabel = new()
        {
            Text = title,
            Location = new Point(10, 8),
            Width = ContentWidth - 24,
            Height = 24,
            Font = new Font(Control.DefaultFont, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleLeft
        };
        Label descriptionLabel = new()
        {
            Text = description,
            Location = new Point(10, 34),
            Width = ContentWidth - 24,
            Height = 36,
            ForeColor = SystemColors.GrayText
        };
        FlowLayoutPanel bodyPanel = new()
        {
            Location = new Point(10, 74),
            Width = ContentWidth - 24,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoSize = true
        };

        foreach (Control control in controls)
        {
            bodyPanel.Controls.Add(control);
        }

        int bodyHeight = controls.Sum(control => control.Height + control.Margin.Vertical);
        bodyPanel.Height = bodyHeight;
        sectionPanel.Height = 94 + bodyHeight;
        sectionPanel.Controls.Add(titleLabel);
        sectionPanel.Controls.Add(descriptionLabel);
        sectionPanel.Controls.Add(bodyPanel);
        return sectionPanel;
    }

    private static Panel CreateSettingRow(CheckBox checkBox, string description)
    {
        Panel panel = new()
        {
            Width = ContentWidth - 24,
            Height = 58,
            Margin = new Padding(0, 0, 0, 4)
        };
        checkBox.Location = new Point(0, 0);
        checkBox.Width = ContentWidth - 40;

        Label descriptionLabel = new()
        {
            Text = description,
            Location = new Point(24, 28),
            Width = ContentWidth - 64,
            Height = 28,
            ForeColor = SystemColors.GrayText
        };
        panel.Controls.Add(checkBox);
        panel.Controls.Add(descriptionLabel);
        return panel;
    }

    private static Panel CreateComboRow(string labelText, Control control, string description)
    {
        Panel panel = new()
        {
            Width = ContentWidth - 24,
            Height = 60,
            Margin = new Padding(0, 0, 0, 4)
        };
        Label label = new()
        {
            Text = labelText,
            Location = new Point(0, 4),
            Width = 170,
            Height = 24
        };
        control.Location = new Point(180, 0);
        Label descriptionLabel = new()
        {
            Text = description,
            Location = new Point(180, 28),
            Width = ContentWidth - 220,
            Height = 30,
            ForeColor = SystemColors.GrayText
        };
        panel.Controls.Add(label);
        panel.Controls.Add(control);
        panel.Controls.Add(descriptionLabel);
        return panel;
    }

    private static Panel CreateConfirmationSection(string title, IEnumerable<string> lines)
    {
        Panel sectionPanel = new()
        {
            Width = ContentWidth,
            BorderStyle = BorderStyle.FixedSingle,
            Padding = new Padding(10),
            Margin = new Padding(0, 0, 0, 10)
        };
        Label titleLabel = new()
        {
            Text = title,
            Location = new Point(10, 8),
            Width = ContentWidth - 24,
            Height = 24,
            Font = new Font(Control.DefaultFont, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleLeft
        };
        FlowLayoutPanel bodyPanel = new()
        {
            Location = new Point(10, 36),
            Width = ContentWidth - 24,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoSize = true
        };

        foreach (string line in lines)
        {
            Label lineLabel = new()
            {
                Text = line,
                AutoSize = true,
                MaximumSize = new Size(ContentWidth - 38, 0),
                Margin = new Padding(0, 0, 0, 4)
            };
            bodyPanel.Controls.Add(lineLabel);
        }

        int bodyHeight = bodyPanel.Controls.Cast<Control>().Sum(control => control.Height + control.Margin.Vertical);
        bodyPanel.Height = bodyHeight;
        sectionPanel.Height = Math.Max(76, 54 + bodyHeight);
        sectionPanel.Controls.Add(titleLabel);
        sectionPanel.Controls.Add(bodyPanel);
        return sectionPanel;
    }

    private void UpdateSettingsEditability()
    {
        bool canEdit = _customSetupRadio.Checked || _editRecommendedSettingsCheckBox.Checked;
        foreach (Control control in _editableSettingsControls)
        {
            control.Enabled = canEdit;
        }
    }

    private void UpdateStoragePreview()
    {
        _customDirectoryTextBox.Enabled = _customStorageRadio.Checked;
        _browseButton.Enabled = _customStorageRadio.Checked;

        SelectedStorageMode = GetSelectedStorageMode();
        CustomStorageDirectory = _customStorageRadio.Checked ? _customDirectoryTextBox.Text.Trim() : string.Empty;

        string description = SelectedStorageMode switch
        {
            StorageMode.Portable => "このアプリのフォルダに登録内容と設定を保存します。AppDataには保存しません。",
            StorageMode.Custom => "自分で選んだフォルダに登録内容と設定を保存します。AppDataには保存しません。",
            _ => "Windowsの標準的なアプリ用フォルダに登録内容と設定を保存します。"
        };

        _storageDescriptionLabel.Text = description;
        _storagePreviewLabel.Text = $"保存先プレビュー: {CreateStoragePreviewText()}";
    }

    private string CreateStoragePreviewText()
    {
        if (_customStorageRadio.Checked && string.IsNullOrWhiteSpace(_customDirectoryTextBox.Text))
        {
            return "未選択";
        }

        try
        {
            StorageLocation location = _storageLocationService.CreateLocation(
                GetSelectedStorageMode(),
                _customStorageRadio.Checked ? _customDirectoryTextBox.Text.Trim() : null);
            return location.DataDirectory;
        }
        catch (InvalidOperationException ex)
        {
            return ex.Message;
        }
    }

    private void LoadSettingsIntoControls(AppSettings settings)
    {
        _typeDisplayModeComboBox.SelectedIndex = settings.TypeDisplayMode switch
        {
            TypeDisplayMode.IconOnly => 1,
            TypeDisplayMode.TextOnly => 2,
            TypeDisplayMode.Hidden => 3,
            _ => 0
        };
        _showBeginnerHintsCheckBox.Checked = settings.ShowBeginnerHints;
        _showIconLegendCheckBox.Checked = settings.ShowIconLegend;
        _showOperationStatusCheckBox.Checked = settings.ShowOperationStatus;
        _confirmTitleOnDropAddCheckBox.Checked = settings.ConfirmTitleOnDropAdd;
        _focusExistingItemOnDuplicateCheckBox.Checked = settings.FocusExistingItemOnDuplicate;
        _enableGroupDropModifierShortcutsCheckBox.Checked = settings.EnableGroupDropModifierShortcuts;
        _confirmGroupDropCopyMoveCheckBox.Checked = settings.ConfirmGroupDropCopyMove;
        _enableItemDragReorderCheckBox.Checked = settings.EnableItemDragReorder;
        _enableExternalFileDropOutCheckBox.Checked = settings.EnableExternalFileDropOut;
        _enableExternalUrlTextDragOutCheckBox.Checked = settings.EnableExternalUrlTextDragOut;
        _enableExternalTemplateTextDragOutCheckBox.Checked = settings.EnableExternalTemplateTextDragOut;
        _minimizeToTrayOnCloseCheckBox.Checked = settings.MinimizeToTrayOnClose;
        _enableContextMenuDetailsCheckBox.Checked = settings.EnableContextMenuDetails;
        _autoBackupEnabledCheckBox.Checked = settings.AutoBackupEnabled;
        _maxBackupCountNumeric.Value = Math.Clamp(settings.MaxBackupCount, 1, 100);
        _confirmBeforeDeleteCheckBox.Checked = settings.ConfirmBeforeDelete;
        _moveDeletedItemsToTrashCheckBox.Checked = settings.MoveDeletedItemsToTrash;
        _searchTemplateBodyCheckBox.Checked = settings.SearchTemplateBody;
    }

    private AppSettings CreateSettingsFromControls()
    {
        AppSettings settings = AppSettingsFactory.CreateRecommended();
        settings.TypeDisplayMode = _typeDisplayModeComboBox.SelectedIndex switch
        {
            1 => TypeDisplayMode.IconOnly,
            2 => TypeDisplayMode.TextOnly,
            3 => TypeDisplayMode.Hidden,
            _ => TypeDisplayMode.IconAndText
        };
        settings.ShowBeginnerHints = _showBeginnerHintsCheckBox.Checked;
        settings.ShowIconLegend = _showIconLegendCheckBox.Checked;
        settings.ShowOperationStatus = _showOperationStatusCheckBox.Checked;
        settings.ConfirmTitleOnDropAdd = _confirmTitleOnDropAddCheckBox.Checked;
        settings.FocusExistingItemOnDuplicate = _focusExistingItemOnDuplicateCheckBox.Checked;
        settings.EnableGroupDropModifierShortcuts = _enableGroupDropModifierShortcutsCheckBox.Checked;
        settings.ConfirmGroupDropCopyMove = _confirmGroupDropCopyMoveCheckBox.Checked;
        settings.EnableItemDragReorder = _enableItemDragReorderCheckBox.Checked;
        settings.EnableExternalFileDropOut = _enableExternalFileDropOutCheckBox.Checked;
        settings.EnableExternalUrlTextDragOut = _enableExternalUrlTextDragOutCheckBox.Checked;
        settings.EnableExternalTemplateTextDragOut = _enableExternalTemplateTextDragOutCheckBox.Checked;
        settings.MinimizeToTrayOnClose = _minimizeToTrayOnCloseCheckBox.Checked;
        settings.EnableContextMenuDetails = _enableContextMenuDetailsCheckBox.Checked;
        settings.AutoBackupEnabled = _autoBackupEnabledCheckBox.Checked;
        settings.MaxBackupCount = (int)_maxBackupCountNumeric.Value;
        settings.ConfirmBeforeDelete = _confirmBeforeDeleteCheckBox.Checked;
        settings.MoveDeletedItemsToTrash = _moveDeletedItemsToTrashCheckBox.Checked;
        settings.SearchTemplateBody = _searchTemplateBodyCheckBox.Checked;
        return settings;
    }

    private void BrowseButton_Click(object? sender, EventArgs e)
    {
        using FolderBrowserDialog dialog = new()
        {
            Description = "登録内容と設定を保存するフォルダを選んでください。",
            UseDescriptionForTitle = true
        };

        if (!string.IsNullOrWhiteSpace(_customDirectoryTextBox.Text))
        {
            dialog.InitialDirectory = _customDirectoryTextBox.Text;
        }

        if (dialog.ShowDialog(this) == DialogResult.OK)
        {
            _customDirectoryTextBox.Text = dialog.SelectedPath;
        }
    }

    private void NextButton_Click(object? sender, EventArgs e)
    {
        if (!ValidateCurrentStep())
        {
            return;
        }

        if (_currentStep < LastStepIndex)
        {
            MoveStep(1);
            return;
        }

        SelectedStorageMode = GetSelectedStorageMode();
        CustomStorageDirectory = _customStorageRadio.Checked ? _customDirectoryTextBox.Text.Trim() : string.Empty;
        SelectedAppSettings = CreateSettingsFromControls();
        DialogResult = DialogResult.OK;
        Close();
    }

    private bool ValidateCurrentStep()
    {
        if (_currentStep == 1 && _customStorageRadio.Checked && string.IsNullOrWhiteSpace(_customDirectoryTextBox.Text))
        {
            MessageBox.Show(this, "自分で選んだ場所に保存する場合は、保存するフォルダを選んでください。", "入力確認", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (_currentStep == 2 && _typeDisplayModeComboBox.SelectedIndex == 3)
        {
            DialogResult result = MessageBox.Show(
                this,
                "種類表示を非表示にすると、項目の種類が分かりにくくなります。この設定で進みますか？",
                "種類表示の確認",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);
            return result == DialogResult.Yes;
        }

        return true;
    }

    private void MoveStep(int delta)
    {
        _currentStep = Math.Clamp(_currentStep + delta, 0, LastStepIndex);
        RenderCurrentStep();
    }

    private StorageMode GetSelectedStorageMode()
    {
        if (_portableStorageRadio.Checked)
        {
            return StorageMode.Portable;
        }

        return _customStorageRadio.Checked ? StorageMode.Custom : StorageMode.Standard;
    }

    private static string[] BuildMainSettingsSummary(AppSettings settings)
    {
        return
        [
            $"・種類表示: {GetTypeDisplayModeDisplayName(settings.TypeDisplayMode)}",
            $"・初心者向け説明: {ShowOrHide(settings.ShowBeginnerHints)}",
            $"・アイコンの意味: {ShowOrHide(settings.ShowIconLegend)}",
            $"・操作結果ステータス: {ShowOrHide(settings.ShowOperationStatus)}",
            $"・追加時にタイトル確認: {DoOrNot(settings.ConfirmTitleOnDropAdd)}",
            $"・重複時に既存項目へ移動: {DoOrNot(settings.FocusExistingItemOnDuplicate)}",
            $"・Ctrl/Shiftキーでコピー・移動を切り替え: {UseOrNot(settings.EnableGroupDropModifierShortcuts)}",
            $"・グループへドロップしたときの確認: {DoOrNot(settings.ConfirmGroupDropCopyMove)}",
            $"・項目のドラッグ並び替え: {UseOrNot(settings.EnableItemDragReorder)}",
            $"・閉じるボタンでタスクトレイに格納: {DoOrNot(settings.MinimizeToTrayOnClose)}",
            $"・右クリックの詳細操作: {UseOrNot(settings.EnableContextMenuDetails)}",
            $"・自動バックアップ: {UseOrNot(settings.AutoBackupEnabled)}（{settings.MaxBackupCount}件保持）",
            $"・削除前の確認: {DoOrNot(settings.ConfirmBeforeDelete)}",
            $"・削除時にアプリ内のごみ箱へ移動: {DoOrNot(settings.MoveDeletedItemsToTrash)}",
            $"・テンプレート本文も検索: {DoOrNot(settings.SearchTemplateBody)}"
        ];
    }

    private static string GetStorageModeDisplayName(StorageMode mode)
    {
        return mode switch
        {
            StorageMode.Portable => "このアプリのフォルダに保存",
            StorageMode.Custom => "自分で選んだ場所に保存",
            _ => "通常の場所に保存（おすすめ）"
        };
    }

    private static string GetTypeDisplayModeDisplayName(TypeDisplayMode mode)
    {
        return mode switch
        {
            TypeDisplayMode.IconOnly => "アイコンのみ",
            TypeDisplayMode.TextOnly => "文字のみ",
            TypeDisplayMode.Hidden => "非表示",
            _ => "アイコン＋文字"
        };
    }

    private static string ShowOrHide(bool value)
    {
        return value ? "表示する" : "表示しない";
    }

    private static string UseOrNot(bool value)
    {
        return value ? "使う" : "使わない";
    }

    private static string DoOrNot(bool value)
    {
        return value ? "する" : "しない";
    }
}
