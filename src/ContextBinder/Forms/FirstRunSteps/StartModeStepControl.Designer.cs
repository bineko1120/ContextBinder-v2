namespace ContextBinder.Forms.FirstRunSteps;

public sealed partial class StartModeStepControl
{
    private FlowLayoutPanel _rootPanel = null!;
    private Label _headingLabel = null!;
    private Label _descriptionLabel = null!;
    private Panel _recommendedPanel = null!;
    private RadioButton _recommendedSetupRadio = null!;
    private Label _recommendedDescriptionLabel = null!;
    private Panel _customPanel = null!;
    private RadioButton _customSetupRadio = null!;
    private Label _customDescriptionLabel = null!;

    private void InitializeComponent()
    {
        _rootPanel = new FlowLayoutPanel();
        _headingLabel = new Label();
        _descriptionLabel = new Label();
        _recommendedPanel = new Panel();
        _recommendedSetupRadio = new RadioButton();
        _recommendedDescriptionLabel = new Label();
        _customPanel = new Panel();
        _customSetupRadio = new RadioButton();
        _customDescriptionLabel = new Label();
        _rootPanel.SuspendLayout();
        _recommendedPanel.SuspendLayout();
        _customPanel.SuspendLayout();
        SuspendLayout();

        _rootPanel.AutoScroll = true;
        _rootPanel.Controls.Add(_headingLabel);
        _rootPanel.Controls.Add(_descriptionLabel);
        _rootPanel.Controls.Add(_recommendedPanel);
        _rootPanel.Controls.Add(_customPanel);
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
        _headingLabel.Text = "ContextBinderへようこそ";
        _headingLabel.TextAlign = ContentAlignment.MiddleLeft;

        _descriptionLabel.AutoSize = true;
        _descriptionLabel.MaximumSize = new Size(740, 0);
        _descriptionLabel.Name = "_descriptionLabel";
        _descriptionLabel.Size = new Size(740, 80);
        _descriptionLabel.TabIndex = 1;
        _descriptionLabel.Text = "このツールは、ファイル・フォルダ・URL・テンプレート文を\r\nグループごとにまとめて、すぐ開く/コピーできるツールです。\r\n\r\nまずは保存場所と使いやすさ設定を選びます。";

        _recommendedPanel.BorderStyle = BorderStyle.FixedSingle;
        _recommendedPanel.Controls.Add(_recommendedSetupRadio);
        _recommendedPanel.Controls.Add(_recommendedDescriptionLabel);
        _recommendedPanel.Margin = new Padding(0, 0, 0, 10);
        _recommendedPanel.Name = "_recommendedPanel";
        _recommendedPanel.Padding = new Padding(10);
        _recommendedPanel.Size = new Size(740, 118);
        _recommendedPanel.TabIndex = 2;

        _recommendedSetupRadio.Checked = true;
        _recommendedSetupRadio.Location = new Point(10, 10);
        _recommendedSetupRadio.Name = "_recommendedSetupRadio";
        _recommendedSetupRadio.Size = new Size(710, 24);
        _recommendedSetupRadio.TabIndex = 0;
        _recommendedSetupRadio.TabStop = true;
        _recommendedSetupRadio.Text = "初心者おすすめ設定で始める";
        _recommendedSetupRadio.UseVisualStyleBackColor = true;
        _recommendedSetupRadio.CheckedChanged += SetupRadio_CheckedChanged;

        _recommendedDescriptionLabel.Location = new Point(30, 38);
        _recommendedDescriptionLabel.Name = "_recommendedDescriptionLabel";
        _recommendedDescriptionLabel.Size = new Size(690, 70);
        _recommendedDescriptionLabel.TabIndex = 1;
        _recommendedDescriptionLabel.Text = "迷った場合はこちらを選んでください。\r\n見やすさと安全性を優先した設定で始めます。\r\n次の画面で保存場所を選べます。\r\nおすすめ設定の内容は後で確認できます。";

        _customPanel.BorderStyle = BorderStyle.FixedSingle;
        _customPanel.Controls.Add(_customSetupRadio);
        _customPanel.Controls.Add(_customDescriptionLabel);
        _customPanel.Margin = new Padding(0, 0, 0, 10);
        _customPanel.Name = "_customPanel";
        _customPanel.Padding = new Padding(10);
        _customPanel.Size = new Size(740, 110);
        _customPanel.TabIndex = 3;

        _customSetupRadio.Location = new Point(10, 10);
        _customSetupRadio.Name = "_customSetupRadio";
        _customSetupRadio.Size = new Size(710, 24);
        _customSetupRadio.TabIndex = 0;
        _customSetupRadio.Text = "カスタム設定を選ぶ";
        _customSetupRadio.UseVisualStyleBackColor = true;
        _customSetupRadio.CheckedChanged += SetupRadio_CheckedChanged;

        _customDescriptionLabel.Location = new Point(30, 38);
        _customDescriptionLabel.Name = "_customDescriptionLabel";
        _customDescriptionLabel.Size = new Size(690, 62);
        _customDescriptionLabel.TabIndex = 1;
        _customDescriptionLabel.Text = "表示、ドラッグ＆ドロップ、削除確認、バックアップなどを自分で選びます。\r\nある程度使い方を決めたい人向けです。\r\n保存場所も次の画面で選べます。";

        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(_rootPanel);
        Font = new Font("Yu Gothic UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
        Name = "StartModeStepControl";
        Size = new Size(780, 580);
        _customPanel.ResumeLayout(false);
        _recommendedPanel.ResumeLayout(false);
        _rootPanel.ResumeLayout(false);
        _rootPanel.PerformLayout();
        ResumeLayout(false);
    }
}
