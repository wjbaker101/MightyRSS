namespace MightyRSS.Models;

public sealed class FeedSourceModel
{
    public required Guid Reference { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string RssUrl { get; init; }
    public required string WebsiteUrl { get; init; }
}