using ContextBinder.Models;
using ContextBinder.Services;
using System.ComponentModel;
using static ContextBinder.UiTexts.FirstRunSetup;

namespace ContextBinder.Forms.FirstRunSteps;

public sealed partial class UsabilitySettingsStepControl : UserControl
{
    private readonly List<Control> _editableSettingsControls = [];
    private bool _isCustomSetup;

    public UsabilitySettingsStepControl()
    {
        InitializeComponent();
        RegisterEditableControls();
        if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
        {
            return;
        }

        LoadSettings(AppSettingsFactory.CreateRecommended());
        SetCustomSetup(false);
    }

    public TypeDisplayMode SelectedTypeDisplayMode => _typeDisplayModeComboBox.SelectedIndex switch
    {
        1 => TypeDisplayMode.IconOnly,
        2 => TypeDisplayMode.TextOnly,
        3 => TypeDisplayMode.Hidden,
        _ => TypeDisplayMode.IconAndText
    };

    public void SetCustomSetup(bool isCustomSetup)
    {
        _isCustomSetup = isCustomSetup;
        _headingLabel.Text = isCustomSetup ? SettingsCustomHeading : SettingsRecommendedHeading;
        _descriptionLabel.Text = isCustomSetup ? SettingsCustomDescription : SettingsRecommendedDescription;
        _editRecommendedSettingsCheckBox.Visible = !isCustomSetup;
        UpdateSettingsEditability();
    }

    public void LoadSettings(AppSettings settings)
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

    public AppSettings CreateSettings()
    {
        AppSettings settings = AppSettingsFactory.CreateRecommended();
        settings.TypeDisplayMode = SelectedTypeDisplayMode;
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

    private void EditRecommendedSettingsCheckBox_CheckedChanged(object? sender, EventArgs e)
    {
        UpdateSettingsEditability();
    }

    private void RegisterEditableControls()
    {
        _editableSettingsControls.AddRange(
        [
            _typeDisplayModeComboBox,
            _showBeginnerHintsCheckBox,
            _showIconLegendCheckBox,
            _showOperationStatusCheckBox,
            _confirmTitleOnDropAddCheckBox,
            _focusExistingItemOnDuplicateCheckBox,
            _enableGroupDropModifierShortcutsCheckBox,
            _confirmGroupDropCopyMoveCheckBox,
            _enableItemDragReorderCheckBox,
            _enableExternalFileDropOutCheckBox,
            _enableExternalUrlTextDragOutCheckBox,
            _enableExternalTemplateTextDragOutCheckBox,
            _minimizeToTrayOnCloseCheckBox,
            _enableContextMenuDetailsCheckBox,
            _autoBackupEnabledCheckBox,
            _maxBackupCountNumeric,
            _confirmBeforeDeleteCheckBox,
            _moveDeletedItemsToTrashCheckBox,
            _searchTemplateBodyCheckBox
        ]);
    }

    private void UpdateSettingsEditability()
    {
        bool canEdit = _isCustomSetup || _editRecommendedSettingsCheckBox.Checked;
        foreach (Control control in _editableSettingsControls)
        {
            control.Enabled = canEdit;
        }
    }
}
