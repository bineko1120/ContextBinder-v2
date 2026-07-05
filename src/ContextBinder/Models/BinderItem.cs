using System.Text.Json.Serialization;

namespace ContextBinder.Models;

public sealed class BinderItem
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    public string GroupId { get; set; } = string.Empty;

    public BinderItemType Type { get; set; } = BinderItemType.File;

    public string Title { get; set; } = string.Empty;

    public string PathOrUrl { get; set; } = string.Empty;

    public string TemplateText { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;

    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.Now;

    [JsonIgnore]
    public string ReferenceText => Type == BinderItemType.Template ? TemplateText : PathOrUrl;
}
