using CodeHollow.FeedReader;
using MightyRSS._Api.Feed.Types;
using System;
using System.Linq;
using WJBCommon.Lib.Api.Type;

namespace MightyRSS._Api.Feed
{
    public interface IFeedReaderService
    {
        Result<FeedDetails> Read(string url, Guid? reference);
    }

    public sealed class FeedReaderService : IFeedReaderService
    {
        public Result<FeedDetails> Read(string url, Guid? reference)
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out var sourceUrl))
                return Result<FeedDetails>.Error("The given URL was formatted incorrectly please try again.");

            try
            {
                return Read(sourceUrl.ToString(), url, reference);
            }
            catch
            {
                return Result<FeedDetails>.Error("Sorry, unable to retrieve details of the feed. Please try again later.");
            }
        }

        private Result<FeedDetails> Read(string sourceUrl, string url, Guid? reference)
        {
            var feed = FeedReader.ReadAsync(sourceUrl).Result;

            return Result<FeedDetails>.Of(new FeedDetails
            {
                Reference = reference ?? Guid.NewGuid(),
                Title = feed.Title,
                Description = feed.Description,
                RssUrl = url,
                WebsiteUrl = feed.Link,
                Articles = feed.Items
                    .Select(x => new FeedDetails.Article
                    {
                        Url = x.Link,
                        Title = x.Title,
                        Summary = x.Description,
                        Author = x.Author,
                        PublishedAt = x.PublishingDate,
                        PublishedAtAsString = x.PublishingDateString
                    })
                    .ToList()
            });
        }
    }
}