using ContextBinder.Models;
using ContextBinder.Services;
using static ContextBinder.UiTexts.FirstRunSetup;
using FirstRunLayout = ContextBinder.UiLayoutSettings.FirstRun;

namespace ContextBinder.Forms;

public sealed partial class FirstRunSetupForm : Form
{
    private const int ContentWidth = FirstRunLayout.ContentWidth;
    private const int LastStepIndex = 3;

    private readonly StorageLocationService _storageLocationService;
    private readonly Label _stepLabel = new();
    private readonly Panel _contentPanel = new();
    private readonly Button _backButton = new();
    private readonly Button _nextButton = new();
    private readonly Button _cancelButton = new();

    private readonly RadioButton _recommendedSetupRadio = new();
    private readonly RadioButton _customSetupRadio = new();
    private readonly RadioButton _standardStorageRadio = new();
    private readonly RadioButton _portableStorageRadio = new();
    private readonly RadioButton _customStorageRadio = new();
    private readonly TextBox _customDirectoryTextBox = new();
    private readonly Button _browseButton = new();
    private readonly Label _storageDescriptionLabel = new();
    private readonly Label _storagePreviewLabel = new();

    private readonly CheckBox _editRecommendedSettingsCheckBox = new();
    private readonly ComboBox _typeDisplayModeComboBox = new();
    private readonly CheckBox _showBeginnerHintsCheckBox = new();
    private readonly CheckBox _showIconLegendCheckBox = new();
    private readonly CheckBox _showOperationStatusCheckBox = new();
    private readonly CheckBox _confirmTitleOnDropAddCheckBox = new();
    private readonly CheckBox _focusExistingItemOnDuplicateCheckBox = new();
    private readonly CheckBox _enableGroupDropModifierShortcutsCheckBox = new();
    private readonly CheckBox _confirmGroupDropCopyMoveCheckBox = new();
    private readonly CheckBox _enableItemDragReorderCheckBox = new();
    private readonly CheckBox _enableExternalFileDropOutCheckBox = new();
    private readonly CheckBox _enableExternalUrlTextDragOutCheckBox = new();
    private readonly CheckBox _enableExternalTemplateTextDragOutCheckBox = new();
    private readonly CheckBox _minimizeToTrayOnCloseCheckBox = new();
    private readonly CheckBox _enableContextMenuDetailsCheckBox = new();
    private readonly CheckBox _autoBackupEnabledCheckBox = new();
    private readonly NumericUpDown _maxBackupCountNumeric = new();
    private readonly CheckBox _confirmBeforeDeleteCheckBox = new();
    private readonly CheckBox _moveDeletedItemsToTrashCheckBox = new();
    private readonly CheckBox _searchTemplateBodyCheckBox = new();

    private readonly List<Control> _editableSettingsControls = [];
    private int _currentStep;

    public FirstRunSetupForm(StorageLocationService? storageLocationService = null)
    {
        _storageLocationService = storageLocationService ?? new StorageLocationService();

        Text = WindowTitle;
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.Sizable;
        MaximizeBox = true;
        MinimizeBox = false;
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(FirstRunLayout.InitialWidth, FirstRunLayout.InitialHeight);
        MinimumSize = new Size(FirstRunLayout.MinimumWidth, FirstRunLayout.MinimumHeight);
        Font = SystemFonts.MessageBoxFont;

        InitializeSetupControls();
        InitializeStorageControls();
        InitializeSettingsControls();
        BuildLayout();
        RenderCurrentStep();
    }

    public StorageMode SelectedStorageMode { get; private set; } = StorageMode.Standard;

    public string CustomStorageDirectory { get; private set; } = string.Empty;

    public AppSettings SelectedAppSettings { get; private set; } = AppSettingsFactory.CreateRecommended();

    private void InitializeSetupControls()
    {
        _recommendedSetupRadio.Text = RecommendedSetupLabel;
        _recommendedSetupRadio.Checked = true;
        _recommendedSetupRadio.CheckedChanged += (_, _) => RenderCurrentStep();

        _customSetupRadio.Text = CustomSetupLabel;
        _customSetupRadio.CheckedChanged += (_, _) => RenderCurrentStep();
    }

    private void InitializeStorageControls()
    {
        _standardStorageRadio.Text = StandardStorageLabel;
        _standardStorageRadio.Checked = true;
        _standardStorageRadio.CheckedChanged += (_, _) => UpdateStoragePreview();

        _portableStorageRadio.Text = PortableStorageLabel;
        _portableStorageRadio.CheckedChanged += (_, _) => UpdateStoragePreview();

        _customStorageRadio.Text = CustomStorageLabel;
        _customStorageRadio.CheckedChanged += (_, _) => UpdateStoragePreview();

        _customDirectoryTextBox.Width = FirstRunLayout.CustomDirectoryTextBoxWidth;
        _customDirectoryTextBox.TextChanged += (_, _) => UpdateStoragePreview();

        _browseButton.Text = BrowseButton;
        _browseButton.Width = FirstRunLayout.BrowseButtonWidth;
        _browseButton.Click += BrowseButton_Click;
    }

    private void InitializeSettingsControls()
    {
        _typeDisplayModeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        _typeDisplayModeComboBox.Width = 180;
        _typeDisplayModeComboBox.Items.AddRange(TypeDisplayModeItems);

        _maxBackupCountNumeric.Minimum = 1;
        _maxBackupCountNumeric.Maximum = 100;
        _maxBackupCountNumeric.Width = 80;

        LoadSettingsIntoControls(AppSettingsFactory.CreateRecommended());
        _editRecommendedSettingsCheckBox.Text = EditRecommendedSettings;
        _editRecommendedSettingsCheckBox.CheckedChanged += (_, _) => UpdateSettingsEditability();

        RegisterSettingControl(_typeDisplayModeComboBox);
        RegisterSettingControl(_showBeginnerHintsCheckBox, ShowBeginnerHintsLabel);
        RegisterSettingControl(_showIconLegendCheckBox, ShowIconMeaningLabel);
        RegisterSettingControl(_showOperationStatusCheckBox, ShowOperationStatusLabel);
        RegisterSettingControl(_confirmTitleOnDropAddCheckBox, ConfirmTitleOnDropAddLabel);
        RegisterSettingControl(_focusExistingItemOnDuplicateCheckBox, FocusExistingItemOnDuplicateLabel);
        RegisterSettingControl(_enableGroupDropModifierShortcutsCheckBox, EnableGroupDropModifierShortcutsLabel);
        RegisterSettingControl(_confirmGroupDropCopyMoveCheckBox, ConfirmGroupDropCopyMoveLabel);
        RegisterSettingControl(_enableItemDragReorderCheckBox, EnableItemDragReorderLabel);
        RegisterSettingControl(_enableExternalFileDropOutCheckBox, EnableExternalFileDropOutLabel);
        RegisterSettingControl(_enableExternalUrlTextDragOutCheckBox, EnableExternalUrlTextDragOutLabel);
        RegisterSettingControl(_enableExternalTemplateTextDragOutCheckBox, EnableExternalTemplateTextDragOutLabel);
        RegisterSettingControl(_minimizeToTrayOnCloseCheckBox, MinimizeToTrayOnCloseLabel);
        RegisterSettingControl(_enableContextMenuDetailsCheckBox, EnableContextMenuDetailsLabel);
        RegisterSettingControl(_autoBackupEnabledCheckBox, AutoBackupEnabledLabel);
        RegisterSettingControl(_maxBackupCountNumeric);
        RegisterSettingControl(_confirmBeforeDeleteCheckBox, ConfirmBeforeDeleteLabel);
        RegisterSettingControl(_moveDeletedItemsToTrashCheckBox, MoveDeletedItemsToTrashLabel);
        RegisterSettingControl(_searchTemplateBodyCheckBox, SearchTemplateBodyLabel);
    }

    private void RegisterSettingControl(Control control)
    {
        _editableSettingsControls.Add(control);
    }

    private void RegisterSettingControl(CheckBox checkBox, string text)
    {
        checkBox.Text = text;
        checkBox.Width = ContentWidth - 40;
        _editableSettingsControls.Add(checkBox);
    }

    private void BuildLayout()
    {
        TableLayoutPanel root = new()
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(FirstRunLayout.RootPadding),
            ColumnCount = 1,
            RowCount = 3
        };
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, FirstRunLayout.StepHeaderHeight));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, FirstRunLayout.FooterButtonHeight));

        _stepLabel.Dock = DockStyle.Fill;
        _stepLabel.TextAlign = ContentAlignment.MiddleLeft;
        _stepLabel.Font = new Font(Font, FontStyle.Bold);

        _contentPanel.Dock = DockStyle.Fill;
        _contentPanel.BorderStyle = BorderStyle.FixedSingle;
        _contentPanel.Padding = new Padding(FirstRunLayout.ContentPadding);

        TableLayoutPanel buttonPanel = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 4
        };
        buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, FirstRunLayout.WizardButtonWidth));
        buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, FirstRunLayout.WizardButtonWidth));
        buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, FirstRunLayout.WizardButtonWidth));

        _backButton.Text = BackButton;
        _backButton.Dock = DockStyle.Fill;
        _backButton.Click += (_, _) => MoveStep(-1);

        _nextButton.Dock = DockStyle.Fill;
        _nextButton.Click += NextButton_Click;

        _cancelButton.Text = ContextBinder.UiTexts.FirstRunSetup.CancelButton;
        _cancelButton.Dock = DockStyle.Fill;
        _cancelButton.DialogResult = DialogResult.Cancel;

        buttonPanel.Controls.Add(_backButton, 1, 0);
        buttonPanel.Controls.Add(_nextButton, 2, 0);
        buttonPanel.Controls.Add(_cancelButton, 3, 0);

        root.Controls.Add(_stepLabel, 0, 0);
        root.Controls.Add(_contentPanel, 0, 1);
        root.Controls.Add(buttonPanel, 0, 2);
        Controls.Add(root);

        AcceptButton = _nextButton;
        CancelButton = _cancelButton;
    }

    private void RenderCurrentStep()
    {
        _contentPanel.Controls.Clear();
        _stepLabel.Text = CreateStepText();
        _backButton.Enabled = _currentStep > 0;
        _nextButton.Text = _currentStep == LastStepIndex ? StartButton : NextButton;

        Control page = _currentStep switch
        {
            0 => BuildStartModePage(),
            1 => BuildStoragePage(),
            2 => BuildSettingsPage(),
            _ => BuildConfirmationPage()
        };

        page.Dock = DockStyle.Fill;
        _contentPanel.Controls.Add(page);
    }

    private string CreateStepText()
    {
        return string.Join("  →  ", StepLabels.Select((step, index) => index == _currentStep ? $"【{step}】" : step));
    }

    private Control BuildStartModePage()
    {
        FlowLayoutPanel panel = CreateVerticalPanel();
        panel.Controls.Add(CreateHeading(WelcomeHeading));
        panel.Controls.Add(CreateParagraph(WelcomeDescription));
        panel.Controls.Add(CreateOptionPanel(
            _recommendedSetupRadio,
            RecommendedSetupDescription,
            118));
        panel.Controls.Add(CreateOptionPanel(
            _customSetupRadio,
            CustomSetupDescription,
            100));

        return panel;
    }

    private Control BuildStoragePage()
    {
        FlowLayoutPanel panel = CreateVerticalPanel();
        panel.Controls.Add(CreateHeading(StorageHeading));
        panel.Controls.Add(CreateStorageOptionPanel(
            _standardStorageRadio,
            StandardStorageDescription));
        panel.Controls.Add(CreateStorageOptionPanel(
            _portableStorageRadio,
            PortableStorageDescription));
        Panel customPanel = CreateStorageOptionPanel(
            _customStorageRadio,
            CustomStorageDescription,
            150);
        FlowLayoutPanel customPathPanel = new()
        {
            Location = new Point(30, 112),
            Width = ContentWidth - 40,
            Height = 32,
            FlowDirection = FlowDirection.LeftToRight
        };
        customPathPanel.Controls.Add(_customDirectoryTextBox);
        customPathPanel.Controls.Add(_browseButton);
        customPanel.Controls.Add(customPathPanel);
        panel.Controls.Add(customPanel);

        _storageDescriptionLabel.Width = ContentWidth;
        _storageDescriptionLabel.Height = FirstRunLayout.StorageDescriptionHeight;
        _storageDescriptionLabel.Margin = new Padding(0, 8, 0, 0);
        _storagePreviewLabel.Width = ContentWidth;
        _storagePreviewLabel.Height = FirstRunLayout.StoragePreviewHeight;
        _storagePreviewLabel.BorderStyle = BorderStyle.FixedSingle;
        _storagePreviewLabel.Padding = new Padding(8);
        panel.Controls.Add(_storageDescriptionLabel);
        panel.Controls.Add(_storagePreviewLabel);
        UpdateStoragePreview();
        return panel;
    }

    private Control BuildSettingsPage()
    {
        FlowLayoutPanel panel = CreateVerticalPanel();
        bool customSetup = _customSetupRadio.Checked;

        panel.Controls.Add(CreateHeading(customSetup ? SettingsCustomHeading : SettingsRecommendedHeading));
        panel.Controls.Add(CreateParagraph(customSetup
            ? SettingsCustomDescription
            : SettingsRecommendedDescription));

        if (!customSetup)
        {
            _editRecommendedSettingsCheckBox.Width = ContentWidth;
            panel.Controls.Add(_editRecommendedSettingsCheckBox);
        }

        panel.Controls.Add(CreateSettingsCategoryPanel(
            DisplaySettingsCategory.Title,
            DisplaySettingsCategory.Description,
            new Control[]
            {
                CreateComboRow(TypeDisplayModeLabel, _typeDisplayModeComboBox, TypeDisplayModeDescription),
                CreateSettingRow(_showBeginnerHintsCheckBox, ShowBeginnerHintsDescription),
                CreateSettingRow(_showIconLegendCheckBox, ShowIconMeaningDescription),
                CreateSettingRow(_showOperationStatusCheckBox, ShowOperationStatusDescription)
            }));

        panel.Controls.Add(CreateSettingsCategoryPanel(
            DragDropSettingsCategory.Title,
            DragDropSettingsCategory.Description,
            new Control[]
            {
                CreateSettingRow(_confirmTitleOnDropAddCheckBox, ConfirmTitleOnDropAddDescription),
                CreateSettingRow(_focusExistingItemOnDuplicateCheckBox, FocusExistingItemOnDuplicateDescription),
                CreateSettingRow(_enableGroupDropModifierShortcutsCheckBox, EnableGroupDropModifierShortcutsDescription),
                CreateSettingRow(_confirmGroupDropCopyMoveCheckBox, ConfirmGroupDropCopyMoveDescription),
                CreateSettingRow(_enableItemDragReorderCheckBox, EnableItemDragReorderDescription),
                CreateSettingRow(_enableExternalFileDropOutCheckBox, EnableExternalFileDropOutDescription),
                CreateSettingRow(_enableExternalUrlTextDragOutCheckBox, EnableExternalUrlTextDragOutDescription),
                CreateSettingRow(_enableExternalTemplateTextDragOutCheckBox, EnableExternalTemplateTextDragOutDescription)
            }));

        panel.Controls.Add(CreateSettingsCategoryPanel(
            CloseBehaviorSettingsCategory.Title,
            CloseBehaviorSettingsCategory.Description,
            new Control[]
            {
                CreateSettingRow(_minimizeToTrayOnCloseCheckBox, MinimizeToTrayOnCloseDescription)
            }));

        panel.Controls.Add(CreateSettingsCategoryPanel(
            ContextMenuSettingsCategory.Title,
            ContextMenuSettingsCategory.Description,
            new Control[]
            {
                CreateSettingRow(_enableContextMenuDetailsCheckBox, EnableContextMenuDetailsDescription)
            }));

        panel.Controls.Add(CreateSettingsCategoryPanel(
            BackupDeleteSettingsCategory.Title,
            BackupDeleteSettingsCategory.Description,
            new Control[]
            {
                CreateSettingRow(_autoBackupEnabledCheckBox, AutoBackupEnabledDescription),
                CreateComboRow(MaxBackupCountLabel, _maxBackupCountNumeric, MaxBackupCountDescription),
                CreateSettingRow(_confirmBeforeDeleteCheckBox, ConfirmBeforeDeleteDescription),
                CreateSettingRow(_moveDeletedItemsToTrashCheckBox, MoveDeletedItemsToTrashDescription)
            }));

        panel.Controls.Add(CreateSettingsCategoryPanel(
            SearchSettingsCategory.Title,
            SearchSettingsCategory.Description,
            new Control[]
            {
                CreateSettingRow(_searchTemplateBodyCheckBox, SearchTemplateBodyDescription)
            }));

        UpdateSettingsEditability();
        return panel;
    }

    private Control BuildConfirmationPage()
    {
        FlowLayoutPanel panel = CreateVerticalPanel();
        panel.Controls.Add(CreateHeading(ConfirmationHeading));
        panel.Controls.Add(CreateParagraph(ConfirmationDescription));

        AppSettings settings = CreateSettingsFromControls();
        panel.Controls.Add(CreateConfirmationSection(StartModeConfirmationTitle, [
            _customSetupRadio.Checked ? CustomSetupSummary : RecommendedSetupSummary
        ]));
        panel.Controls.Add(CreateConfirmationSection(StorageConfirmationTitle, [
            GetStorageModeDisplayName(GetSelectedStorageMode()),
            CreateStoragePreviewText()
        ]));
        panel.Controls.Add(CreateConfirmationSection(MainSettingsConfirmationTitle, BuildMainSettingsSummary(settings)));
        panel.Controls.Add(CreateConfirmationSection(CreatedItemsConfirmationTitle, CreatedItems));
        panel.Controls.Add(CreateConfirmationSection(SavedItemsConfirmationTitle, SavedItems));
        panel.Controls.Add(CreateConfirmationSection(NotSavedItemsConfirmationTitle, NotSavedItems));
        return panel;
    }

    private static FlowLayoutPanel CreateVerticalPanel()
    {
        return new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoScroll = true,
            Padding = new Padding(0, 0, 8, 0)
        };
    }

    private static Label CreateHeading(string text)
    {
        return new Label
        {
            Text = text,
            Width = ContentWidth,
            Height = 36,
            Font = new Font(Control.DefaultFont, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleLeft
        };
    }

    private static Label CreateParagraph(string text)
    {
        return new Label
        {
            Text = text,
            Width = ContentWidth,
            AutoSize = true,
            MaximumSize = new Size(ContentWidth, 0),
            Margin = new Padding(0, 0, 0, 12)
        };
    }

    private static Panel CreateOptionPanel(RadioButton radioButton, string description, int height)
    {
        Panel panel = new()
        {
            Width = ContentWidth,
            Height = height,
            BorderStyle = BorderStyle.FixedSingle,
            Padding = new Padding(FirstRunLayout.SettingCategoryPadding),
            Margin = new Padding(0, 0, 0, 10)
        };

        radioButton.Location = new Point(10, 10);
        radioButton.Width = ContentWidth - 30;
        Label descriptionLabel = new()
        {
            Text = description,
            Location = new Point(30, 38),
            Width = ContentWidth - 50,
            Height = height - 48
        };
        panel.Controls.Add(radioButton);
        panel.Controls.Add(descriptionLabel);
        return panel;
    }

    private static Panel CreateStorageOptionPanel(RadioButton radioButton, string description, int height = 124)
    {
        return CreateOptionPanel(radioButton, description, height);
    }

    private static Panel CreateSettingsCategoryPanel(string title, string description, IReadOnlyCollection<Control> controls)
    {
        Panel sectionPanel = new()
        {
            Width = ContentWidth,
            BorderStyle = BorderStyle.FixedSingle,
            Padding = new Padding(10),
            Margin = new Padding(0, 6, 0, 12)
        };

        Label titleLabel = new()
        {
            Text = title,
            Location = new Point(10, 8),
            Width = ContentWidth - 24,
            Height = 24,
            Font = new Font(Control.DefaultFont, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleLeft
        };
        Label descriptionLabel = new()
        {
            Text = description,
            Location = new Point(10, 34),
            Width = ContentWidth - 24,
            Height = 36,
            ForeColor = SystemColors.GrayText
        };
        FlowLayoutPanel bodyPanel = new()
        {
            Location = new Point(10, 74),
            Width = ContentWidth - 24,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoSize = true
        };

        foreach (Control control in controls)
        {
            bodyPanel.Controls.Add(control);
        }

        int bodyHeight = controls.Sum(control => control.Height + control.Margin.Vertical);
        bodyPanel.Height = bodyHeight;
        sectionPanel.Height = 94 + bodyHeight;
        sectionPanel.Controls.Add(titleLabel);
        sectionPanel.Controls.Add(descriptionLabel);
        sectionPanel.Controls.Add(bodyPanel);
        return sectionPanel;
    }

    private static Panel CreateSettingRow(CheckBox checkBox, string description)
    {
        Panel panel = new()
        {
            Width = ContentWidth - 24,
            Height = FirstRunLayout.SettingRowHeight,
            Margin = new Padding(0, 0, 0, 4)
        };
        checkBox.Location = new Point(0, 0);
        checkBox.Width = ContentWidth - 40;

        Label descriptionLabel = new()
        {
            Text = description,
            Location = new Point(24, 28),
            Width = ContentWidth - 64,
            Height = 28,
            ForeColor = SystemColors.GrayText
        };
        panel.Controls.Add(checkBox);
        panel.Controls.Add(descriptionLabel);
        return panel;
    }

    private static Panel CreateComboRow(string labelText, Control control, string description)
    {
        Panel panel = new()
        {
            Width = ContentWidth - 24,
            Height = FirstRunLayout.ComboRowHeight,
            Margin = new Padding(0, 0, 0, 4)
        };
        Label label = new()
        {
            Text = labelText,
            Location = new Point(0, 4),
            Width = 170,
            Height = 24
        };
        control.Location = new Point(180, 0);
        Label descriptionLabel = new()
        {
            Text = description,
            Location = new Point(180, 28),
            Width = ContentWidth - 220,
            Height = 30,
            ForeColor = SystemColors.GrayText
        };
        panel.Controls.Add(label);
        panel.Controls.Add(control);
        panel.Controls.Add(descriptionLabel);
        return panel;
    }

    private static Panel CreateConfirmationSection(string title, IEnumerable<string> lines)
    {
        Panel sectionPanel = new()
        {
            Width = ContentWidth,
            BorderStyle = BorderStyle.FixedSingle,
            Padding = new Padding(10),
            Margin = new Padding(0, 0, 0, 10)
        };
        Label titleLabel = new()
        {
            Text = title,
            Location = new Point(10, 8),
            Width = ContentWidth - 24,
            Height = 24,
            Font = new Font(Control.DefaultFont, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleLeft
        };
        FlowLayoutPanel bodyPanel = new()
        {
            Location = new Point(10, 36),
            Width = ContentWidth - 24,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoSize = true
        };

        foreach (string line in lines)
        {
            Label lineLabel = new()
            {
                Text = line,
                AutoSize = true,
                MaximumSize = new Size(ContentWidth - 38, 0),
                Margin = new Padding(0, 0, 0, 4)
            };
            bodyPanel.Controls.Add(lineLabel);
        }

        int bodyHeight = bodyPanel.Controls.Cast<Control>().Sum(control => control.Height + control.Margin.Vertical);
        bodyPanel.Height = bodyHeight;
        sectionPanel.Height = Math.Max(FirstRunLayout.ConfirmationMinimumSectionHeight, 54 + bodyHeight);
        sectionPanel.Controls.Add(titleLabel);
        sectionPanel.Controls.Add(bodyPanel);
        return sectionPanel;
    }

    private void UpdateSettingsEditability()
    {
        bool canEdit = _customSetupRadio.Checked || _editRecommendedSettingsCheckBox.Checked;
        foreach (Control control in _editableSettingsControls)
        {
            control.Enabled = canEdit;
        }
    }

    private void UpdateStoragePreview()
    {
        _customDirectoryTextBox.Enabled = _customStorageRadio.Checked;
        _browseButton.Enabled = _customStorageRadio.Checked;

        SelectedStorageMode = GetSelectedStorageMode();
        CustomStorageDirectory = _customStorageRadio.Checked ? _customDirectoryTextBox.Text.Trim() : string.Empty;

        string description = SelectedStorageMode switch
        {
            StorageMode.Portable => PortableStorageCurrentDescription,
            StorageMode.Custom => CustomStorageCurrentDescription,
            _ => StandardStorageCurrentDescription
        };

        _storageDescriptionLabel.Text = description;
        _storagePreviewLabel.Text = $"{StoragePreviewPrefix}: {CreateStoragePreviewText()}";
    }

    private string CreateStoragePreviewText()
    {
        if (_customStorageRadio.Checked && string.IsNullOrWhiteSpace(_customDirectoryTextBox.Text))
        {
            return CustomStorageNotSelected;
        }

        try
        {
            StorageLocation location = _storageLocationService.CreateLocation(
                GetSelectedStorageMode(),
                _customStorageRadio.Checked ? _customDirectoryTextBox.Text.Trim() : null);
            return location.DataDirectory;
        }
        catch (InvalidOperationException ex)
        {
            return ex.Message;
        }
    }

    private void LoadSettingsIntoControls(AppSettings settings)
    {
        _typeDisplayModeComboBox.SelectedIndex = settings.TypeDisplayMode switch
        {
            TypeDisplayMode.IconOnly => 1,
            TypeDisplayMode.TextOnly => 2,
            TypeDisplayMode.Hidden => 3,
            _ => 0
        };
        _showBeginnerHintsCheckBox.Checked = settings.ShowBeginnerHints;
        _showIconLegendCheckBox.Checked = settings.ShowIconLegend;
        _showOperationStatusCheckBox.Checked = settings.ShowOperationStatus;
        _confirmTitleOnDropAddCheckBox.Checked = settings.ConfirmTitleOnDropAdd;
        _focusExistingItemOnDuplicateCheckBox.Checked = settings.FocusExistingItemOnDuplicate;
        _enableGroupDropModifierShortcutsCheckBox.Checked = settings.EnableGroupDropModifierShortcuts;
        _confirmGroupDropCopyMoveCheckBox.Checked = settings.ConfirmGroupDropCopyMove;
        _enableItemDragReorderCheckBox.Checked = settings.EnableItemDragReorder;
        _enableExternalFileDropOutCheckBox.Checked = settings.EnableExternalFileDropOut;
        _enableExternalUrlTextDragOutCheckBox.Checked = settings.EnableExternalUrlTextDragOut;
        _enableExternalTemplateTextDragOutCheckBox.Checked = settings.EnableExternalTemplateTextDragOut;
        _minimizeToTrayOnCloseCheckBox.Checked = settings.MinimizeToTrayOnClose;
        _enableContextMenuDetailsCheckBox.Checked = settings.EnableContextMenuDetails;
        _autoBackupEnabledCheckBox.Checked = settings.AutoBackupEnabled;
        _maxBackupCountNumeric.Value = Math.Clamp(settings.MaxBackupCount, 1, 100);
        _confirmBeforeDeleteCheckBox.Checked = settings.ConfirmBeforeDelete;
        _moveDeletedItemsToTrashCheckBox.Checked = settings.MoveDeletedItemsToTrash;
        _searchTemplateBodyCheckBox.Checked = settings.SearchTemplateBody;
    }

    private AppSettings CreateSettingsFromControls()
    {
        AppSettings settings = AppSettingsFactory.CreateRecommended();
        settings.TypeDisplayMode = _typeDisplayModeComboBox.SelectedIndex switch
        {
            1 => TypeDisplayMode.IconOnly,
            2 => TypeDisplayMode.TextOnly,
            3 => TypeDisplayMode.Hidden,
            _ => TypeDisplayMode.IconAndText
        };
        settings.ShowBeginnerHints = _showBeginnerHintsCheckBox.Checked;
        settings.ShowIconLegend = _showIconLegendCheckBox.Checked;
        settings.ShowOperationStatus = _showOperationStatusCheckBox.Checked;
        settings.ConfirmTitleOnDropAdd = _confirmTitleOnDropAddCheckBox.Checked;
        settings.FocusExistingItemOnDuplicate = _focusExistingItemOnDuplicateCheckBox.Checked;
        settings.EnableGroupDropModifierShortcuts = _enableGroupDropModifierShortcutsCheckBox.Checked;
        settings.ConfirmGroupDropCopyMove = _confirmGroupDropCopyMoveCheckBox.Checked;
        settings.EnableItemDragReorder = _enableItemDragReorderCheckBox.Checked;
        settings.EnableExternalFileDropOut = _enableExternalFileDropOutCheckBox.Checked;
        settings.EnableExternalUrlTextDragOut = _enableExternalUrlTextDragOutCheckBox.Checked;
        settings.EnableExternalTemplateTextDragOut = _enableExternalTemplateTextDragOutCheckBox.Checked;
        settings.MinimizeToTrayOnClose = _minimizeToTrayOnCloseCheckBox.Checked;
        settings.EnableContextMenuDetails = _enableContextMenuDetailsCheckBox.Checked;
        settings.AutoBackupEnabled = _autoBackupEnabledCheckBox.Checked;
        settings.MaxBackupCount = (int)_maxBackupCountNumeric.Value;
        settings.ConfirmBeforeDelete = _confirmBeforeDeleteCheckBox.Checked;
        settings.MoveDeletedItemsToTrash = _moveDeletedItemsToTrashCheckBox.Checked;
        settings.SearchTemplateBody = _searchTemplateBodyCheckBox.Checked;
        return settings;
    }

    private void BrowseButton_Click(object? sender, EventArgs e)
    {
        using FolderBrowserDialog dialog = new()
        {
            Description = CustomFolderDialogDescription,
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

    private void NextButton_Click(object? sender, EventArgs e)
    {
        if (!ValidateCurrentStep())
        {
            return;
        }

        if (_currentStep < LastStepIndex)
        {
            MoveStep(1);
            return;
        }

        SelectedStorageMode = GetSelectedStorageMode();
        CustomStorageDirectory = _customStorageRadio.Checked ? _customDirectoryTextBox.Text.Trim() : string.Empty;
        SelectedAppSettings = CreateSettingsFromControls();
        DialogResult = DialogResult.OK;
        Close();
    }

    private bool ValidateCurrentStep()
    {
        if (_currentStep == 1 && _customStorageRadio.Checked && string.IsNullOrWhiteSpace(_customDirectoryTextBox.Text))
        {
            MessageBox.Show(this, CustomStorageRequiredMessage, ValidationDialogTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (_currentStep == 2 && _typeDisplayModeComboBox.SelectedIndex == 3)
        {
            DialogResult result = MessageBox.Show(
                this,
                TypeDisplayHiddenWarningMessage,
                TypeDisplayHiddenWarningTitle,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);
            return result == DialogResult.Yes;
        }

        return true;
    }

    private void MoveStep(int delta)
    {
        _currentStep = Math.Clamp(_currentStep + delta, 0, LastStepIndex);
        RenderCurrentStep();
    }

    private StorageMode GetSelectedStorageMode()
    {
        if (_portableStorageRadio.Checked)
        {
            return StorageMode.Portable;
        }

        return _customStorageRadio.Checked ? StorageMode.Custom : StorageMode.Standard;
    }
}
