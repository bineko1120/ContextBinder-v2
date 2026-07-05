using ContextBinder.Models;

namespace ContextBinder.Services;

public sealed class TrashService
{
    public DeletedItemRecord MoveItemToTrash(BinderGroup group, BinderItem item)
    {
        group.Items.Remove(item);

        return new DeletedItemRecord
        {
            OriginalGroupId = group.Id,
            Item = item,
            Reason = "UserDeleted",
            DeletedAt = DateTimeOffset.Now
        };
    }
}
