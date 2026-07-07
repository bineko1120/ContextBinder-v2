namespace ContextBinder.Forms;

public sealed partial class FirstRunSetupForm
{
    private TableLayoutPanel _rootLayout = null!;
    private Label _stepLabel = null!;
    private Panel _contentPanel = null!;
    private Label _designHintLabel = null!;
    private TableLayoutPanel _footerButtonPanel = null!;
    private Button _backButton = null!;
    private Button _nextButton = null!;
    private Button _cancelButton = null!;

    private void InitializeComponent()
    {
        _rootLayout = new TableLayoutPanel();
        _stepLabel = new Label();
        _contentPanel = new Panel();
        _designHintLabel = new Label();
        _footerButtonPanel = new TableLayoutPanel();
        _backButton = new Button();
        _nextButton = new Button();
        _cancelButton = new Button();
        _rootLayout.SuspendLayout();
        _contentPanel.SuspendLayout();
        _footerButtonPanel.SuspendLayout();
        SuspendLayout();

        _rootLayout.ColumnCount = 1;
        _rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _rootLayout.Controls.Add(_stepLabel, 0, 0);
        _rootLayout.Controls.Add(_contentPanel, 0, 1);
        _rootLayout.Controls.Add(_footerButtonPanel, 0, 2);
        _rootLayout.Dock = DockStyle.Fill;
        _rootLayout.Location = new Point(0, 0);
        _rootLayout.Name = "_rootLayout";
        _rootLayout.Padding = new Padding(16);
        _rootLayout.RowCount = 3;
        _rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
        _rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        _rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
        _rootLayout.Size = new Size(900, 760);
        _rootLayout.TabIndex = 0;

        _stepLabel.Dock = DockStyle.Fill;
        _stepLabel.Font = new Font("Yu Gothic UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
        _stepLabel.Location = new Point(19, 16);
        _stepLabel.Name = "_stepLabel";
        _stepLabel.Size = new Size(862, 48);
        _stepLabel.TabIndex = 0;
        _stepLabel.Text = "【1. 始め方】  →  2. 保存場所  →  3. 使いやすさ設定  →  4. 確認";
        _stepLabel.TextAlign = ContentAlignment.MiddleLeft;

        _contentPanel.BorderStyle = BorderStyle.FixedSingle;
        _contentPanel.Controls.Add(_designHintLabel);
        _contentPanel.Dock = DockStyle.Fill;
        _contentPanel.Location = new Point(19, 67);
        _contentPanel.Name = "_contentPanel";
        _contentPanel.Padding = new Padding(14);
        _contentPanel.Size = new Size(862, 626);
        _contentPanel.TabIndex = 1;

        _designHintLabel.Dock = DockStyle.Fill;
        _designHintLabel.ForeColor = SystemColors.GrayText;
        _designHintLabel.Location = new Point(14, 14);
        _designHintLabel.Name = "_designHintLabel";
        _designHintLabel.Size = new Size(832, 596);
        _designHintLabel.TabIndex = 0;
        _designHintLabel.Text = "ここに初回セットアップの各ステップを表示します。\r\n\r\nステップの中身は Forms/FirstRunSteps 配下の UserControl をDesignerで開いて編集してください。\r\n\r\n- StartModeStepControl\r\n- StorageLocationStepControl\r\n- UsabilitySettingsStepControl\r\n- ConfirmStartStepControl";
        _designHintLabel.TextAlign = ContentAlignment.MiddleCenter;

        _footerButtonPanel.ColumnCount = 4;
        _footerButtonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _footerButtonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 96F));
        _footerButtonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 96F));
        _footerButtonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 96F));
        _footerButtonPanel.Controls.Add(_backButton, 1, 0);
        _footerButtonPanel.Controls.Add(_nextButton, 2, 0);
        _footerButtonPanel.Controls.Add(_cancelButton, 3, 0);
        _footerButtonPanel.Dock = DockStyle.Fill;
        _footerButtonPanel.Location = new Point(19, 699);
        _footerButtonPanel.Name = "_footerButtonPanel";
        _footerButtonPanel.RowCount = 1;
        _footerButtonPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        _footerButtonPanel.Size = new Size(862, 42);
        _footerButtonPanel.TabIndex = 2;

        _backButton.Dock = DockStyle.Fill;
        _backButton.Location = new Point(577, 3);
        _backButton.Name = "_backButton";
        _backButton.Size = new Size(90, 36);
        _backButton.TabIndex = 0;
        _backButton.Text = "戻る";
        _backButton.UseVisualStyleBackColor = true;

        _nextButton.Dock = DockStyle.Fill;
        _nextButton.Location = new Point(673, 3);
        _nextButton.Name = "_nextButton";
        _nextButton.Size = new Size(90, 36);
        _nextButton.TabIndex = 1;
        _nextButton.Text = "次へ";
        _nextButton.UseVisualStyleBackColor = true;

        _cancelButton.DialogResult = DialogResult.Cancel;
        _cancelButton.Dock = DockStyle.Fill;
        _cancelButton.Location = new Point(769, 3);
        _cancelButton.Name = "_cancelButton";
        _cancelButton.Size = new Size(90, 36);
        _cancelButton.TabIndex = 2;
        _cancelButton.Text = "キャンセル";
        _cancelButton.UseVisualStyleBackColor = true;

        AcceptButton = _nextButton;
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = _cancelButton;
        ClientSize = new Size(900, 760);
        Controls.Add(_rootLayout);
        Font = new Font("Yu Gothic UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
        FormBorderStyle = FormBorderStyle.Sizable;
        MaximizeBox = true;
        MinimizeBox = false;
        MinimumSize = new Size(860, 720);
        Name = "FirstRunSetupForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "ContextBinder 初回セットアップ";

        _footerButtonPanel.ResumeLayout(false);
        _contentPanel.ResumeLayout(false);
        _rootLayout.ResumeLayout(false);
        ResumeLayout(false);
    }
}
