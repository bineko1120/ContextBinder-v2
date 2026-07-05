namespace ContextBinder.Models;

public sealed class BinderGroup
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    public string Name { get; set; } = string.Empty;

    public int SortOrder { get; set; }

    public List<BinderItem> Items { get; set; } = [];

    public override string ToString()
    {
        return Name;
    }
}
