using static ContextBinder.UiTexts.FirstRunSetup;

namespace ContextBinder.Forms.FirstRunSteps;

public sealed partial class ConfirmStartStepControl
{
    private FlowLayoutPanel _rootPanel = null!;
    private Label _headingLabel = null!;
    private Label _descriptionLabel = null!;
    private FlowLayoutPanel _startModeLinesPanel = null!;
    private FlowLayoutPanel _storageLinesPanel = null!;
    private FlowLayoutPanel _mainSettingsLinesPanel = null!;
    private FlowLayoutPanel _createdItemsLinesPanel = null!;
    private FlowLayoutPanel _savedItemsLinesPanel = null!;
    private FlowLayoutPanel _notSavedItemsLinesPanel = null!;

    private void InitializeComponent()
    {
        _rootPanel = new FlowLayoutPanel();
        _headingLabel = new Label();
        _descriptionLabel = new Label();
        _rootPanel.SuspendLayout();
        SuspendLayout();

        _rootPanel.AutoScroll = true;
        _rootPanel.Controls.Add(_headingLabel);
        _rootPanel.Controls.Add(_descriptionLabel);
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
        _headingLabel.Text = ConfirmationHeading;
        _headingLabel.TextAlign = ContentAlignment.MiddleLeft;

        _descriptionLabel.AutoSize = true;
        _descriptionLabel.MaximumSize = new Size(740, 0);
        _descriptionLabel.Name = "_descriptionLabel";
        _descriptionLabel.Size = new Size(740, 38);
        _descriptionLabel.Text = ConfirmationDescription;

        _rootPanel.Controls.Add(CreateSectionGroup(StartModeConfirmationTitle, out _startModeLinesPanel, 76));
        _rootPanel.Controls.Add(CreateSectionGroup(StorageConfirmationTitle, out _storageLinesPanel, 96));
        _rootPanel.Controls.Add(CreateSectionGroup(MainSettingsConfirmationTitle, out _mainSettingsLinesPanel, 310));
        _rootPanel.Controls.Add(CreateSectionGroup(CreatedItemsConfirmationTitle, out _createdItemsLinesPanel, 118));
        _rootPanel.Controls.Add(CreateSectionGroup(SavedItemsConfirmationTitle, out _savedItemsLinesPanel, 158));
        _rootPanel.Controls.Add(CreateSectionGroup(NotSavedItemsConfirmationTitle, out _notSavedItemsLinesPanel, 118));

        AutoScaleMode = AutoScaleMode.Dpi;
        Controls.Add(_rootPanel);
        Name = "ConfirmStartStepControl";
        Size = new Size(780, 580);
        _rootPanel.ResumeLayout(false);
        _rootPanel.PerformLayout();
        ResumeLayout(false);
    }

    private static GroupBox CreateSectionGroup(string title, out FlowLayoutPanel linesPanel, int height)
    {
        linesPanel = new FlowLayoutPanel
        {
            AutoSize = false,
            FlowDirection = FlowDirection.TopDown,
            Location = new Point(12, 28),
            Name = $"{title}LinesPanel",
            Size = new Size(700, Math.Max(36, height - 42)),
            WrapContents = false
        };

        GroupBox groupBox = new()
        {
            Margin = new Padding(0, 8, 0, 4),
            Name = $"{title}GroupBox",
            Size = new Size(740, height),
            Text = title
        };
        groupBox.Controls.Add(linesPanel);
        return groupBox;
    }
}
