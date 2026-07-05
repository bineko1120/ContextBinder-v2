namespace ContextBinder.Models;

public sealed class ContextBinderStore
{
    public int SchemaVersion { get; set; } = 1;

    public List<BinderGroup> Groups { get; set; } = [];

    public List<DeletedItemRecord> DeletedItems { get; set; } = [];

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;

    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.Now;
}
