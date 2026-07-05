using ContextBinder.Models;

namespace ContextBinder.Services;

public sealed class FileTypeDetector
{
    private static readonly HashSet<string> ImageExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".bmp", ".gif", ".jpeg", ".jpg", ".png", ".webp", ".tif", ".tiff"
    };

    private static readonly HashSet<string> VideoExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".avi", ".m2ts", ".m4v", ".mkv", ".mov", ".mp4", ".mpeg", ".mpg", ".webm", ".wmv"
    };

    public BinderItemType DetectFromPath(string path)
    {
        if (Directory.Exists(path))
        {
            return BinderItemType.Folder;
        }

        string extension = Path.GetExtension(path);
        if (ImageExtensions.Contains(extension))
        {
            return BinderItemType.Image;
        }

        if (VideoExtensions.Contains(extension))
        {
            return BinderItemType.Video;
        }

        return BinderItemType.File;
    }

    public BinderItemType DetectFromText(string text)
    {
        if (Uri.TryCreate(text, UriKind.Absolute, out Uri? uri)
            && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
        {
            return BinderItemType.Url;
        }

        return BinderItemType.Template;
    }

    public string CreateDefaultTitle(BinderItemType type, string pathOrUrl, string templateText = "")
    {
        return type switch
        {
            BinderItemType.Folder => new DirectoryInfo(pathOrUrl).Name,
            BinderItemType.File or BinderItemType.Image or BinderItemType.Video => Path.GetFileName(pathOrUrl),
            BinderItemType.Url => pathOrUrl,
            BinderItemType.Template => CreateTemplateTitle(templateText),
            _ => pathOrUrl
        };
    }

    public string GetDisplayName(BinderItemType type)
    {
        return type switch
        {
            BinderItemType.Folder => "フォルダ",
            BinderItemType.File => "ファイル",
            BinderItemType.Image => "画像",
            BinderItemType.Video => "動画",
            BinderItemType.Url => "URL",
            BinderItemType.Template => "テンプレート",
            _ => type.ToString()
        };
    }

    private static string CreateTemplateTitle(string templateText)
    {
        string firstLine = templateText
            .Split(["\r\n", "\n"], StringSplitOptions.None)
            .FirstOrDefault(line => !string.IsNullOrWhiteSpace(line))
            ?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(firstLine))
        {
            return "テンプレート";
        }

        return firstLine.Length <= 40 ? firstLine : firstLine[..40];
    }
}
