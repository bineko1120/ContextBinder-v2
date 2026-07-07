namespace ContextBinder.Forms;

public sealed partial class MainForm
{
    private TableLayoutPanel _rootLayout = null!;
    private Panel _groupPanel = null!;
    private Label _groupLabel = null!;
    private ListBox _groupListBox = null!;
    private DataGridView _itemGrid = null!;
    private FlowLayoutPanel _actionPanel = null!;
    private Button _addGroupButton = null!;
    private Button _addFileButton = null!;
    private Button _addFolderButton = null!;
    private Button _addUrlButton = null!;
    private Button _addTemplateButton = null!;
    private Button _openButton = null!;
    private Button _copyButton = null!;
    private Button _editButton = null!;
    private Button _detailButton = null!;
    private Button _deleteButton = null!;
    private TableLayoutPanel _bottomPanel = null!;
    private FlowLayoutPanel _displayTogglePanel = null!;
    private CheckBox _showBeginnerHintsCheckBox = null!;
    private CheckBox _showIconLegendCheckBox = null!;
    private Label _statusLabel = null!;
    private Panel _beginnerHintsPanel = null!;
    private TableLayoutPanel _beginnerHintsLayout = null!;
    private Label _beginnerHintsTitleLabel = null!;
    private Label _beginnerHintsLabel = null!;
    private Panel _iconLegendPanel = null!;
    private TableLayoutPanel _iconMeaningLayout = null!;
    private Label _iconMeaningTitleLabel = null!;
    private FlowLayoutPanel _iconLegendFlow = null!;
    private Label _iconMeaningPreviewFolderLabel = null!;
    private Label _iconMeaningPreviewFileLabel = null!;
    private Label _iconMeaningPreviewImageLabel = null!;
    private Label _iconMeaningPreviewVideoLabel = null!;
    private Label _iconMeaningPreviewUrlLabel = null!;
    private Label _iconMeaningPreviewTemplateLabel = null!;
    private ContextMenuStrip _itemContextMenu = null!;
    private NotifyIcon _notifyIcon = null!;
    private ContextMenuStrip _trayMenu = null!;

    private void InitializeComponent()
    {
        _rootLayout = new TableLayoutPanel();
        _groupPanel = new Panel();
        _groupLabel = new Label();
        _groupListBox = new ListBox();
        _itemGrid = new DataGridView();
        _itemContextMenu = new ContextMenuStrip();
        _actionPanel = new FlowLayoutPanel();
        _addGroupButton = new Button();
        _addFileButton = new Button();
        _addFolderButton = new Button();
        _addUrlButton = new Button();
        _addTemplateButton = new Button();
        _openButton = new Button();
        _copyButton = new Button();
        _editButton = new Button();
        _detailButton = new Button();
        _deleteButton = new Button();
        _bottomPanel = new TableLayoutPanel();
        _displayTogglePanel = new FlowLayoutPanel();
        _showBeginnerHintsCheckBox = new CheckBox();
        _showIconLegendCheckBox = new CheckBox();
        _statusLabel = new Label();
        _beginnerHintsPanel = new Panel();
        _beginnerHintsLayout = new TableLayoutPanel();
        _beginnerHintsTitleLabel = new Label();
        _beginnerHintsLabel = new Label();
        _iconLegendPanel = new Panel();
        _iconMeaningLayout = new TableLayoutPanel();
        _iconMeaningTitleLabel = new Label();
        _iconLegendFlow = new FlowLayoutPanel();
        _iconMeaningPreviewFolderLabel = new Label();
        _iconMeaningPreviewFileLabel = new Label();
        _iconMeaningPreviewImageLabel = new Label();
        _iconMeaningPreviewVideoLabel = new Label();
        _iconMeaningPreviewUrlLabel = new Label();
        _iconMeaningPreviewTemplateLabel = new Label();
        _trayMenu = new ContextMenuStrip();
        _notifyIcon = new NotifyIcon();
        ((System.ComponentModel.ISupportInitialize)_itemGrid).BeginInit();
        _rootLayout.SuspendLayout();
        _groupPanel.SuspendLayout();
        _actionPanel.SuspendLayout();
        _bottomPanel.SuspendLayout();
        _displayTogglePanel.SuspendLayout();
        _beginnerHintsPanel.SuspendLayout();
        _beginnerHintsLayout.SuspendLayout();
        _iconLegendPanel.SuspendLayout();
        _iconMeaningLayout.SuspendLayout();
        _iconLegendFlow.SuspendLayout();
        SuspendLayout();

        _rootLayout.ColumnCount = 3;
        _rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 190F));
        _rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 185F));
        _rootLayout.Controls.Add(_groupPanel, 0, 0);
        _rootLayout.Controls.Add(_itemGrid, 1, 0);
        _rootLayout.Controls.Add(_actionPanel, 2, 0);
        _rootLayout.Controls.Add(_bottomPanel, 0, 1);
        _rootLayout.Dock = DockStyle.Fill;
        _rootLayout.Location = new Point(0, 0);
        _rootLayout.Name = "_rootLayout";
        _rootLayout.Padding = new Padding(10);
        _rootLayout.RowCount = 2;
        _rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        _rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 316F));
        _rootLayout.SetColumnSpan(_bottomPanel, 3);
        _rootLayout.Size = new Size(1180, 780);
        _rootLayout.TabIndex = 0;

        _groupPanel.Controls.Add(_groupListBox);
        _groupPanel.Controls.Add(_groupLabel);
        _groupPanel.Dock = DockStyle.Fill;
        _groupPanel.Location = new Point(13, 13);
        _groupPanel.Name = "_groupPanel";
        _groupPanel.Padding = new Padding(0, 0, 8, 0);
        _groupPanel.Size = new Size(184, 438);
        _groupPanel.TabIndex = 0;

        _groupLabel.Dock = DockStyle.Top;
        _groupLabel.Font = new Font("Yu Gothic UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
        _groupLabel.Location = new Point(0, 0);
        _groupLabel.Name = "_groupLabel";
        _groupLabel.Size = new Size(176, 24);
        _groupLabel.TabIndex = 0;
        _groupLabel.Text = "グループ一覧";
        _groupLabel.TextAlign = ContentAlignment.MiddleLeft;

        _groupListBox.Dock = DockStyle.Fill;
        _groupListBox.FormattingEnabled = true;
        _groupListBox.ItemHeight = 20;
        _groupListBox.Location = new Point(0, 24);
        _groupListBox.Name = "_groupListBox";
        _groupListBox.Size = new Size(176, 414);
        _groupListBox.TabIndex = 1;

        _itemGrid.AllowUserToAddRows = false;
        _itemGrid.AllowUserToDeleteRows = false;
        _itemGrid.AllowUserToResizeRows = false;
        _itemGrid.AutoGenerateColumns = false;
        _itemGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _itemGrid.BackgroundColor = SystemColors.Window;
        _itemGrid.BorderStyle = BorderStyle.FixedSingle;
        _itemGrid.ContextMenuStrip = _itemContextMenu;
        _itemGrid.Dock = DockStyle.Fill;
        _itemGrid.Location = new Point(203, 13);
        _itemGrid.MultiSelect = false;
        _itemGrid.Name = "_itemGrid";
        _itemGrid.ReadOnly = true;
        _itemGrid.RowHeadersVisible = false;
        _itemGrid.RowTemplate.Height = 36;
        _itemGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _itemGrid.Size = new Size(779, 438);
        _itemGrid.TabIndex = 1;

        _actionPanel.AutoScroll = true;
        _actionPanel.Controls.Add(_addGroupButton);
        _actionPanel.Controls.Add(_addFileButton);
        _actionPanel.Controls.Add(_addFolderButton);
        _actionPanel.Controls.Add(_addUrlButton);
        _actionPanel.Controls.Add(_addTemplateButton);
        _actionPanel.Controls.Add(_openButton);
        _actionPanel.Controls.Add(_copyButton);
        _actionPanel.Controls.Add(_editButton);
        _actionPanel.Controls.Add(_detailButton);
        _actionPanel.Controls.Add(_deleteButton);
        _actionPanel.Dock = DockStyle.Fill;
        _actionPanel.FlowDirection = FlowDirection.TopDown;
        _actionPanel.Location = new Point(988, 13);
        _actionPanel.Name = "_actionPanel";
        _actionPanel.Padding = new Padding(8, 0, 0, 0);
        _actionPanel.Size = new Size(179, 438);
        _actionPanel.TabIndex = 2;
        _actionPanel.WrapContents = false;

        _addGroupButton.Margin = new Padding(0, 0, 0, 7);
        _addGroupButton.Name = "_addGroupButton";
        _addGroupButton.Size = new Size(165, 31);
        _addGroupButton.TabIndex = 0;
        _addGroupButton.Text = "グループ追加";
        _addGroupButton.UseVisualStyleBackColor = true;

        _addFileButton.Margin = new Padding(0, 0, 0, 7);
        _addFileButton.Name = "_addFileButton";
        _addFileButton.Size = new Size(165, 31);
        _addFileButton.TabIndex = 1;
        _addFileButton.Text = "ファイル追加";
        _addFileButton.UseVisualStyleBackColor = true;

        _addFolderButton.Margin = new Padding(0, 0, 0, 7);
        _addFolderButton.Name = "_addFolderButton";
        _addFolderButton.Size = new Size(165, 31);
        _addFolderButton.TabIndex = 2;
        _addFolderButton.Text = "フォルダー追加";
        _addFolderButton.UseVisualStyleBackColor = true;

        _addUrlButton.Margin = new Padding(0, 0, 0, 7);
        _addUrlButton.Name = "_addUrlButton";
        _addUrlButton.Size = new Size(165, 31);
        _addUrlButton.TabIndex = 3;
        _addUrlButton.Text = "URL追加";
        _addUrlButton.UseVisualStyleBackColor = true;

        _addTemplateButton.Margin = new Padding(0, 0, 0, 7);
        _addTemplateButton.Name = "_addTemplateButton";
        _addTemplateButton.Size = new Size(165, 31);
        _addTemplateButton.TabIndex = 4;
        _addTemplateButton.Text = "テンプレート追加";
        _addTemplateButton.UseVisualStyleBackColor = true;

        _openButton.Margin = new Padding(0, 0, 0, 7);
        _openButton.Name = "_openButton";
        _openButton.Size = new Size(165, 31);
        _openButton.TabIndex = 5;
        _openButton.Text = "開く";
        _openButton.UseVisualStyleBackColor = true;

        _copyButton.Margin = new Padding(0, 0, 0, 7);
        _copyButton.Name = "_copyButton";
        _copyButton.Size = new Size(165, 31);
        _copyButton.TabIndex = 6;
        _copyButton.Text = "コピー";
        _copyButton.UseVisualStyleBackColor = true;

        _editButton.Margin = new Padding(0, 0, 0, 7);
        _editButton.Name = "_editButton";
        _editButton.Size = new Size(165, 31);
        _editButton.TabIndex = 7;
        _editButton.Text = "編集";
        _editButton.UseVisualStyleBackColor = true;

        _detailButton.Margin = new Padding(0, 0, 0, 7);
        _detailButton.Name = "_detailButton";
        _detailButton.Size = new Size(165, 31);
        _detailButton.TabIndex = 8;
        _detailButton.Text = "詳細";
        _detailButton.UseVisualStyleBackColor = true;

        _deleteButton.Margin = new Padding(0, 0, 0, 7);
        _deleteButton.Name = "_deleteButton";
        _deleteButton.Size = new Size(165, 31);
        _deleteButton.TabIndex = 9;
        _deleteButton.Text = "削除";
        _deleteButton.UseVisualStyleBackColor = true;

        _bottomPanel.ColumnCount = 1;
        _bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _bottomPanel.Controls.Add(_displayTogglePanel, 0, 0);
        _bottomPanel.Controls.Add(_beginnerHintsPanel, 0, 1);
        _bottomPanel.Controls.Add(_iconLegendPanel, 0, 2);
        _bottomPanel.Dock = DockStyle.Fill;
        _bottomPanel.Location = new Point(13, 457);
        _bottomPanel.Name = "_bottomPanel";
        _bottomPanel.Padding = new Padding(0, 8, 0, 0);
        _bottomPanel.RowCount = 3;
        _bottomPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
        _bottomPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 124F));
        _bottomPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 148F));
        _bottomPanel.Size = new Size(1154, 310);
        _bottomPanel.TabIndex = 3;

        _displayTogglePanel.Controls.Add(_showBeginnerHintsCheckBox);
        _displayTogglePanel.Controls.Add(_showIconLegendCheckBox);
        _displayTogglePanel.Controls.Add(_statusLabel);
        _displayTogglePanel.Dock = DockStyle.Fill;
        _displayTogglePanel.FlowDirection = FlowDirection.LeftToRight;
        _displayTogglePanel.Location = new Point(3, 11);
        _displayTogglePanel.Name = "_displayTogglePanel";
        _displayTogglePanel.Size = new Size(1148, 28);
        _displayTogglePanel.TabIndex = 0;
        _displayTogglePanel.WrapContents = true;

        _showBeginnerHintsCheckBox.Name = "_showBeginnerHintsCheckBox";
        _showBeginnerHintsCheckBox.Size = new Size(170, 24);
        _showBeginnerHintsCheckBox.TabIndex = 0;
        _showBeginnerHintsCheckBox.Text = "初心者向け説明を表示";
        _showBeginnerHintsCheckBox.UseVisualStyleBackColor = true;

        _showIconLegendCheckBox.Name = "_showIconLegendCheckBox";
        _showIconLegendCheckBox.Size = new Size(170, 24);
        _showIconLegendCheckBox.TabIndex = 1;
        _showIconLegendCheckBox.Text = "アイコンの意味を表示";
        _showIconLegendCheckBox.UseVisualStyleBackColor = true;

        _statusLabel.AutoEllipsis = true;
        _statusLabel.Name = "_statusLabel";
        _statusLabel.Size = new Size(520, 26);
        _statusLabel.TabIndex = 2;
        _statusLabel.Text = "ここに操作結果を表示します。";
        _statusLabel.TextAlign = ContentAlignment.MiddleLeft;

        _beginnerHintsPanel.BorderStyle = BorderStyle.FixedSingle;
        _beginnerHintsPanel.Controls.Add(_beginnerHintsLayout);
        _beginnerHintsPanel.Dock = DockStyle.Fill;
        _beginnerHintsPanel.Location = new Point(3, 45);
        _beginnerHintsPanel.Name = "_beginnerHintsPanel";
        _beginnerHintsPanel.Padding = new Padding(8);
        _beginnerHintsPanel.Size = new Size(1148, 118);
        _beginnerHintsPanel.TabIndex = 1;

        _beginnerHintsLayout.ColumnCount = 1;
        _beginnerHintsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _beginnerHintsLayout.Controls.Add(_beginnerHintsTitleLabel, 0, 0);
        _beginnerHintsLayout.Controls.Add(_beginnerHintsLabel, 0, 1);
        _beginnerHintsLayout.Dock = DockStyle.Fill;
        _beginnerHintsLayout.Location = new Point(8, 8);
        _beginnerHintsLayout.Name = "_beginnerHintsLayout";
        _beginnerHintsLayout.RowCount = 2;
        _beginnerHintsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
        _beginnerHintsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        _beginnerHintsLayout.Size = new Size(1130, 100);
        _beginnerHintsLayout.TabIndex = 0;

        _beginnerHintsTitleLabel.Dock = DockStyle.Fill;
        _beginnerHintsTitleLabel.Font = new Font("Yu Gothic UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
        _beginnerHintsTitleLabel.Name = "_beginnerHintsTitleLabel";
        _beginnerHintsTitleLabel.Size = new Size(1130, 24);
        _beginnerHintsTitleLabel.TabIndex = 0;
        _beginnerHintsTitleLabel.Text = "使い方のヒント";
        _beginnerHintsTitleLabel.TextAlign = ContentAlignment.MiddleLeft;

        _beginnerHintsLabel.Dock = DockStyle.Fill;
        _beginnerHintsLabel.Location = new Point(3, 24);
        _beginnerHintsLabel.Name = "_beginnerHintsLabel";
        _beginnerHintsLabel.Size = new Size(1124, 76);
        _beginnerHintsLabel.TabIndex = 1;
        _beginnerHintsLabel.Text = "・グループ追加: 用途ごとに登録先を分けます。\r\n・ファイル / フォルダー / URL / テンプレート追加: 参照先や文章を登録します。\r\n・開く / コピー: 選択した項目を開いたり、パス・URL・本文をコピーします。\r\n・編集 / 詳細 / 削除: 登録内容を確認・整理します。";
        _beginnerHintsLabel.TextAlign = ContentAlignment.TopLeft;

        _iconLegendPanel.BorderStyle = BorderStyle.FixedSingle;
        _iconLegendPanel.Controls.Add(_iconMeaningLayout);
        _iconLegendPanel.Dock = DockStyle.Fill;
        _iconLegendPanel.Location = new Point(3, 169);
        _iconLegendPanel.Name = "_iconLegendPanel";
        _iconLegendPanel.Padding = new Padding(8);
        _iconLegendPanel.Size = new Size(1148, 142);
        _iconLegendPanel.TabIndex = 2;

        _iconMeaningLayout.ColumnCount = 1;
        _iconMeaningLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _iconMeaningLayout.Controls.Add(_iconMeaningTitleLabel, 0, 0);
        _iconMeaningLayout.Controls.Add(_iconLegendFlow, 0, 1);
        _iconMeaningLayout.Dock = DockStyle.Fill;
        _iconMeaningLayout.Location = new Point(8, 8);
        _iconMeaningLayout.Name = "_iconMeaningLayout";
        _iconMeaningLayout.RowCount = 2;
        _iconMeaningLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
        _iconMeaningLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        _iconMeaningLayout.Size = new Size(1130, 124);
        _iconMeaningLayout.TabIndex = 0;

        _iconMeaningTitleLabel.Dock = DockStyle.Fill;
        _iconMeaningTitleLabel.Font = new Font("Yu Gothic UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
        _iconMeaningTitleLabel.Name = "_iconMeaningTitleLabel";
        _iconMeaningTitleLabel.Size = new Size(1130, 24);
        _iconMeaningTitleLabel.TabIndex = 0;
        _iconMeaningTitleLabel.Text = "アイコンの意味";
        _iconMeaningTitleLabel.TextAlign = ContentAlignment.MiddleLeft;

        _iconLegendFlow.AutoScroll = true;
        _iconLegendFlow.Controls.Add(_iconMeaningPreviewFolderLabel);
        _iconLegendFlow.Controls.Add(_iconMeaningPreviewFileLabel);
        _iconLegendFlow.Controls.Add(_iconMeaningPreviewImageLabel);
        _iconLegendFlow.Controls.Add(_iconMeaningPreviewVideoLabel);
        _iconLegendFlow.Controls.Add(_iconMeaningPreviewUrlLabel);
        _iconLegendFlow.Controls.Add(_iconMeaningPreviewTemplateLabel);
        _iconLegendFlow.Dock = DockStyle.Fill;
        _iconLegendFlow.FlowDirection = FlowDirection.LeftToRight;
        _iconLegendFlow.Location = new Point(3, 27);
        _iconLegendFlow.Name = "_iconLegendFlow";
        _iconLegendFlow.Size = new Size(1124, 94);
        _iconLegendFlow.TabIndex = 1;
        _iconLegendFlow.WrapContents = true;

        _iconMeaningPreviewFolderLabel.BorderStyle = BorderStyle.FixedSingle;
        _iconMeaningPreviewFolderLabel.Margin = new Padding(0, 0, 8, 6);
        _iconMeaningPreviewFolderLabel.Name = "_iconMeaningPreviewFolderLabel";
        _iconMeaningPreviewFolderLabel.Padding = new Padding(8, 0, 0, 0);
        _iconMeaningPreviewFolderLabel.Size = new Size(250, 44);
        _iconMeaningPreviewFolderLabel.TabIndex = 0;
        _iconMeaningPreviewFolderLabel.Text = "フォルダー: フォルダーを開きます";
        _iconMeaningPreviewFolderLabel.TextAlign = ContentAlignment.MiddleLeft;

        _iconMeaningPreviewFileLabel.BorderStyle = BorderStyle.FixedSingle;
        _iconMeaningPreviewFileLabel.Margin = new Padding(0, 0, 8, 6);
        _iconMeaningPreviewFileLabel.Name = "_iconMeaningPreviewFileLabel";
        _iconMeaningPreviewFileLabel.Padding = new Padding(8, 0, 0, 0);
        _iconMeaningPreviewFileLabel.Size = new Size(250, 44);
        _iconMeaningPreviewFileLabel.TabIndex = 1;
        _iconMeaningPreviewFileLabel.Text = "ファイル: 既定のアプリで開きます";
        _iconMeaningPreviewFileLabel.TextAlign = ContentAlignment.MiddleLeft;

        _iconMeaningPreviewImageLabel.BorderStyle = BorderStyle.FixedSingle;
        _iconMeaningPreviewImageLabel.Margin = new Padding(0, 0, 8, 6);
        _iconMeaningPreviewImageLabel.Name = "_iconMeaningPreviewImageLabel";
        _iconMeaningPreviewImageLabel.Padding = new Padding(8, 0, 0, 0);
        _iconMeaningPreviewImageLabel.Size = new Size(250, 44);
        _iconMeaningPreviewImageLabel.TabIndex = 2;
        _iconMeaningPreviewImageLabel.Text = "画像: 画像ファイルです";
        _iconMeaningPreviewImageLabel.TextAlign = ContentAlignment.MiddleLeft;

        _iconMeaningPreviewVideoLabel.BorderStyle = BorderStyle.FixedSingle;
        _iconMeaningPreviewVideoLabel.Margin = new Padding(0, 0, 8, 6);
        _iconMeaningPreviewVideoLabel.Name = "_iconMeaningPreviewVideoLabel";
        _iconMeaningPreviewVideoLabel.Padding = new Padding(8, 0, 0, 0);
        _iconMeaningPreviewVideoLabel.Size = new Size(250, 44);
        _iconMeaningPreviewVideoLabel.TabIndex = 3;
        _iconMeaningPreviewVideoLabel.Text = "動画: 動画ファイルです";
        _iconMeaningPreviewVideoLabel.TextAlign = ContentAlignment.MiddleLeft;

        _iconMeaningPreviewUrlLabel.BorderStyle = BorderStyle.FixedSingle;
        _iconMeaningPreviewUrlLabel.Margin = new Padding(0, 0, 8, 6);
        _iconMeaningPreviewUrlLabel.Name = "_iconMeaningPreviewUrlLabel";
        _iconMeaningPreviewUrlLabel.Padding = new Padding(8, 0, 0, 0);
        _iconMeaningPreviewUrlLabel.Size = new Size(250, 44);
        _iconMeaningPreviewUrlLabel.TabIndex = 4;
        _iconMeaningPreviewUrlLabel.Text = "URL: ブラウザで開きます";
        _iconMeaningPreviewUrlLabel.TextAlign = ContentAlignment.MiddleLeft;

        _iconMeaningPreviewTemplateLabel.BorderStyle = BorderStyle.FixedSingle;
        _iconMeaningPreviewTemplateLabel.Margin = new Padding(0, 0, 8, 6);
        _iconMeaningPreviewTemplateLabel.Name = "_iconMeaningPreviewTemplateLabel";
        _iconMeaningPreviewTemplateLabel.Padding = new Padding(8, 0, 0, 0);
        _iconMeaningPreviewTemplateLabel.Size = new Size(250, 44);
        _iconMeaningPreviewTemplateLabel.TabIndex = 5;
        _iconMeaningPreviewTemplateLabel.Text = "テンプレート: 本文をコピーします";
        _iconMeaningPreviewTemplateLabel.TextAlign = ContentAlignment.MiddleLeft;

        AllowDrop = true;
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1180, 780);
        Controls.Add(_rootLayout);
        Font = new Font("Yu Gothic UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
        MinimumSize = new Size(980, 700);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "ContextBinder v2";

        _iconLegendFlow.ResumeLayout(false);
        _iconMeaningLayout.ResumeLayout(false);
        _iconLegendPanel.ResumeLayout(false);
        _beginnerHintsLayout.ResumeLayout(false);
        _beginnerHintsPanel.ResumeLayout(false);
        _displayTogglePanel.ResumeLayout(false);
        _bottomPanel.ResumeLayout(false);
        _actionPanel.ResumeLayout(false);
        _groupPanel.ResumeLayout(false);
        _rootLayout.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)_itemGrid).EndInit();
        ResumeLayout(false);
    }
}
