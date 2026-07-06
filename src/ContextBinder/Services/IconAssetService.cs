using ContextBinder.Models;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace ContextBinder.Services;

public sealed class IconAssetService : IDisposable
{
    private const int ItemIconSize = 28;
    private readonly string _iconDirectory;
    private readonly Dictionary<BinderItemType, Image> _itemIconCache = [];
    private readonly Image _placeholderImage;
    private Icon? _appIcon;
    private Icon? _trayIcon;
    private bool _disposed;

    public IconAssetService(string? iconDirectory = null)
    {
        _iconDirectory = Path.GetFullPath(iconDirectory ?? Path.Combine(AppContext.BaseDirectory, "Assets", "Icons"));
        _placeholderImage = CreatePlaceholderImage();
    }

    public Image GetItemIcon(BinderItemType type)
    {
        ThrowIfDisposed();

        if (_itemIconCache.TryGetValue(type, out Image? cachedImage))
        {
            return cachedImage;
        }

        string iconPath = GetIconPath(GetItemIconFileName(type));
        Image image = TryLoadScaledImage(iconPath, ItemIconSize) ?? _placeholderImage;

        _itemIconCache[type] = image;
        return image;
    }

    public Icon GetAppIcon()
    {
        ThrowIfDisposed();
        _appIcon ??= LoadIconOrDefault("icon_app.ico");
        return _appIcon;
    }

    public Icon GetTrayIcon()
    {
        ThrowIfDisposed();
        _trayIcon ??= LoadIconOrDefault("icon_tray.ico");
        return _trayIcon;
    }

    public string GetItemIconFileName(BinderItemType type)
    {
        return type switch
        {
            BinderItemType.Folder => "icon_cat_folder.png",
            BinderItemType.File => "icon_cat_file.png",
            BinderItemType.Image => "icon_cat_image.png",
            BinderItemType.Video => "icon_cat_video.png",
            BinderItemType.Url => "icon_cat_url.png",
            BinderItemType.Template => "icon_cat_template.png",
            _ => "icon_cat_file.png"
        };
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        foreach (Image image in _itemIconCache.Values.Distinct())
        {
            if (!ReferenceEquals(image, _placeholderImage))
            {
                image.Dispose();
            }
        }

        _placeholderImage.Dispose();
        _appIcon?.Dispose();
        _trayIcon?.Dispose();
        _disposed = true;
    }

    private string GetIconPath(string fileName)
    {
        return Path.Combine(_iconDirectory, fileName);
    }

    private static Image? TryLoadScaledImage(string filePath, int size)
    {
        if (!File.Exists(filePath))
        {
            return null;
        }

        try
        {
            using Image sourceImage = Image.FromFile(filePath);
            Bitmap bitmap = new(size, size);

            using Graphics graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.Transparent);
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphics.DrawImage(sourceImage, new Rectangle(0, 0, size, size));

            return bitmap;
        }
        catch (Exception ex) when (IsIconLoadException(ex))
        {
            return null;
        }
    }

    private static Image CreatePlaceholderImage()
    {
        Bitmap bitmap = new(ItemIconSize, ItemIconSize);

        using Graphics graphics = Graphics.FromImage(bitmap);
        graphics.Clear(Color.Gainsboro);
        using Pen pen = new(Color.Gray);
        graphics.DrawRectangle(pen, 0, 0, ItemIconSize - 1, ItemIconSize - 1);
        TextRenderer.DrawText(
            graphics,
            "?",
            SystemFonts.MessageBoxFont,
            new Rectangle(0, 0, ItemIconSize, ItemIconSize),
            Color.DimGray,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

        return bitmap;
    }

    private Icon LoadIconOrDefault(string fileName)
    {
        string iconPath = GetIconPath(fileName);
        if (File.Exists(iconPath))
        {
            try
            {
                return new Icon(iconPath);
            }
            catch (Exception ex) when (IsIconLoadException(ex))
            {
            }
        }

        return (Icon)SystemIcons.Application.Clone();
    }

    private static bool IsIconLoadException(Exception ex)
    {
        return ex is IOException
            or UnauthorizedAccessException
            or ArgumentException
            or ExternalException
            or OutOfMemoryException;
    }

    private void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
    }
}
