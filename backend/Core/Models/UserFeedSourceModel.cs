namespace Core.Models;

public sealed class UserFeedSourceModel
{
    public required string? Collection { get; init; }
    public required string? TitleAlias { get; init; }
}