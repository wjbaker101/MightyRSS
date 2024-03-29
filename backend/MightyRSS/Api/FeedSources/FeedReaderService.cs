﻿using CodeHollow.FeedReader;
using MightyRSS.Api.FeedSources.Types;
using NetApiLibs.Extension;
using NetApiLibs.Type;
using System;

namespace MightyRSS.Api.FeedSources;

public interface IFeedReaderService
{
    Result<FeedDetails> Read(string url, Guid? reference);
}

public sealed class FeedReaderService : IFeedReaderService
{
    public Result<FeedDetails> Read(string url, Guid? reference)
    {
        if (!Uri.TryCreate(url, UriKind.Absolute, out var sourceUrl))
            return Result<FeedDetails>.Failure("The given URL was formatted incorrectly please try again.");

        try
        {
            return Read(sourceUrl.ToString(), url, reference);
        }
        catch
        {
            return Result<FeedDetails>.Failure("Sorry, unable to retrieve details of the feed. Please try again later.");
        }
    }

    private static FeedDetails Read(string sourceUrl, string url, Guid? reference)
    {
        var feed = FeedReader.ReadAsync(sourceUrl).ConfigureAwait(false).GetAwaiter().GetResult();

        return new FeedDetails
        {
            Reference = reference ?? Guid.NewGuid(),
            Title = feed.Title,
            Description = feed.Description,
            RssUrl = url,
            WebsiteUrl = feed.Link,
            Articles = feed.Items.ConvertAll(x => new FeedDetails.Article
            {
                Url = x.Link,
                Title = x.Title,
                Summary = x.Description,
                Author = x.Author,
                PublishedAt = x.PublishingDate,
                PublishedAtAsString = x.PublishingDateString
            })
        };
    }
}