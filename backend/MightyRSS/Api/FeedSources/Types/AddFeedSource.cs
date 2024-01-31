using MightyRSS.Models;
using System;
using System.Collections.Generic;

namespace MightyRSS.Api.FeedSources.Types;

public sealed class AddFeedSourceRequest
{
    public required string Url { get; init; }
}

public sealed class AddFeedSourceResponse
{
    public required FeedSourceModel FeedSource { get; init; }
    public required List<FeedArticle> Articles { get; init; }

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