using ContextBinder.Forms;
using ContextBinder.Models;
using ContextBinder.Services;

namespace ContextBinder;

internal static class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        StartupErrorService startupErrorService = new();
        StorageLocation? storageLocation = null;

        try
        {
            ApplicationConfiguration.Initialize();
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (_, e) => ShowStartupError(e.Exception, storageLocation, startupErrorService);
            AppDomain.CurrentDomain.UnhandledException += (_, e) =>
            {
                if (e.ExceptionObject is Exception exception)
                {
                    startupErrorService.WriteStartupError(exception, storageLocation);
                }
            };

            StorageLocationService storageLocationService = new(args);
            storageLocation = ResolveStorageLocation(storageLocationService);
            if (storageLocation is null)
            {
                return;
            }

            StoreService storeService = new(storageLocation, new BackupService());
            using MainForm mainForm = new(storeService);
            Application.Run(mainForm);
        }
        catch (Exception ex)
        {
            ShowStartupError(ex, storageLocation, startupErrorService);
        }
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
            using FirstRunSetupForm setupForm = new(storageLocationService);
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
                storeService.SaveSettings(storageLocationService.CreateInitialSettings(location, setupForm.SelectedAppSettings));
                storeService.EnsureStoreFile();

                return location;
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or InvalidOperationException)
            {
                MessageBox.Show(
                    $"登録内容と設定の保存場所を準備できませんでした。\n\n{ex.Message}\n\n保存場所を変更するか、フォルダへの書き込み権限を確認してください。",
                    "保存場所の準備エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }

    private static void ShowStartupError(
        Exception exception,
        StorageLocation? storageLocation,
        StartupErrorService startupErrorService)
    {
        string? logPath = startupErrorService.WriteStartupError(exception, storageLocation);
        MessageBox.Show(
            StartupErrorService.CreateUserMessage(exception, logPath),
            "起動エラー",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error);
    }

}
