using ContextBinder.Models;

namespace ContextBinder.Forms;

public sealed class FirstRunSetupForm : Form
{
    private readonly RadioButton _recommendedSetupRadio = new();
    private readonly RadioButton _customSetupRadio = new();
    private readonly GroupBox _storageGroup = new();
    private readonly RadioButton _standardStorageRadio = new();
    private readonly RadioButton _portableStorageRadio = new();
    private readonly RadioButton _customStorageRadio = new();
    private readonly TextBox _customDirectoryTextBox = new();
    private readonly Button _browseButton = new();

    public FirstRunSetupForm()
    {
        Text = "ContextBinder 初回セットアップ";
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ClientSize = new Size(680, 620);
        Font = SystemFonts.MessageBoxFont;

        BuildLayout();
        UpdateStorageControls();
    }

    public StorageMode SelectedStorageMode { get; private set; } = StorageMode.Standard;

    public string CustomStorageDirectory { get; private set; } = string.Empty;

    private void BuildLayout()
    {
        TableLayoutPanel root = new()
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(18),
            ColumnCount = 1,
            RowCount = 5
        };
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 74));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 96));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 188));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 46));

        Label leadLabel = new()
        {
            Dock = DockStyle.Fill,
            Text = "ContextBinder へようこそ。\n登録内容と設定をどこに保存するかを選んでください。",
            TextAlign = ContentAlignment.MiddleLeft
        };

        GroupBox setupGroup = new()
        {
            Dock = DockStyle.Fill,
            Text = "始め方"
        };
        _recommendedSetupRadio.Text = "初心者おすすめ設定で始める";
        _recommendedSetupRadio.Location = new Point(16, 28);
        _recommendedSetupRadio.Width = 300;
        _recommendedSetupRadio.Checked = true;
        _recommendedSetupRadio.CheckedChanged += (_, _) => UpdateStorageControls();

        // TODO: 表示設定や操作設定の詳細選択は、設定画面実装フェーズで追加する。
        _customSetupRadio.Text = "保存場所を自分で選んで始める";
        _customSetupRadio.Location = new Point(16, 58);
        _customSetupRadio.Width = 300;
        _customSetupRadio.CheckedChanged += (_, _) => UpdateStorageControls();
        setupGroup.Controls.AddRange([_recommendedSetupRadio, _customSetupRadio]);

        _storageGroup.Dock = DockStyle.Fill;
        _storageGroup.Text = "登録内容と設定の保存場所";

        _standardStorageRadio.Text = "通常の場所に保存（おすすめ）";
        _standardStorageRadio.Location = new Point(16, 30);
        _standardStorageRadio.Width = 420;
        _standardStorageRadio.Checked = true;

        _portableStorageRadio.Text = "このアプリのフォルダに保存";
        _portableStorageRadio.Location = new Point(16, 64);
        _portableStorageRadio.Width = 420;

        _customStorageRadio.Text = "自分で選んだ場所に保存";
        _customStorageRadio.Location = new Point(16, 98);
        _customStorageRadio.Width = 420;
        _customStorageRadio.CheckedChanged += (_, _) => UpdateStorageControls();

        _customDirectoryTextBox.Location = new Point(36, 132);
        _customDirectoryTextBox.Width = 500;

        _browseButton.Text = "参照...";
        _browseButton.Location = new Point(546, 130);
        _browseButton.Width = 88;
        _browseButton.Click += BrowseButton_Click;

        _storageGroup.Controls.AddRange([
            _standardStorageRadio,
            _portableStorageRadio,
            _customStorageRadio,
            _customDirectoryTextBox,
            _browseButton
        ]);

        TextBox explanationTextBox = new()
        {
            Dock = DockStyle.Fill,
            Multiline = true,
            ReadOnly = true,
            ScrollBars = ScrollBars.Vertical,
            Text = """
保存されるもの:
・登録したファイル、フォルダ、URLの参照先
・登録したテンプレート文
・グループ名と並び順
・表示設定や操作設定
・自動バックアップ
・ごみ箱、削除履歴

保存されないもの:
・登録元のファイルそのもの
・登録元の画像や動画そのもの
・登録元のフォルダの中身
"""
        };

        FlowLayoutPanel buttonPanel = new()
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft
        };

        Button startButton = new()
        {
            Text = "開始",
            Width = 88,
            DialogResult = DialogResult.OK
        };
        startButton.Click += StartButton_Click;

        Button cancelButton = new()
        {
            Text = "キャンセル",
            Width = 88,
            DialogResult = DialogResult.Cancel
        };

        buttonPanel.Controls.AddRange([cancelButton, startButton]);

        root.Controls.Add(leadLabel, 0, 0);
        root.Controls.Add(setupGroup, 0, 1);
        root.Controls.Add(_storageGroup, 0, 2);
        root.Controls.Add(explanationTextBox, 0, 3);
        root.Controls.Add(buttonPanel, 0, 4);
        Controls.Add(root);

        AcceptButton = startButton;
        CancelButton = cancelButton;
    }

    private void UpdateStorageControls()
    {
        bool customSetup = _customSetupRadio.Checked;
        _storageGroup.Enabled = customSetup;

        if (!customSetup)
        {
            _standardStorageRadio.Checked = true;
        }

        bool customDirectoryEnabled = customSetup && _customStorageRadio.Checked;
        _customDirectoryTextBox.Enabled = customDirectoryEnabled;
        _browseButton.Enabled = customDirectoryEnabled;
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

    private void StartButton_Click(object? sender, EventArgs e)
    {
        if (_recommendedSetupRadio.Checked || _standardStorageRadio.Checked)
        {
            SelectedStorageMode = StorageMode.Standard;
            CustomStorageDirectory = string.Empty;
            return;
        }

        if (_portableStorageRadio.Checked)
        {
            SelectedStorageMode = StorageMode.Portable;
            CustomStorageDirectory = string.Empty;
            return;
        }

        string customDirectory = _customDirectoryTextBox.Text.Trim();
        if (string.IsNullOrWhiteSpace(customDirectory))
        {
            MessageBox.Show(this, "保存するフォルダを選んでください。", "入力確認", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            DialogResult = DialogResult.None;
            return;
        }

        SelectedStorageMode = StorageMode.Custom;
        CustomStorageDirectory = customDirectory;
    }
}
