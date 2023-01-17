namespace MightyRSS._Api.Feed.Types;

public sealed class UpdateFeedSourceRequest
{
    public string Collection { get; init; }
    public string Title { get; init; }
}