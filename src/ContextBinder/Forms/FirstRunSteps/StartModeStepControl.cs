using static ContextBinder.UiTexts.FirstRunSetup;

namespace ContextBinder.Forms.FirstRunSteps;

public sealed partial class StartModeStepControl : UserControl
{
    public StartModeStepControl()
    {
        InitializeComponent();
    }

    public event EventHandler? SelectionChanged;

    public bool IsCustomSetup => _customSetupRadio.Checked;

    public void SelectRecommended()
    {
        _recommendedSetupRadio.Checked = true;
    }

    private void SetupRadio_CheckedChanged(object? sender, EventArgs e)
    {
        if (((RadioButton)sender!).Checked)
        {
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
