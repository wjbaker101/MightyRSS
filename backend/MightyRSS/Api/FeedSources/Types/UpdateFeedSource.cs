namespace MightyRSS.Api.FeedSources.Types;

public sealed class UpdateFeedSourceRequest
{
    public string Collection { get; init; }
    public string Title { get; init; }
}