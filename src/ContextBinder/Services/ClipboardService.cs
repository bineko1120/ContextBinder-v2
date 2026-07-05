using ContextBinder.Models;

namespace ContextBinder.Services;

public sealed class ClipboardService
{
    public void CopyItemReference(BinderItem item)
    {
        string text = item.Type == BinderItemType.Template ? item.TemplateText : item.PathOrUrl;
        if (!string.IsNullOrEmpty(text))
        {
            Clipboard.SetText(text);
        }
    }

    public void CopyTitle(BinderItem item)
    {
        if (!string.IsNullOrWhiteSpace(item.Title))
        {
            Clipboard.SetText(item.Title);
        }
    }
}
