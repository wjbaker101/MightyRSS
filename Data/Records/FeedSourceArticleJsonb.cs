using System;

namespace MightyRSS.Data.Records
{
    public sealed class FeedSourceArticleJsonb
    {
        public string Url { get; init; }
        public string Title { get; init; }
        public string Summary { get; init; }
        public string Author { get; init; }
        public DateTime? PublishedAt { get; init; }
    }
}