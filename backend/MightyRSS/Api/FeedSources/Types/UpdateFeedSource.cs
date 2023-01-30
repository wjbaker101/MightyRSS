namespace MightyRSS.Api.FeedSources.Types;

public sealed class UpdateFeedSourceRequest
{
    public required string Collection { get; init; }
    public required string Title { get; init; }
}

public sealed class UpdateFeedSourceResponse
{
}