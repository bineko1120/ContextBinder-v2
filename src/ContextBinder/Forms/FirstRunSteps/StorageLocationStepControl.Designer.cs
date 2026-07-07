using static ContextBinder.UiTexts.FirstRunSetup;

namespace ContextBinder.Forms.FirstRunSteps;

public sealed partial class StorageLocationStepControl
{
    private FlowLayoutPanel _rootPanel = null!;
    private Label _headingLabel = null!;
    private Panel _standardPanel = null!;
    private RadioButton _standardStorageRadio = null!;
    private Label _standardDescriptionLabel = null!;
    private Panel _portablePanel = null!;
    private RadioButton _portableStorageRadio = null!;
    private Label _portableDescriptionLabel = null!;
    private Panel _customPanel = null!;
    private RadioButton _customStorageRadio = null!;
    private Label _customDescriptionLabel = null!;
    private FlowLayoutPanel _customPathPanel = null!;
    private TextBox _customDirectoryTextBox = null!;
    private Button _browseButton = null!;
    private Label _storageDescriptionLabel = null!;
    private Label _storagePreviewLabel = null!;

    private void InitializeComponent()
    {
        _rootPanel = new FlowLayoutPanel();
        _headingLabel = new Label();
        _standardPanel = new Panel();
        _standardStorageRadio = new RadioButton();
        _standardDescriptionLabel = new Label();
        _portablePanel = new Panel();
        _portableStorageRadio = new RadioButton();
        _portableDescriptionLabel = new Label();
        _customPanel = new Panel();
        _customStorageRadio = new RadioButton();
        _customDescriptionLabel = new Label();
        _customPathPanel = new FlowLayoutPanel();
        _customDirectoryTextBox = new TextBox();
        _browseButton = new Button();
        _storageDescriptionLabel = new Label();
        _storagePreviewLabel = new Label();
        _rootPanel.SuspendLayout();
        _standardPanel.SuspendLayout();
        _portablePanel.SuspendLayout();
        _customPanel.SuspendLayout();
        _customPathPanel.SuspendLayout();
        SuspendLayout();

        _rootPanel.AutoScroll = true;
        _rootPanel.Controls.Add(_headingLabel);
        _rootPanel.Controls.Add(_standardPanel);
        _rootPanel.Controls.Add(_portablePanel);
        _rootPanel.Controls.Add(_customPanel);
        _rootPanel.Controls.Add(_storageDescriptionLabel);
        _rootPanel.Controls.Add(_storagePreviewLabel);
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
        _headingLabel.Text = StorageHeading;
        _headingLabel.TextAlign = ContentAlignment.MiddleLeft;

        _standardPanel.BorderStyle = BorderStyle.FixedSingle;
        _standardPanel.Controls.Add(_standardStorageRadio);
        _standardPanel.Controls.Add(_standardDescriptionLabel);
        _standardPanel.Margin = new Padding(0, 0, 0, 10);
        _standardPanel.Name = "_standardPanel";
        _standardPanel.Padding = new Padding(10);
        _standardPanel.Size = new Size(740, 118);

        _standardStorageRadio.Checked = true;
        _standardStorageRadio.Location = new Point(10, 10);
        _standardStorageRadio.Name = "_standardStorageRadio";
        _standardStorageRadio.Size = new Size(710, 24);
        _standardStorageRadio.TabIndex = 0;
        _standardStorageRadio.TabStop = true;
        _standardStorageRadio.Text = StandardStorageLabel;
        _standardStorageRadio.UseVisualStyleBackColor = true;
        _standardStorageRadio.CheckedChanged += StorageOption_CheckedChanged;

        _standardDescriptionLabel.Location = new Point(30, 38);
        _standardDescriptionLabel.Name = "_standardDescriptionLabel";
        _standardDescriptionLabel.Size = new Size(690, 70);
        _standardDescriptionLabel.Text = StandardStorageDescription;

        _portablePanel.BorderStyle = BorderStyle.FixedSingle;
        _portablePanel.Controls.Add(_portableStorageRadio);
        _portablePanel.Controls.Add(_portableDescriptionLabel);
        _portablePanel.Margin = new Padding(0, 0, 0, 10);
        _portablePanel.Name = "_portablePanel";
        _portablePanel.Padding = new Padding(10);
        _portablePanel.Size = new Size(740, 136);

        _portableStorageRadio.Location = new Point(10, 10);
        _portableStorageRadio.Name = "_portableStorageRadio";
        _portableStorageRadio.Size = new Size(710, 24);
        _portableStorageRadio.TabIndex = 1;
        _portableStorageRadio.Text = PortableStorageLabel;
        _portableStorageRadio.UseVisualStyleBackColor = true;
        _portableStorageRadio.CheckedChanged += StorageOption_CheckedChanged;

        _portableDescriptionLabel.Location = new Point(30, 38);
        _portableDescriptionLabel.Name = "_portableDescriptionLabel";
        _portableDescriptionLabel.Size = new Size(690, 88);
        _portableDescriptionLabel.Text = PortableStorageDescription;

        _customPanel.BorderStyle = BorderStyle.FixedSingle;
        _customPanel.Controls.Add(_customStorageRadio);
        _customPanel.Controls.Add(_customDescriptionLabel);
        _customPanel.Controls.Add(_customPathPanel);
        _customPanel.Margin = new Padding(0, 0, 0, 10);
        _customPanel.Name = "_customPanel";
        _customPanel.Padding = new Padding(10);
        _customPanel.Size = new Size(740, 164);

        _customStorageRadio.Location = new Point(10, 10);
        _customStorageRadio.Name = "_customStorageRadio";
        _customStorageRadio.Size = new Size(710, 24);
        _customStorageRadio.TabIndex = 2;
        _customStorageRadio.Text = CustomStorageLabel;
        _customStorageRadio.UseVisualStyleBackColor = true;
        _customStorageRadio.CheckedChanged += StorageOption_CheckedChanged;

        _customDescriptionLabel.Location = new Point(30, 38);
        _customDescriptionLabel.Name = "_customDescriptionLabel";
        _customDescriptionLabel.Size = new Size(690, 76);
        _customDescriptionLabel.Text = CustomStorageDescription;

        _customPathPanel.Controls.Add(_customDirectoryTextBox);
        _customPathPanel.Controls.Add(_browseButton);
        _customPathPanel.FlowDirection = FlowDirection.LeftToRight;
        _customPathPanel.Location = new Point(30, 122);
        _customPathPanel.Name = "_customPathPanel";
        _customPathPanel.Size = new Size(690, 32);
        _customPathPanel.TabIndex = 3;
        _customPathPanel.WrapContents = false;

        _customDirectoryTextBox.Name = "_customDirectoryTextBox";
        _customDirectoryTextBox.Size = new Size(560, 27);
        _customDirectoryTextBox.TabIndex = 0;
        _customDirectoryTextBox.TextChanged += CustomDirectoryTextBox_TextChanged;

        _browseButton.Name = "_browseButton";
        _browseButton.Size = new Size(92, 29);
        _browseButton.TabIndex = 1;
        _browseButton.Text = BrowseButton;
        _browseButton.UseVisualStyleBackColor = true;
        _browseButton.Click += BrowseButton_Click;

        _storageDescriptionLabel.Margin = new Padding(0, 8, 0, 0);
        _storageDescriptionLabel.Name = "_storageDescriptionLabel";
        _storageDescriptionLabel.Size = new Size(740, 54);
        _storageDescriptionLabel.Text = StandardStorageCurrentDescription;

        _storagePreviewLabel.BorderStyle = BorderStyle.FixedSingle;
        _storagePreviewLabel.Name = "_storagePreviewLabel";
        _storagePreviewLabel.Padding = new Padding(8);
        _storagePreviewLabel.Size = new Size(740, 48);
        _storagePreviewLabel.Text = $"{StoragePreviewPrefix}: %AppData%\\ContextBinder\\";

        AutoScaleMode = AutoScaleMode.Dpi;
        Controls.Add(_rootPanel);
        Name = "StorageLocationStepControl";
        Size = new Size(780, 580);
        _customPathPanel.ResumeLayout(false);
        _customPathPanel.PerformLayout();
        _customPanel.ResumeLayout(false);
        _portablePanel.ResumeLayout(false);
        _standardPanel.ResumeLayout(false);
        _rootPanel.ResumeLayout(false);
        ResumeLayout(false);
    }
}
