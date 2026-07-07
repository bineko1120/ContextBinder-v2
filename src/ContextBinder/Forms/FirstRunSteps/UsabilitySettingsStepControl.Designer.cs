using static ContextBinder.UiTexts.FirstRunSetup;

namespace ContextBinder.Forms.FirstRunSteps;

public sealed partial class UsabilitySettingsStepControl
{
    private FlowLayoutPanel _rootPanel = null!;
    private Label _headingLabel = null!;
    private Label _descriptionLabel = null!;
    private CheckBox _editRecommendedSettingsCheckBox = null!;
    private ComboBox _typeDisplayModeComboBox = null!;
    private CheckBox _showBeginnerHintsCheckBox = null!;
    private CheckBox _showIconLegendCheckBox = null!;
    private CheckBox _showOperationStatusCheckBox = null!;
    private CheckBox _confirmTitleOnDropAddCheckBox = null!;
    private CheckBox _focusExistingItemOnDuplicateCheckBox = null!;
    private CheckBox _enableGroupDropModifierShortcutsCheckBox = null!;
    private CheckBox _confirmGroupDropCopyMoveCheckBox = null!;
    private CheckBox _enableItemDragReorderCheckBox = null!;
    private CheckBox _enableExternalFileDropOutCheckBox = null!;
    private CheckBox _enableExternalUrlTextDragOutCheckBox = null!;
    private CheckBox _enableExternalTemplateTextDragOutCheckBox = null!;
    private CheckBox _minimizeToTrayOnCloseCheckBox = null!;
    private CheckBox _enableContextMenuDetailsCheckBox = null!;
    private CheckBox _autoBackupEnabledCheckBox = null!;
    private NumericUpDown _maxBackupCountNumeric = null!;
    private CheckBox _confirmBeforeDeleteCheckBox = null!;
    private CheckBox _moveDeletedItemsToTrashCheckBox = null!;
    private CheckBox _searchTemplateBodyCheckBox = null!;

    private void InitializeComponent()
    {
        _rootPanel = new FlowLayoutPanel();
        _headingLabel = new Label();
        _descriptionLabel = new Label();
        _editRecommendedSettingsCheckBox = new CheckBox();
        _typeDisplayModeComboBox = new ComboBox();
        _showBeginnerHintsCheckBox = new CheckBox();
        _showIconLegendCheckBox = new CheckBox();
        _showOperationStatusCheckBox = new CheckBox();
        _confirmTitleOnDropAddCheckBox = new CheckBox();
        _focusExistingItemOnDuplicateCheckBox = new CheckBox();
        _enableGroupDropModifierShortcutsCheckBox = new CheckBox();
        _confirmGroupDropCopyMoveCheckBox = new CheckBox();
        _enableItemDragReorderCheckBox = new CheckBox();
        _enableExternalFileDropOutCheckBox = new CheckBox();
        _enableExternalUrlTextDragOutCheckBox = new CheckBox();
        _enableExternalTemplateTextDragOutCheckBox = new CheckBox();
        _minimizeToTrayOnCloseCheckBox = new CheckBox();
        _enableContextMenuDetailsCheckBox = new CheckBox();
        _autoBackupEnabledCheckBox = new CheckBox();
        _maxBackupCountNumeric = new NumericUpDown();
        _confirmBeforeDeleteCheckBox = new CheckBox();
        _moveDeletedItemsToTrashCheckBox = new CheckBox();
        _searchTemplateBodyCheckBox = new CheckBox();
        ((System.ComponentModel.ISupportInitialize)_maxBackupCountNumeric).BeginInit();
        _rootPanel.SuspendLayout();
        SuspendLayout();

        _rootPanel.AutoScroll = true;
        _rootPanel.Controls.Add(_headingLabel);
        _rootPanel.Controls.Add(_descriptionLabel);
        _rootPanel.Controls.Add(_editRecommendedSettingsCheckBox);
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
        _headingLabel.Text = SettingsRecommendedHeading;
        _headingLabel.TextAlign = ContentAlignment.MiddleLeft;

        _descriptionLabel.AutoSize = true;
        _descriptionLabel.MaximumSize = new Size(740, 0);
        _descriptionLabel.Name = "_descriptionLabel";
        _descriptionLabel.Size = new Size(740, 38);
        _descriptionLabel.Text = SettingsRecommendedDescription;

        _editRecommendedSettingsCheckBox.AutoSize = true;
        _editRecommendedSettingsCheckBox.Name = "_editRecommendedSettingsCheckBox";
        _editRecommendedSettingsCheckBox.Size = new Size(220, 24);
        _editRecommendedSettingsCheckBox.Text = EditRecommendedSettings;
        _editRecommendedSettingsCheckBox.UseVisualStyleBackColor = true;
        _editRecommendedSettingsCheckBox.CheckedChanged += EditRecommendedSettingsCheckBox_CheckedChanged;

        _typeDisplayModeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        _typeDisplayModeComboBox.Items.AddRange(TypeDisplayModeItems);
        _typeDisplayModeComboBox.Name = "_typeDisplayModeComboBox";
        _typeDisplayModeComboBox.Size = new Size(180, 28);

        _maxBackupCountNumeric.Minimum = 1;
        _maxBackupCountNumeric.Maximum = 100;
        _maxBackupCountNumeric.Name = "_maxBackupCountNumeric";
        _maxBackupCountNumeric.Size = new Size(80, 27);
        _maxBackupCountNumeric.Value = 20;

        _rootPanel.Controls.Add(CreateDisplaySettingsGroup());
        _rootPanel.Controls.Add(CreateDragDropSettingsGroup());
        _rootPanel.Controls.Add(CreateCloseBehaviorSettingsGroup());
        _rootPanel.Controls.Add(CreateContextMenuSettingsGroup());
        _rootPanel.Controls.Add(CreateBackupDeleteSettingsGroup());
        _rootPanel.Controls.Add(CreateSearchSettingsGroup());

        AutoScaleMode = AutoScaleMode.Dpi;
        Controls.Add(_rootPanel);
        Name = "UsabilitySettingsStepControl";
        Size = new Size(780, 580);
        ((System.ComponentModel.ISupportInitialize)_maxBackupCountNumeric).EndInit();
        _rootPanel.ResumeLayout(false);
        _rootPanel.PerformLayout();
        ResumeLayout(false);
    }

    private GroupBox CreateDisplaySettingsGroup()
    {
        FlowLayoutPanel bodyPanel = CreateCategoryBody();
        bodyPanel.Controls.Add(CreateComboRow(TypeDisplayModeLabel, _typeDisplayModeComboBox, TypeDisplayModeDescription));
        bodyPanel.Controls.Add(CreateCheckRow(_showBeginnerHintsCheckBox, ShowBeginnerHintsLabel, ShowBeginnerHintsDescription));
        bodyPanel.Controls.Add(CreateCheckRow(_showIconLegendCheckBox, ShowIconMeaningLabel, ShowIconMeaningDescription));
        bodyPanel.Controls.Add(CreateCheckRow(_showOperationStatusCheckBox, ShowOperationStatusLabel, ShowOperationStatusDescription));
        return CreateCategoryGroup(DisplaySettingsCategory.Title, DisplaySettingsCategory.Description, bodyPanel, 290);
    }

    private GroupBox CreateDragDropSettingsGroup()
    {
        FlowLayoutPanel bodyPanel = CreateCategoryBody();
        bodyPanel.Controls.Add(CreateCheckRow(_confirmTitleOnDropAddCheckBox, ConfirmTitleOnDropAddLabel, ConfirmTitleOnDropAddDescription));
        bodyPanel.Controls.Add(CreateCheckRow(_focusExistingItemOnDuplicateCheckBox, FocusExistingItemOnDuplicateLabel, FocusExistingItemOnDuplicateDescription));
        bodyPanel.Controls.Add(CreateCheckRow(_enableGroupDropModifierShortcutsCheckBox, EnableGroupDropModifierShortcutsLabel, EnableGroupDropModifierShortcutsDescription));
        bodyPanel.Controls.Add(CreateCheckRow(_confirmGroupDropCopyMoveCheckBox, ConfirmGroupDropCopyMoveLabel, ConfirmGroupDropCopyMoveDescription));
        bodyPanel.Controls.Add(CreateCheckRow(_enableItemDragReorderCheckBox, EnableItemDragReorderLabel, EnableItemDragReorderDescription));
        bodyPanel.Controls.Add(CreateCheckRow(_enableExternalFileDropOutCheckBox, EnableExternalFileDropOutLabel, EnableExternalFileDropOutDescription));
        bodyPanel.Controls.Add(CreateCheckRow(_enableExternalUrlTextDragOutCheckBox, EnableExternalUrlTextDragOutLabel, EnableExternalUrlTextDragOutDescription));
        bodyPanel.Controls.Add(CreateCheckRow(_enableExternalTemplateTextDragOutCheckBox, EnableExternalTemplateTextDragOutLabel, EnableExternalTemplateTextDragOutDescription));
        return CreateCategoryGroup(DragDropSettingsCategory.Title, DragDropSettingsCategory.Description, bodyPanel, 520);
    }

    private GroupBox CreateCloseBehaviorSettingsGroup()
    {
        FlowLayoutPanel bodyPanel = CreateCategoryBody();
        bodyPanel.Controls.Add(CreateCheckRow(_minimizeToTrayOnCloseCheckBox, MinimizeToTrayOnCloseLabel, MinimizeToTrayOnCloseDescription));
        return CreateCategoryGroup(CloseBehaviorSettingsCategory.Title, CloseBehaviorSettingsCategory.Description, bodyPanel, 126);
    }

    private GroupBox CreateContextMenuSettingsGroup()
    {
        FlowLayoutPanel bodyPanel = CreateCategoryBody();
        bodyPanel.Controls.Add(CreateCheckRow(_enableContextMenuDetailsCheckBox, EnableContextMenuDetailsLabel, EnableContextMenuDetailsDescription));
        return CreateCategoryGroup(ContextMenuSettingsCategory.Title, ContextMenuSettingsCategory.Description, bodyPanel, 126);
    }

    private GroupBox CreateBackupDeleteSettingsGroup()
    {
        FlowLayoutPanel bodyPanel = CreateCategoryBody();
        bodyPanel.Controls.Add(CreateCheckRow(_autoBackupEnabledCheckBox, AutoBackupEnabledLabel, AutoBackupEnabledDescription));
        bodyPanel.Controls.Add(CreateComboRow(MaxBackupCountLabel, _maxBackupCountNumeric, MaxBackupCountDescription));
        bodyPanel.Controls.Add(CreateCheckRow(_confirmBeforeDeleteCheckBox, ConfirmBeforeDeleteLabel, ConfirmBeforeDeleteDescription));
        bodyPanel.Controls.Add(CreateCheckRow(_moveDeletedItemsToTrashCheckBox, MoveDeletedItemsToTrashLabel, MoveDeletedItemsToTrashDescription));
        return CreateCategoryGroup(BackupDeleteSettingsCategory.Title, BackupDeleteSettingsCategory.Description, bodyPanel, 290);
    }

    private GroupBox CreateSearchSettingsGroup()
    {
        FlowLayoutPanel bodyPanel = CreateCategoryBody();
        bodyPanel.Controls.Add(CreateCheckRow(_searchTemplateBodyCheckBox, SearchTemplateBodyLabel, SearchTemplateBodyDescription));
        return CreateCategoryGroup(SearchSettingsCategory.Title, SearchSettingsCategory.Description, bodyPanel, 126);
    }

    private static GroupBox CreateCategoryGroup(string title, string description, FlowLayoutPanel bodyPanel, int height)
    {
        Label descriptionLabel = new()
        {
            Location = new Point(12, 26),
            Name = $"{title}DescriptionLabel",
            Size = new Size(700, 38),
            Text = description,
            ForeColor = SystemColors.GrayText
        };

        GroupBox groupBox = new()
        {
            Margin = new Padding(0, 8, 0, 10),
            Name = $"{title}GroupBox",
            Size = new Size(740, height),
            Text = title
        };

        bodyPanel.Location = new Point(12, 66);
        groupBox.Controls.Add(descriptionLabel);
        groupBox.Controls.Add(bodyPanel);
        return groupBox;
    }

    private static FlowLayoutPanel CreateCategoryBody()
    {
        return new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.TopDown,
            Name = "categoryBodyPanel",
            Size = new Size(710, 430),
            WrapContents = false
        };
    }

    private static Panel CreateCheckRow(CheckBox checkBox, string labelText, string description)
    {
        Label descriptionLabel = new()
        {
            ForeColor = SystemColors.GrayText,
            Location = new Point(24, 28),
            Size = new Size(660, 28),
            Text = description
        };

        checkBox.Location = new Point(0, 0);
        checkBox.Size = new Size(680, 24);
        checkBox.Text = labelText;
        checkBox.UseVisualStyleBackColor = true;

        Panel panel = new()
        {
            Margin = new Padding(0, 0, 0, 4),
            Size = new Size(700, 58)
        };
        panel.Controls.Add(checkBox);
        panel.Controls.Add(descriptionLabel);
        return panel;
    }

    private static Panel CreateComboRow(string labelText, Control control, string description)
    {
        Label label = new()
        {
            Location = new Point(0, 4),
            Size = new Size(170, 24),
            Text = labelText
        };

        Label descriptionLabel = new()
        {
            ForeColor = SystemColors.GrayText,
            Location = new Point(180, 30),
            Size = new Size(500, 30),
            Text = description
        };

        control.Location = new Point(180, 0);

        Panel panel = new()
        {
            Margin = new Padding(0, 0, 0, 4),
            Size = new Size(700, 64)
        };
        panel.Controls.Add(label);
        panel.Controls.Add(control);
        panel.Controls.Add(descriptionLabel);
        return panel;
    }
}
