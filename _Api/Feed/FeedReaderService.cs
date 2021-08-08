using CodeHollow.FeedReader;
using MightyRSS._Api.Feed.Types;
using System;
using System.Linq;

namespace MightyRSS._Api.Feed
{
    public interface IFeedReaderService
    {
        FeedReaderResult Read(string url, Guid? reference);
    }

    public sealed class FeedReaderService : IFeedReaderService
    {
        public FeedReaderResult Read(string url, Guid? reference)
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out var sourceUrl))
                return null;

            try
            {
                return Read(sourceUrl.ToString(), url, reference);
            }
            catch
            {
                return null;
            }
        }

        private FeedReaderResult Read(string sourceUrl, string url, Guid? reference)
        {
            var feed = FeedReader.ReadAsync(sourceUrl).Result;

            return new FeedReaderResult
            {
                Reference = reference ?? Guid.NewGuid(),
                Title = feed.Title,
                Description = feed.Description,
                RssUrl = url,
                WebsiteUrl = feed.Link,
                Articles = feed.Items
                    .Select(x => new FeedReaderResult.FeedArticle
                    {
                        Url = x.Link,
                        Title = x.Title,
                        Summary = x.Description,
                        Author = x.Author,
                        PublishedAt = x.PublishingDate,
                        PublishedAtAsString = x.PublishingDateString
                    })
                    .ToList()
            };
        }
    }
}