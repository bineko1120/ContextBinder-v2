using ContextBinder.Models;
using static ContextBinder.UiTexts.FirstRunSetup;

namespace ContextBinder.Forms.FirstRunSteps;

public sealed partial class StorageLocationStepControl : UserControl
{
    public StorageLocationStepControl()
    {
        InitializeComponent();
        UpdateCustomInputState();
    }

    public event EventHandler? SelectionChanged;

    public event EventHandler? BrowseRequested;

    public StorageMode SelectedStorageMode
    {
        get
        {
            if (_portableStorageRadio.Checked)
            {
                return StorageMode.Portable;
            }

            return _customStorageRadio.Checked ? StorageMode.Custom : StorageMode.Standard;
        }
    }

    public string CustomStorageDirectory => _customDirectoryTextBox.Text.Trim();

    public void SetCustomDirectory(string directory)
    {
        _customDirectoryTextBox.Text = directory;
    }

    public void SetPreview(string description, string previewText)
    {
        _storageDescriptionLabel.Text = description;
        _storagePreviewLabel.Text = $"{StoragePreviewPrefix}: {previewText}";
    }

    public void SelectStandard()
    {
        _standardStorageRadio.Checked = true;
    }

    private void StorageOption_CheckedChanged(object? sender, EventArgs e)
    {
        if (sender is RadioButton { Checked: true })
        {
            UpdateCustomInputState();
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void CustomDirectoryTextBox_TextChanged(object? sender, EventArgs e)
    {
        SelectionChanged?.Invoke(this, EventArgs.Empty);
    }

    private void BrowseButton_Click(object? sender, EventArgs e)
    {
        BrowseRequested?.Invoke(this, EventArgs.Empty);
    }

    private void UpdateCustomInputState()
    {
        bool isCustom = _customStorageRadio.Checked;
        _customDirectoryTextBox.Enabled = isCustom;
        _browseButton.Enabled = isCustom;
    }
}
