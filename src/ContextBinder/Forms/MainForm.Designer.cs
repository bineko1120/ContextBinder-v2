using MainFormLayout = ContextBinder.UiLayoutSettings.Main;
using MainFormTexts = ContextBinder.UiTexts.MainForm;

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
        _itemContextMenu = new ContextMenuStrip();
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
        SuspendLayout();

        _rootLayout.ColumnCount = 3;
        _rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, MainFormLayout.GroupColumnWidth));
        _rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, MainFormLayout.ActionColumnWidth));
        _rootLayout.Controls.Add(_groupPanel, 0, 0);
        _rootLayout.Controls.Add(_itemGrid, 1, 0);
        _rootLayout.Controls.Add(_actionPanel, 2, 0);
        _rootLayout.Controls.Add(_bottomPanel, 0, 1);
        _rootLayout.Dock = DockStyle.Fill;
        _rootLayout.Location = new Point(0, 0);
        _rootLayout.Name = "_rootLayout";
        _rootLayout.Padding = new Padding(MainFormLayout.RootPadding);
        _rootLayout.RowCount = 2;
        _rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        _rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, MainFormLayout.InitialBottomHeight));
        _rootLayout.SetColumnSpan(_bottomPanel, 3);
        _rootLayout.Size = new Size(MainFormLayout.InitialWidth, MainFormLayout.InitialHeight);
        _rootLayout.TabIndex = 0;
        _bottomRowStyle = _rootLayout.RowStyles[1];

        _groupPanel.Controls.Add(_groupListBox);
        _groupPanel.Controls.Add(_groupLabel);
        _groupPanel.Dock = DockStyle.Fill;
        _groupPanel.Location = new Point(13, 13);
        _groupPanel.Name = "_groupPanel";
        _groupPanel.Padding = new Padding(0, 0, 8, 0);
        _groupPanel.Size = new Size(MainFormLayout.GroupColumnWidth - 3, 542);
        _groupPanel.TabIndex = 0;

        _groupLabel.Dock = DockStyle.Top;
        _groupLabel.Height = MainFormLayout.GroupHeaderHeight;
        _groupLabel.Name = "_groupLabel";
        _groupLabel.Text = MainFormTexts.GroupListTitle;
        _groupLabel.TextAlign = ContentAlignment.MiddleLeft;

        _groupListBox.Dock = DockStyle.Fill;
        _groupListBox.FormattingEnabled = true;
        _groupListBox.ItemHeight = 15;
        _groupListBox.Location = new Point(0, MainFormLayout.GroupHeaderHeight);
        _groupListBox.Name = "_groupListBox";
        _groupListBox.Size = new Size(MainFormLayout.GroupColumnWidth - 11, 518);
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
        _itemGrid.RowTemplate.Height = MainFormLayout.GridRowHeight;
        _itemGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _itemGrid.Size = new Size(779, 542);
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
        _actionPanel.Size = new Size(MainFormLayout.ActionColumnWidth - 3, 542);
        _actionPanel.TabIndex = 2;
        _actionPanel.WrapContents = false;

        _addGroupButton.Margin = new Padding(0, 0, 0, MainFormLayout.ActionButtonBottomMargin);
        _addGroupButton.Name = nameof(_addGroupButton);
        _addGroupButton.Size = new Size(MainFormLayout.ActionButtonWidth, MainFormLayout.ActionButtonHeight);
        _addGroupButton.TabIndex = 0;
        _addGroupButton.Text = MainFormTexts.AddGroupButton.Label;
        _addGroupButton.UseVisualStyleBackColor = true;

        _addFileButton.Margin = new Padding(0, 0, 0, MainFormLayout.ActionButtonBottomMargin);
        _addFileButton.Name = nameof(_addFileButton);
        _addFileButton.Size = new Size(MainFormLayout.ActionButtonWidth, MainFormLayout.ActionButtonHeight);
        _addFileButton.TabIndex = 1;
        _addFileButton.Text = MainFormTexts.AddFileButton.Label;
        _addFileButton.UseVisualStyleBackColor = true;

        _addFolderButton.Margin = new Padding(0, 0, 0, MainFormLayout.ActionButtonBottomMargin);
        _addFolderButton.Name = nameof(_addFolderButton);
        _addFolderButton.Size = new Size(MainFormLayout.ActionButtonWidth, MainFormLayout.ActionButtonHeight);
        _addFolderButton.TabIndex = 2;
        _addFolderButton.Text = MainFormTexts.AddFolderButton.Label;
        _addFolderButton.UseVisualStyleBackColor = true;

        _addUrlButton.Margin = new Padding(0, 0, 0, MainFormLayout.ActionButtonBottomMargin);
        _addUrlButton.Name = nameof(_addUrlButton);
        _addUrlButton.Size = new Size(MainFormLayout.ActionButtonWidth, MainFormLayout.ActionButtonHeight);
        _addUrlButton.TabIndex = 3;
        _addUrlButton.Text = MainFormTexts.AddUrlButton.Label;
        _addUrlButton.UseVisualStyleBackColor = true;

        _addTemplateButton.Margin = new Padding(0, 0, 0, MainFormLayout.ActionButtonBottomMargin);
        _addTemplateButton.Name = nameof(_addTemplateButton);
        _addTemplateButton.Size = new Size(MainFormLayout.ActionButtonWidth, MainFormLayout.ActionButtonHeight);
        _addTemplateButton.TabIndex = 4;
        _addTemplateButton.Text = MainFormTexts.AddTemplateButton.Label;
        _addTemplateButton.UseVisualStyleBackColor = true;

        _openButton.Margin = new Padding(0, 0, 0, MainFormLayout.ActionButtonBottomMargin);
        _openButton.Name = nameof(_openButton);
        _openButton.Size = new Size(MainFormLayout.ActionButtonWidth, MainFormLayout.ActionButtonHeight);
        _openButton.TabIndex = 5;
        _openButton.Text = MainFormTexts.OpenButton.Label;
        _openButton.UseVisualStyleBackColor = true;

        _copyButton.Margin = new Padding(0, 0, 0, MainFormLayout.ActionButtonBottomMargin);
        _copyButton.Name = nameof(_copyButton);
        _copyButton.Size = new Size(MainFormLayout.ActionButtonWidth, MainFormLayout.ActionButtonHeight);
        _copyButton.TabIndex = 6;
        _copyButton.Text = MainFormTexts.CopyButton.Label;
        _copyButton.UseVisualStyleBackColor = true;

        _editButton.Margin = new Padding(0, 0, 0, MainFormLayout.ActionButtonBottomMargin);
        _editButton.Name = nameof(_editButton);
        _editButton.Size = new Size(MainFormLayout.ActionButtonWidth, MainFormLayout.ActionButtonHeight);
        _editButton.TabIndex = 7;
        _editButton.Text = MainFormTexts.EditButton.Label;
        _editButton.UseVisualStyleBackColor = true;

        _detailButton.Margin = new Padding(0, 0, 0, MainFormLayout.ActionButtonBottomMargin);
        _detailButton.Name = nameof(_detailButton);
        _detailButton.Size = new Size(MainFormLayout.ActionButtonWidth, MainFormLayout.ActionButtonHeight);
        _detailButton.TabIndex = 8;
        _detailButton.Text = MainFormTexts.DetailButton.Label;
        _detailButton.UseVisualStyleBackColor = true;

        _deleteButton.Margin = new Padding(0, 0, 0, MainFormLayout.ActionButtonBottomMargin);
        _deleteButton.Name = nameof(_deleteButton);
        _deleteButton.Size = new Size(MainFormLayout.ActionButtonWidth, MainFormLayout.ActionButtonHeight);
        _deleteButton.TabIndex = 9;
        _deleteButton.Text = MainFormTexts.DeleteButton.Label;
        _deleteButton.UseVisualStyleBackColor = true;

        _bottomPanel.ColumnCount = 1;
        _bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _bottomPanel.Controls.Add(_displayTogglePanel, 0, 0);
        _bottomPanel.Controls.Add(_beginnerHintsPanel, 0, 1);
        _bottomPanel.Controls.Add(_iconLegendPanel, 0, 2);
        _bottomPanel.Dock = DockStyle.Fill;
        _bottomPanel.Location = new Point(13, 561);
        _bottomPanel.Name = "_bottomPanel";
        _bottomPanel.Padding = new Padding(0, MainFormLayout.BottomTopPadding, 0, 0);
        _bottomPanel.RowCount = 3;
        _bottomPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, MainFormLayout.ToggleRowHeight));
        _bottomPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, MainFormLayout.BeginnerHintsHeight));
        _bottomPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, MainFormLayout.IconMeaningHeight));
        _bottomPanel.Size = new Size(1154, MainFormLayout.InitialBottomHeight);
        _bottomPanel.TabIndex = 3;

        _displayTogglePanel.Controls.Add(_showBeginnerHintsCheckBox);
        _displayTogglePanel.Controls.Add(_showIconLegendCheckBox);
        _displayTogglePanel.Controls.Add(_statusLabel);
        _displayTogglePanel.Dock = DockStyle.Fill;
        _displayTogglePanel.FlowDirection = FlowDirection.LeftToRight;
        _displayTogglePanel.Location = new Point(3, MainFormLayout.BottomTopPadding + 3);
        _displayTogglePanel.Name = "_displayTogglePanel";
        _displayTogglePanel.Size = new Size(1148, MainFormLayout.ToggleRowHeight - 6);
        _displayTogglePanel.TabIndex = 0;
        _displayTogglePanel.WrapContents = true;

        _showBeginnerHintsCheckBox.Name = "_showBeginnerHintsCheckBox";
        _showBeginnerHintsCheckBox.Size = new Size(MainFormLayout.ToggleCheckBoxWidth, 24);
        _showBeginnerHintsCheckBox.Text = MainFormTexts.ShowBeginnerHintsToggle;
        _showBeginnerHintsCheckBox.UseVisualStyleBackColor = true;

        _showIconLegendCheckBox.Name = "_showIconLegendCheckBox";
        _showIconLegendCheckBox.Size = new Size(MainFormLayout.ToggleCheckBoxWidth, 24);
        _showIconLegendCheckBox.Text = MainFormTexts.ShowIconMeaningToggle;
        _showIconLegendCheckBox.UseVisualStyleBackColor = true;

        _statusLabel.AutoEllipsis = true;
        _statusLabel.Name = "_statusLabel";
        _statusLabel.Size = new Size(MainFormLayout.StatusLabelWidth, MainFormLayout.StatusLabelHeight);
        _statusLabel.TextAlign = ContentAlignment.MiddleLeft;

        _beginnerHintsPanel.BorderStyle = BorderStyle.FixedSingle;
        _beginnerHintsPanel.Controls.Add(_beginnerHintsLayout);
        _beginnerHintsPanel.Dock = DockStyle.Fill;
        _beginnerHintsPanel.Location = new Point(3, 45);
        _beginnerHintsPanel.Name = "_beginnerHintsPanel";
        _beginnerHintsPanel.Padding = new Padding(8);
        _beginnerHintsPanel.Size = new Size(1148, MainFormLayout.BeginnerHintsHeight - 6);
        _beginnerHintsPanel.TabIndex = 1;

        _beginnerHintsLayout.ColumnCount = 1;
        _beginnerHintsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _beginnerHintsLayout.Controls.Add(_beginnerHintsTitleLabel, 0, 0);
        _beginnerHintsLayout.Controls.Add(_beginnerHintsLabel, 0, 1);
        _beginnerHintsLayout.Dock = DockStyle.Fill;
        _beginnerHintsLayout.Name = "_beginnerHintsLayout";
        _beginnerHintsLayout.RowCount = 2;
        _beginnerHintsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, MainFormLayout.BottomTitleHeight));
        _beginnerHintsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        _beginnerHintsTitleLabel.Dock = DockStyle.Fill;
        _beginnerHintsTitleLabel.Font = new Font(Control.DefaultFont, FontStyle.Bold);
        _beginnerHintsTitleLabel.Name = "_beginnerHintsTitleLabel";
        _beginnerHintsTitleLabel.Text = MainFormTexts.BeginnerHintsTitle;
        _beginnerHintsTitleLabel.TextAlign = ContentAlignment.MiddleLeft;

        _beginnerHintsLabel.Dock = DockStyle.Fill;
        _beginnerHintsLabel.Name = "_beginnerHintsLabel";
        _beginnerHintsLabel.Text = MainFormTexts.BeginnerHintsText;
        _beginnerHintsLabel.TextAlign = ContentAlignment.TopLeft;

        _iconLegendPanel.BorderStyle = BorderStyle.FixedSingle;
        _iconLegendPanel.Controls.Add(_iconMeaningLayout);
        _iconLegendPanel.Dock = DockStyle.Fill;
        _iconLegendPanel.Location = new Point(3, 169);
        _iconLegendPanel.Name = "_iconLegendPanel";
        _iconLegendPanel.Padding = new Padding(8);
        _iconLegendPanel.Size = new Size(1148, MainFormLayout.IconMeaningHeight - 6);
        _iconLegendPanel.TabIndex = 2;

        _iconMeaningLayout.ColumnCount = 1;
        _iconMeaningLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _iconMeaningLayout.Controls.Add(_iconMeaningTitleLabel, 0, 0);
        _iconMeaningLayout.Controls.Add(_iconLegendFlow, 0, 1);
        _iconMeaningLayout.Dock = DockStyle.Fill;
        _iconMeaningLayout.Name = "_iconMeaningLayout";
        _iconMeaningLayout.RowCount = 2;
        _iconMeaningLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, MainFormLayout.BottomTitleHeight));
        _iconMeaningLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        _iconMeaningTitleLabel.Dock = DockStyle.Fill;
        _iconMeaningTitleLabel.Font = new Font(Control.DefaultFont, FontStyle.Bold);
        _iconMeaningTitleLabel.Name = "_iconMeaningTitleLabel";
        _iconMeaningTitleLabel.Text = MainFormTexts.IconMeaningTitle;
        _iconMeaningTitleLabel.TextAlign = ContentAlignment.MiddleLeft;

        _iconLegendFlow.AutoScroll = true;
        _iconLegendFlow.Dock = DockStyle.Fill;
        _iconLegendFlow.FlowDirection = FlowDirection.LeftToRight;
        _iconLegendFlow.Name = "_iconLegendFlow";
        _iconLegendFlow.WrapContents = true;

        AutoScaleMode = AutoScaleMode.Dpi;
        AllowDrop = true;
        ClientSize = new Size(MainFormLayout.InitialWidth, MainFormLayout.InitialHeight);
        Controls.Add(_rootLayout);
        Font = SystemFonts.MessageBoxFont;
        MinimumSize = new Size(MainFormLayout.MinimumWidth, MainFormLayout.MinimumHeight);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = MainFormTexts.WindowTitle;

        ((System.ComponentModel.ISupportInitialize)_itemGrid).EndInit();
        _iconMeaningLayout.ResumeLayout(false);
        _iconLegendPanel.ResumeLayout(false);
        _beginnerHintsLayout.ResumeLayout(false);
        _beginnerHintsPanel.ResumeLayout(false);
        _displayTogglePanel.ResumeLayout(false);
        _bottomPanel.ResumeLayout(false);
        _actionPanel.ResumeLayout(false);
        _groupPanel.ResumeLayout(false);
        _rootLayout.ResumeLayout(false);
        ResumeLayout(false);
    }

}
