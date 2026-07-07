using ContextBinder.Forms.FirstRunSteps;
using ContextBinder.Models;
using ContextBinder.Services;
using System.ComponentModel;
using static ContextBinder.UiTexts.FirstRunSetup;

namespace ContextBinder.Forms;

public sealed partial class FirstRunSetupForm : Form
{
    private const int LastStepIndex = 3;

    private readonly StorageLocationService _storageLocationService;
    private readonly StartModeStepControl _startModeStepControl;
    private readonly StorageLocationStepControl _storageLocationStepControl;
    private readonly UsabilitySettingsStepControl _usabilitySettingsStepControl;
    private readonly ConfirmStartStepControl _confirmStartStepControl;
    private int _currentStep;

    public FirstRunSetupForm(StorageLocationService? storageLocationService = null)
    {
        _storageLocationService = storageLocationService ?? new StorageLocationService();

        InitializeComponent();

        _startModeStepControl = new StartModeStepControl();
        _storageLocationStepControl = new StorageLocationStepControl();
        _usabilitySettingsStepControl = new UsabilitySettingsStepControl();
        _confirmStartStepControl = new ConfirmStartStepControl();

        InitializeStepControls();
        WireShellEvents();
        if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
        {
            return;
        }

        RenderCurrentStep();
    }

    public StorageMode SelectedStorageMode { get; private set; } = StorageMode.Standard;

    public string CustomStorageDirectory { get; private set; } = string.Empty;

    public AppSettings SelectedAppSettings { get; private set; } = AppSettingsFactory.CreateRecommended();

    private void InitializeStepControls()
    {
        _startModeStepControl.Dock = DockStyle.Fill;
        _storageLocationStepControl.Dock = DockStyle.Fill;
        _usabilitySettingsStepControl.Dock = DockStyle.Fill;
        _confirmStartStepControl.Dock = DockStyle.Fill;

        _startModeStepControl.SelectionChanged += (_, _) =>
        {
            _usabilitySettingsStepControl.SetCustomSetup(_startModeStepControl.IsCustomSetup);
        };
        _storageLocationStepControl.SelectionChanged += (_, _) => UpdateStoragePreview();
        _storageLocationStepControl.BrowseRequested += BrowseButton_Click;

        _startModeStepControl.SelectRecommended();
        _storageLocationStepControl.SelectStandard();
        UpdateStoragePreview();
    }

    private void WireShellEvents()
    {
        _backButton.Click += (_, _) => MoveStep(-1);
        _nextButton.Click += NextButton_Click;
    }

    private void RenderCurrentStep()
    {
        _contentPanel.Controls.Clear();
        _stepLabel.Text = CreateStepText();
        _backButton.Enabled = _currentStep > 0;
        _nextButton.Text = _currentStep == LastStepIndex ? StartButton : NextButton;

        Control page = _currentStep switch
        {
            0 => _startModeStepControl,
            1 => _storageLocationStepControl,
            2 => PrepareUsabilitySettingsStep(),
            _ => PrepareConfirmStartStep()
        };

        _contentPanel.Controls.Add(page);
    }

    private string CreateStepText()
    {
        return string.Join("  →  ", StepLabels.Select((step, index) => index == _currentStep ? $"【{step}】" : step));
    }

    private Control PrepareUsabilitySettingsStep()
    {
        _usabilitySettingsStepControl.SetCustomSetup(_startModeStepControl.IsCustomSetup);
        return _usabilitySettingsStepControl;
    }

    private Control PrepareConfirmStartStep()
    {
        UpdateStoragePreview();
        SelectedAppSettings = _usabilitySettingsStepControl.CreateSettings();
        _confirmStartStepControl.UpdateSummary(
            _startModeStepControl.IsCustomSetup,
            SelectedStorageMode,
            CreateStoragePreviewText(),
            SelectedAppSettings);
        return _confirmStartStepControl;
    }

    private void UpdateStoragePreview()
    {
        SelectedStorageMode = _storageLocationStepControl.SelectedStorageMode;
        CustomStorageDirectory = SelectedStorageMode == StorageMode.Custom
            ? _storageLocationStepControl.CustomStorageDirectory
            : string.Empty;

        string description = SelectedStorageMode switch
        {
            StorageMode.Portable => PortableStorageCurrentDescription,
            StorageMode.Custom => CustomStorageCurrentDescription,
            _ => StandardStorageCurrentDescription
        };

        _storageLocationStepControl.SetPreview(description, CreateStoragePreviewText());
    }

    private string CreateStoragePreviewText()
    {
        if (SelectedStorageMode == StorageMode.Custom
            && string.IsNullOrWhiteSpace(_storageLocationStepControl.CustomStorageDirectory))
        {
            return CustomStorageNotSelected;
        }

        try
        {
            StorageLocation location = _storageLocationService.CreateLocation(
                SelectedStorageMode,
                SelectedStorageMode == StorageMode.Custom
                    ? _storageLocationStepControl.CustomStorageDirectory
                    : null);
            return location.DataDirectory;
        }
        catch (InvalidOperationException ex)
        {
            return ex.Message;
        }
    }

    private void BrowseButton_Click(object? sender, EventArgs e)
    {
        using FolderBrowserDialog dialog = new()
        {
            Description = CustomFolderDialogDescription,
            UseDescriptionForTitle = true
        };

        if (!string.IsNullOrWhiteSpace(_storageLocationStepControl.CustomStorageDirectory))
        {
            dialog.InitialDirectory = _storageLocationStepControl.CustomStorageDirectory;
        }

        if (dialog.ShowDialog(this) == DialogResult.OK)
        {
            _storageLocationStepControl.SetCustomDirectory(dialog.SelectedPath);
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

        SelectedStorageMode = _storageLocationStepControl.SelectedStorageMode;
        CustomStorageDirectory = SelectedStorageMode == StorageMode.Custom
            ? _storageLocationStepControl.CustomStorageDirectory
            : string.Empty;
        SelectedAppSettings = _usabilitySettingsStepControl.CreateSettings();
        DialogResult = DialogResult.OK;
        Close();
    }

    private bool ValidateCurrentStep()
    {
        if (_currentStep == 1
            && _storageLocationStepControl.SelectedStorageMode == StorageMode.Custom
            && string.IsNullOrWhiteSpace(_storageLocationStepControl.CustomStorageDirectory))
        {
            MessageBox.Show(this, CustomStorageRequiredMessage, ValidationDialogTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (_currentStep == 2 && _usabilitySettingsStepControl.SelectedTypeDisplayMode == TypeDisplayMode.Hidden)
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
}
