using ContextBinder.Forms;
using ContextBinder.Models;
using ContextBinder.Services;

namespace ContextBinder;

internal static class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        ApplicationConfiguration.Initialize();

        StorageLocationService storageLocationService = new(args);
        StorageLocation? storageLocation = ResolveStorageLocation(storageLocationService);
        if (storageLocation is null)
        {
            return;
        }

        Application.Run(new MainForm(new StoreService(storageLocation, new BackupService())));
    }

    private static StorageLocation? ResolveStorageLocation(StorageLocationService storageLocationService)
    {
        try
        {
            StorageLocation? existingLocation = storageLocationService.ResolveExistingLocation();
            if (existingLocation is not null)
            {
                return existingLocation;
            }
        }
        catch (InvalidOperationException ex)
        {
            MessageBox.Show(
                ex.Message,
                "保存場所設定の確認",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        while (true)
        {
            using FirstRunSetupForm setupForm = new();
            if (setupForm.ShowDialog() != DialogResult.OK)
            {
                return null;
            }

            try
            {
                StorageLocation location = storageLocationService.InitializeFirstRun(
                    setupForm.SelectedStorageMode,
                    setupForm.CustomStorageDirectory);

                StoreService storeService = new(location, new BackupService());
                storeService.SaveSettings(storageLocationService.CreateInitialSettings(location));

                return location;
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or InvalidOperationException)
            {
                MessageBox.Show(
                    $"登録内容と設定の保存場所を準備できませんでした。\n\n{ex.Message}\n\n通常の場所に保存するか、別のフォルダを選んでください。",
                    "保存場所の準備エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
