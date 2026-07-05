using ContextBinder.Models;

namespace ContextBinder.Services;

public sealed class DragDropService
{
    private readonly FileTypeDetector _fileTypeDetector;

    public DragDropService(FileTypeDetector fileTypeDetector)
    {
        _fileTypeDetector = fileTypeDetector;
    }

    public IReadOnlyList<BinderItem> CreateItemsFromDataObject(IDataObject dataObject, string groupId)
    {
        List<BinderItem> items = [];

        if (dataObject.GetDataPresent(DataFormats.FileDrop)
            && dataObject.GetData(DataFormats.FileDrop) is string[] paths)
        {
            foreach (string path in paths)
            {
                BinderItemType type = _fileTypeDetector.DetectFromPath(path);
                items.Add(new BinderItem
                {
                    GroupId = groupId,
                    Type = type,
                    Title = _fileTypeDetector.CreateDefaultTitle(type, path),
                    PathOrUrl = path
                });
            }
        }

        if (dataObject.GetDataPresent(DataFormats.Text)
            && dataObject.GetData(DataFormats.Text) is string text
            && !string.IsNullOrWhiteSpace(text))
        {
            BinderItemType type = _fileTypeDetector.DetectFromText(text.Trim());
            items.Add(new BinderItem
            {
                GroupId = groupId,
                Type = type,
                Title = _fileTypeDetector.CreateDefaultTitle(type, text.Trim(), text),
                PathOrUrl = type == BinderItemType.Url ? text.Trim() : string.Empty,
                TemplateText = type == BinderItemType.Template ? text : string.Empty
            });
        }

        return items;
    }
}
