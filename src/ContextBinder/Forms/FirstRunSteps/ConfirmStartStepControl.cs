using ContextBinder.Models;
using ContextBinder.Services;
using static ContextBinder.UiTexts.FirstRunSetup;

namespace ContextBinder.Forms.FirstRunSteps;

public sealed partial class ConfirmStartStepControl : UserControl
{
    public ConfirmStartStepControl()
    {
        InitializeComponent();
        UpdateSummary(
            isCustomSetup: false,
            storageMode: StorageMode.Standard,
            storagePreviewText: "%AppData%\\ContextBinder\\",
            settings: AppSettingsFactory.CreateRecommended());
    }

    public void UpdateSummary(
        bool isCustomSetup,
        StorageMode storageMode,
        string storagePreviewText,
        AppSettings settings)
    {
        SetLines(_startModeLinesPanel, [isCustomSetup ? CustomSetupSummary : RecommendedSetupSummary]);
        SetLines(_storageLinesPanel, [GetStorageModeDisplayName(storageMode), storagePreviewText]);
        SetLines(_mainSettingsLinesPanel, BuildMainSettingsSummary(settings));
        SetLines(_createdItemsLinesPanel, CreatedItems);
        SetLines(_savedItemsLinesPanel, SavedItems);
        SetLines(_notSavedItemsLinesPanel, NotSavedItems);
    }

    private static void SetLines(FlowLayoutPanel panel, IEnumerable<string> lines)
    {
        panel.SuspendLayout();
        panel.Controls.Clear();
        foreach (string line in lines)
        {
            panel.Controls.Add(new Label
            {
                AutoSize = true,
                MaximumSize = new Size(680, 0),
                Margin = new Padding(0, 0, 0, 4),
                Text = line
            });
        }
        panel.ResumeLayout();
    }
}
