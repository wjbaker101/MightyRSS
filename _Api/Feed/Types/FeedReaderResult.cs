using System;
using System.Collections.Generic;

namespace MightyRSS._Api.Feed.Types
{
    public sealed class FeedReaderResult
    {
        public Guid Reference { get; init; }
        public string Description { get; init; }
        public string RssUrl { get; init; }
        public string WebsiteUrl { get; init; }
        public List<FeedArticle> Articles { get; init; }

        public sealed class FeedArticle
        {
            public string Url { get; init; }
            public string Title { get; init; }
            public string Summary { get; init; }
            public string Author { get; init; }
            public DateTime? PublishedAt { get; init; }
        }
    }
}