using System.Diagnostics;
using ContextBinder.Models;

namespace ContextBinder.Services;

public sealed class ItemActionService
{
    private readonly ClipboardService _clipboardService;

    public ItemActionService(ClipboardService clipboardService)
    {
        _clipboardService = clipboardService;
    }

    public void Open(BinderItem item)
    {
        if (item.Type == BinderItemType.Template)
        {
            _clipboardService.CopyItemReference(item);
            return;
        }

        if (string.IsNullOrWhiteSpace(item.PathOrUrl))
        {
            throw new InvalidOperationException("参照先が空です。");
        }

        Process.Start(new ProcessStartInfo
        {
            FileName = item.PathOrUrl,
            UseShellExecute = true
        });
    }

    public void OpenContainingFolder(BinderItem item)
    {
        if (item.Type == BinderItemType.Folder)
        {
            Open(item);
            return;
        }

        if (item.Type is not (BinderItemType.File or BinderItemType.Image or BinderItemType.Video))
        {
            return;
        }

        if (File.Exists(item.PathOrUrl))
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "explorer.exe",
                Arguments = $"/select,\"{item.PathOrUrl}\"",
                UseShellExecute = true
            });
            return;
        }

        string? parentDirectory = Path.GetDirectoryName(item.PathOrUrl);
        if (!string.IsNullOrWhiteSpace(parentDirectory) && Directory.Exists(parentDirectory))
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = parentDirectory,
                UseShellExecute = true
            });
        }
    }

    public void Copy(BinderItem item)
    {
        _clipboardService.CopyItemReference(item);
    }
}
