using MightyRSS.Models;

namespace MightyRSS.Api.Feed.Types;

public sealed class GetFeedResponse
{
    public required List<FeedSourceDetails> Sources { get; init; }

    public sealed class FeedSourceDetails
    {
        public required FeedSourceModel FeedSource { get; init; }
        public required List<FeedArticle> Articles { get; init; }
    }

    public sealed class FeedArticle
    {
        public required string Url { get; init; }
        public required string Title { get; init; }
        public required string Summary { get; init; }
        public required string Author { get; init; }
        public required DateTime? PublishedAt { get; init; }
        public required string PublishedAtAsString { get; init; }
    }
}