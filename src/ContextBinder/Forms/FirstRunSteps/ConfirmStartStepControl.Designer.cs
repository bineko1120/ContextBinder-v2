namespace ContextBinder.Forms.FirstRunSteps;

public sealed partial class ConfirmStartStepControl
{
    private FlowLayoutPanel _rootPanel = null!;
    private Label _headingLabel = null!;
    private Label _descriptionLabel = null!;
    private GroupBox _startModeGroupBox = null!;
    private FlowLayoutPanel _startModeLinesPanel = null!;
    private GroupBox _storageGroupBox = null!;
    private FlowLayoutPanel _storageLinesPanel = null!;
    private GroupBox _mainSettingsGroupBox = null!;
    private FlowLayoutPanel _mainSettingsLinesPanel = null!;
    private GroupBox _createdItemsGroupBox = null!;
    private FlowLayoutPanel _createdItemsLinesPanel = null!;
    private GroupBox _savedItemsGroupBox = null!;
    private FlowLayoutPanel _savedItemsLinesPanel = null!;
    private GroupBox _notSavedItemsGroupBox = null!;
    private FlowLayoutPanel _notSavedItemsLinesPanel = null!;

    private void InitializeComponent()
    {
        _rootPanel = new FlowLayoutPanel();
        _headingLabel = new Label();
        _descriptionLabel = new Label();
        _startModeGroupBox = new GroupBox();
        _startModeLinesPanel = new FlowLayoutPanel();
        _storageGroupBox = new GroupBox();
        _storageLinesPanel = new FlowLayoutPanel();
        _mainSettingsGroupBox = new GroupBox();
        _mainSettingsLinesPanel = new FlowLayoutPanel();
        _createdItemsGroupBox = new GroupBox();
        _createdItemsLinesPanel = new FlowLayoutPanel();
        _savedItemsGroupBox = new GroupBox();
        _savedItemsLinesPanel = new FlowLayoutPanel();
        _notSavedItemsGroupBox = new GroupBox();
        _notSavedItemsLinesPanel = new FlowLayoutPanel();
        _rootPanel.SuspendLayout();
        _startModeGroupBox.SuspendLayout();
        _storageGroupBox.SuspendLayout();
        _mainSettingsGroupBox.SuspendLayout();
        _createdItemsGroupBox.SuspendLayout();
        _savedItemsGroupBox.SuspendLayout();
        _notSavedItemsGroupBox.SuspendLayout();
        SuspendLayout();

        _rootPanel.AutoScroll = true;
        _rootPanel.Controls.Add(_headingLabel);
        _rootPanel.Controls.Add(_descriptionLabel);
        _rootPanel.Controls.Add(_startModeGroupBox);
        _rootPanel.Controls.Add(_storageGroupBox);
        _rootPanel.Controls.Add(_mainSettingsGroupBox);
        _rootPanel.Controls.Add(_createdItemsGroupBox);
        _rootPanel.Controls.Add(_savedItemsGroupBox);
        _rootPanel.Controls.Add(_notSavedItemsGroupBox);
        _rootPanel.Dock = DockStyle.Fill;
        _rootPanel.FlowDirection = FlowDirection.TopDown;
        _rootPanel.Location = new Point(0, 0);
        _rootPanel.Name = "_rootPanel";
        _rootPanel.Padding = new Padding(0, 0, 8, 0);
        _rootPanel.Size = new Size(780, 580);
        _rootPanel.TabIndex = 0;
        _rootPanel.WrapContents = false;

        _headingLabel.Font = new Font("Yu Gothic UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
        _headingLabel.Name = "_headingLabel";
        _headingLabel.Size = new Size(740, 36);
        _headingLabel.TabIndex = 0;
        _headingLabel.Text = "確認して開始";
        _headingLabel.TextAlign = ContentAlignment.MiddleLeft;

        _descriptionLabel.AutoSize = true;
        _descriptionLabel.MaximumSize = new Size(740, 0);
        _descriptionLabel.Name = "_descriptionLabel";
        _descriptionLabel.Size = new Size(740, 40);
        _descriptionLabel.TabIndex = 1;
        _descriptionLabel.Text = "選んだ内容を確認してください。「開始」を押すと、登録内容と設定の保存先を作成してContextBinderを起動します。";

        _startModeGroupBox.Controls.Add(_startModeLinesPanel);
        _startModeGroupBox.Margin = new Padding(0, 8, 0, 4);
        _startModeGroupBox.Name = "_startModeGroupBox";
        _startModeGroupBox.Size = new Size(740, 76);
        _startModeGroupBox.TabIndex = 2;
        _startModeGroupBox.TabStop = false;
        _startModeGroupBox.Text = "始め方";

        _startModeLinesPanel.FlowDirection = FlowDirection.TopDown;
        _startModeLinesPanel.Location = new Point(12, 28);
        _startModeLinesPanel.Name = "_startModeLinesPanel";
        _startModeLinesPanel.Size = new Size(700, 36);
        _startModeLinesPanel.TabIndex = 0;
        _startModeLinesPanel.WrapContents = false;

        _storageGroupBox.Controls.Add(_storageLinesPanel);
        _storageGroupBox.Margin = new Padding(0, 8, 0, 4);
        _storageGroupBox.Name = "_storageGroupBox";
        _storageGroupBox.Size = new Size(740, 96);
        _storageGroupBox.TabIndex = 3;
        _storageGroupBox.TabStop = false;
        _storageGroupBox.Text = "保存場所";

        _storageLinesPanel.FlowDirection = FlowDirection.TopDown;
        _storageLinesPanel.Location = new Point(12, 28);
        _storageLinesPanel.Name = "_storageLinesPanel";
        _storageLinesPanel.Size = new Size(700, 54);
        _storageLinesPanel.TabIndex = 0;
        _storageLinesPanel.WrapContents = false;

        _mainSettingsGroupBox.Controls.Add(_mainSettingsLinesPanel);
        _mainSettingsGroupBox.Margin = new Padding(0, 8, 0, 4);
        _mainSettingsGroupBox.Name = "_mainSettingsGroupBox";
        _mainSettingsGroupBox.Size = new Size(740, 310);
        _mainSettingsGroupBox.TabIndex = 4;
        _mainSettingsGroupBox.TabStop = false;
        _mainSettingsGroupBox.Text = "主な設定";

        _mainSettingsLinesPanel.FlowDirection = FlowDirection.TopDown;
        _mainSettingsLinesPanel.Location = new Point(12, 28);
        _mainSettingsLinesPanel.Name = "_mainSettingsLinesPanel";
        _mainSettingsLinesPanel.Size = new Size(700, 268);
        _mainSettingsLinesPanel.TabIndex = 0;
        _mainSettingsLinesPanel.WrapContents = false;

        _createdItemsGroupBox.Controls.Add(_createdItemsLinesPanel);
        _createdItemsGroupBox.Margin = new Padding(0, 8, 0, 4);
        _createdItemsGroupBox.Name = "_createdItemsGroupBox";
        _createdItemsGroupBox.Size = new Size(740, 118);
        _createdItemsGroupBox.TabIndex = 5;
        _createdItemsGroupBox.TabStop = false;
        _createdItemsGroupBox.Text = "作成されるもの";

        _createdItemsLinesPanel.FlowDirection = FlowDirection.TopDown;
        _createdItemsLinesPanel.Location = new Point(12, 28);
        _createdItemsLinesPanel.Name = "_createdItemsLinesPanel";
        _createdItemsLinesPanel.Size = new Size(700, 76);
        _createdItemsLinesPanel.TabIndex = 0;
        _createdItemsLinesPanel.WrapContents = false;

        _savedItemsGroupBox.Controls.Add(_savedItemsLinesPanel);
        _savedItemsGroupBox.Margin = new Padding(0, 8, 0, 4);
        _savedItemsGroupBox.Name = "_savedItemsGroupBox";
        _savedItemsGroupBox.Size = new Size(740, 158);
        _savedItemsGroupBox.TabIndex = 6;
        _savedItemsGroupBox.TabStop = false;
        _savedItemsGroupBox.Text = "保存されるもの";

        _savedItemsLinesPanel.FlowDirection = FlowDirection.TopDown;
        _savedItemsLinesPanel.Location = new Point(12, 28);
        _savedItemsLinesPanel.Name = "_savedItemsLinesPanel";
        _savedItemsLinesPanel.Size = new Size(700, 116);
        _savedItemsLinesPanel.TabIndex = 0;
        _savedItemsLinesPanel.WrapContents = false;

        _notSavedItemsGroupBox.Controls.Add(_notSavedItemsLinesPanel);
        _notSavedItemsGroupBox.Margin = new Padding(0, 8, 0, 4);
        _notSavedItemsGroupBox.Name = "_notSavedItemsGroupBox";
        _notSavedItemsGroupBox.Size = new Size(740, 118);
        _notSavedItemsGroupBox.TabIndex = 7;
        _notSavedItemsGroupBox.TabStop = false;
        _notSavedItemsGroupBox.Text = "保存されないもの";

        _notSavedItemsLinesPanel.FlowDirection = FlowDirection.TopDown;
        _notSavedItemsLinesPanel.Location = new Point(12, 28);
        _notSavedItemsLinesPanel.Name = "_notSavedItemsLinesPanel";
        _notSavedItemsLinesPanel.Size = new Size(700, 76);
        _notSavedItemsLinesPanel.TabIndex = 0;
        _notSavedItemsLinesPanel.WrapContents = false;

        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(_rootPanel);
        Font = new Font("Yu Gothic UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
        Name = "ConfirmStartStepControl";
        Size = new Size(780, 580);
        _notSavedItemsGroupBox.ResumeLayout(false);
        _savedItemsGroupBox.ResumeLayout(false);
        _createdItemsGroupBox.ResumeLayout(false);
        _mainSettingsGroupBox.ResumeLayout(false);
        _storageGroupBox.ResumeLayout(false);
        _startModeGroupBox.ResumeLayout(false);
        _rootPanel.ResumeLayout(false);
        _rootPanel.PerformLayout();
        ResumeLayout(false);
    }
}
