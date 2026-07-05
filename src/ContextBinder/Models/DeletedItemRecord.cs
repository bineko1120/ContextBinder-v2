namespace ContextBinder.Models;

public sealed class DeletedItemRecord
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    public string OriginalGroupId { get; set; } = string.Empty;

    public BinderItem Item { get; set; } = new();

    public DateTimeOffset DeletedAt { get; set; } = DateTimeOffset.Now;

    public string Reason { get; set; } = string.Empty;
}
