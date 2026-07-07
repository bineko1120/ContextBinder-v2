using static ContextBinder.UiTexts.FirstRunSetup;
using FirstRunLayout = ContextBinder.UiLayoutSettings.FirstRun;

namespace ContextBinder.Forms;

public sealed partial class FirstRunSetupForm
{
    private TableLayoutPanel _rootLayout = null!;
    private Label _stepLabel = null!;
    private Panel _contentPanel = null!;
    private TableLayoutPanel _footerButtonPanel = null!;
    private Button _backButton = null!;
    private Button _nextButton = null!;
    private Button _cancelButton = null!;

    private void InitializeComponent()
    {
        _rootLayout = new TableLayoutPanel();
        _stepLabel = new Label();
        _contentPanel = new Panel();
        _footerButtonPanel = new TableLayoutPanel();
        _backButton = new Button();
        _nextButton = new Button();
        _cancelButton = new Button();

        _rootLayout.SuspendLayout();
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
        _rootLayout.Padding = new Padding(FirstRunLayout.RootPadding);
        _rootLayout.RowCount = 3;
        _rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, FirstRunLayout.StepHeaderHeight));
        _rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        _rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, FirstRunLayout.FooterButtonHeight));
        _rootLayout.Size = new Size(FirstRunLayout.InitialWidth, FirstRunLayout.InitialHeight);
        _rootLayout.TabIndex = 0;

        _stepLabel.Dock = DockStyle.Fill;
        _stepLabel.Font = new Font(Control.DefaultFont, FontStyle.Bold);
        _stepLabel.Location = new Point(19, 16);
        _stepLabel.Name = "_stepLabel";
        _stepLabel.Size = new Size(FirstRunLayout.InitialWidth - (FirstRunLayout.RootPadding * 2), FirstRunLayout.StepHeaderHeight);
        _stepLabel.TabIndex = 0;
        _stepLabel.TextAlign = ContentAlignment.MiddleLeft;

        _contentPanel.BorderStyle = BorderStyle.FixedSingle;
        _contentPanel.Dock = DockStyle.Fill;
        _contentPanel.Location = new Point(19, 67);
        _contentPanel.Name = "_contentPanel";
        _contentPanel.Padding = new Padding(FirstRunLayout.ContentPadding);
        _contentPanel.Size = new Size(FirstRunLayout.InitialWidth - (FirstRunLayout.RootPadding * 2), FirstRunLayout.InitialHeight - 132);
        _contentPanel.TabIndex = 1;

        _footerButtonPanel.ColumnCount = 4;
        _footerButtonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _footerButtonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, FirstRunLayout.WizardButtonWidth));
        _footerButtonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, FirstRunLayout.WizardButtonWidth));
        _footerButtonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, FirstRunLayout.WizardButtonWidth));
        _footerButtonPanel.Controls.Add(_backButton, 1, 0);
        _footerButtonPanel.Controls.Add(_nextButton, 2, 0);
        _footerButtonPanel.Controls.Add(_cancelButton, 3, 0);
        _footerButtonPanel.Dock = DockStyle.Fill;
        _footerButtonPanel.Location = new Point(19, 715);
        _footerButtonPanel.Name = "_footerButtonPanel";
        _footerButtonPanel.RowCount = 1;
        _footerButtonPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        _footerButtonPanel.Size = new Size(FirstRunLayout.InitialWidth - (FirstRunLayout.RootPadding * 2), FirstRunLayout.FooterButtonHeight);
        _footerButtonPanel.TabIndex = 2;

        _backButton.Dock = DockStyle.Fill;
        _backButton.Name = "_backButton";
        _backButton.TabIndex = 0;
        _backButton.Text = BackButton;
        _backButton.UseVisualStyleBackColor = true;

        _nextButton.Dock = DockStyle.Fill;
        _nextButton.Name = "_nextButton";
        _nextButton.TabIndex = 1;
        _nextButton.Text = NextButton;
        _nextButton.UseVisualStyleBackColor = true;

        _cancelButton.DialogResult = DialogResult.Cancel;
        _cancelButton.Dock = DockStyle.Fill;
        _cancelButton.Name = "_cancelButton";
        _cancelButton.TabIndex = 2;
        _cancelButton.Text = ContextBinder.UiTexts.FirstRunSetup.CancelButton;
        _cancelButton.UseVisualStyleBackColor = true;

        AcceptButton = _nextButton;
        AutoScaleMode = AutoScaleMode.Dpi;
        CancelButton = _cancelButton;
        ClientSize = new Size(FirstRunLayout.InitialWidth, FirstRunLayout.InitialHeight);
        Controls.Add(_rootLayout);
        Font = SystemFonts.MessageBoxFont;
        FormBorderStyle = FormBorderStyle.Sizable;
        MaximizeBox = true;
        MinimizeBox = false;
        MinimumSize = new Size(FirstRunLayout.MinimumWidth, FirstRunLayout.MinimumHeight);
        Name = "FirstRunSetupForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = WindowTitle;

        _footerButtonPanel.ResumeLayout(false);
        _rootLayout.ResumeLayout(false);
        ResumeLayout(false);
    }
}
