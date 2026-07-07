namespace ContextBinder;

// User-editable runtime UI sizing values.
// Prefer Designer Margin/Padding/Size for fixed layout adjustments.
// Keep behavior and event logic in Forms/*.cs.
internal static class UiLayoutSettings
{
    internal static class FirstRun
    {
        public const int ContentWidth = 760;
        public const int InitialWidth = 900;
        public const int InitialHeight = 760;
        public const int MinimumWidth = 860;
        public const int MinimumHeight = 720;
        public const int RootPadding = 16;
        public const int StepHeaderHeight = 48;
        public const int FooterButtonHeight = 48;
        public const int ContentPadding = 14;
        public const int WizardButtonWidth = 96;
        public const int CustomDirectoryTextBoxWidth = 560;
        public const int BrowseButtonWidth = 92;
        public const int StorageDescriptionHeight = 64;
        public const int StoragePreviewHeight = 48;
        public const int SettingCategoryPadding = 10;
        public const int SettingRowHeight = 58;
        public const int ComboRowHeight = 60;
        public const int ConfirmationMinimumSectionHeight = 76;
    }

    internal static class Main
    {
        public const int InitialWidth = 1180;
        public const int InitialHeight = 780;
        public const int MinimumWidth = 980;
        public const int MinimumHeight = 700;
        public const int RootPadding = 10;
        public const int GroupColumnWidth = 190;
        public const int ActionColumnWidth = 185;
        public const int GroupHeaderHeight = 24;
        public const int GridRowHeight = 36;
        public const int ActionButtonWidth = 165;
        public const int ActionButtonHeight = 31;
        public const int ActionButtonBottomMargin = 7;
        public const int ToggleRowHeight = 34;
        public const int BeginnerHintsHeight = 124;
        public const int IconMeaningHeight = 148;
        public const int BottomTopPadding = 8;
        public const int InitialBottomHeight = ToggleRowHeight + BottomTopPadding + 2;
        public const int ToggleCheckBoxWidth = 170;
        public const int StatusLabelWidth = 520;
        public const int StatusLabelHeight = 26;
        public const int BottomTitleHeight = 24;
        public const int IconMeaningItemWidth = 250;
        public const int IconMeaningItemHeight = 50;
        public const int IconMeaningIconSize = 30;
        public const int IconMeaningTextOffsetX = 36;
    }
}
