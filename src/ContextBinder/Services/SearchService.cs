using ContextBinder.Models;

namespace ContextBinder.Services;

public sealed class SearchService
{
    public IReadOnlyList<BinderItem> FilterItems(IEnumerable<BinderGroup> groups, string keyword, SearchScope scope, string? currentGroupId)
    {
        IEnumerable<BinderItem> items = scope == SearchScope.CurrentGroup
            ? groups.FirstOrDefault(group => group.Id == currentGroupId)?.Items ?? []
            : groups.SelectMany(group => group.Items);

        if (string.IsNullOrWhiteSpace(keyword))
        {
            return items.ToArray();
        }

        return items
            .Where(item =>
                item.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                || item.PathOrUrl.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                || item.TemplateText.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            .ToArray();
    }
}
