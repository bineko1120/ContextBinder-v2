using static ContextBinder.UiTexts.FirstRunSetup;

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

        _headingLabel.Font = new Font(Control.DefaultFont, FontStyle.Bold);
        _headingLabel.Name = "_headingLabel";
        _headingLabel.Size = new Size(740, 36);
        _headingLabel.Text = WelcomeHeading;
        _headingLabel.TextAlign = ContentAlignment.MiddleLeft;

        _descriptionLabel.AutoSize = true;
        _descriptionLabel.MaximumSize = new Size(740, 0);
        _descriptionLabel.Name = "_descriptionLabel";
        _descriptionLabel.Size = new Size(740, 54);
        _descriptionLabel.Text = WelcomeDescription;

        _recommendedPanel.BorderStyle = BorderStyle.FixedSingle;
        _recommendedPanel.Controls.Add(_recommendedSetupRadio);
        _recommendedPanel.Controls.Add(_recommendedDescriptionLabel);
        _recommendedPanel.Margin = new Padding(0, 0, 0, 10);
        _recommendedPanel.Name = "_recommendedPanel";
        _recommendedPanel.Padding = new Padding(10);
        _recommendedPanel.Size = new Size(740, 118);

        _recommendedSetupRadio.Checked = true;
        _recommendedSetupRadio.Location = new Point(10, 10);
        _recommendedSetupRadio.Name = "_recommendedSetupRadio";
        _recommendedSetupRadio.Size = new Size(710, 24);
        _recommendedSetupRadio.TabIndex = 0;
        _recommendedSetupRadio.TabStop = true;
        _recommendedSetupRadio.Text = RecommendedSetupLabel;
        _recommendedSetupRadio.UseVisualStyleBackColor = true;
        _recommendedSetupRadio.CheckedChanged += SetupRadio_CheckedChanged;

        _recommendedDescriptionLabel.Location = new Point(30, 38);
        _recommendedDescriptionLabel.Name = "_recommendedDescriptionLabel";
        _recommendedDescriptionLabel.Size = new Size(690, 70);
        _recommendedDescriptionLabel.Text = RecommendedSetupDescription;

        _customPanel.BorderStyle = BorderStyle.FixedSingle;
        _customPanel.Controls.Add(_customSetupRadio);
        _customPanel.Controls.Add(_customDescriptionLabel);
        _customPanel.Margin = new Padding(0, 0, 0, 10);
        _customPanel.Name = "_customPanel";
        _customPanel.Padding = new Padding(10);
        _customPanel.Size = new Size(740, 110);

        _customSetupRadio.Location = new Point(10, 10);
        _customSetupRadio.Name = "_customSetupRadio";
        _customSetupRadio.Size = new Size(710, 24);
        _customSetupRadio.TabIndex = 1;
        _customSetupRadio.Text = CustomSetupLabel;
        _customSetupRadio.UseVisualStyleBackColor = true;
        _customSetupRadio.CheckedChanged += SetupRadio_CheckedChanged;

        _customDescriptionLabel.Location = new Point(30, 38);
        _customDescriptionLabel.Name = "_customDescriptionLabel";
        _customDescriptionLabel.Size = new Size(690, 62);
        _customDescriptionLabel.Text = CustomSetupDescription;

        AutoScaleMode = AutoScaleMode.Dpi;
        Controls.Add(_rootPanel);
        Name = "StartModeStepControl";
        Size = new Size(780, 580);
        _customPanel.ResumeLayout(false);
        _recommendedPanel.ResumeLayout(false);
        _rootPanel.ResumeLayout(false);
        _rootPanel.PerformLayout();
        ResumeLayout(false);
    }
}
