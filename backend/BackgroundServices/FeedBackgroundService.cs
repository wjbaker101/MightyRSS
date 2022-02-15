using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MightyRSS._Api.Feed;
using MightyRSS.Data.Records;
using MightyRSS.Data.UoW;
using MightyRSS.Settings;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MightyRSS.BackgroundServices;

public sealed class FeedBackgroundService : BackgroundService
{
    private readonly FeedSettings _feedSettings;
    private readonly IUnitOfWorkFactory<IMightyUnitOfWork> _mightyUnitOfWorkFactory;
    private readonly IFeedReaderService _feedReaderService;

    public FeedBackgroundService(
        IOptions<FeedSettings> feedSettings,
        IUnitOfWorkFactory<IMightyUnitOfWork> mightyUnitOfWorkFactory,
        IFeedReaderService feedReaderService)
    {
        _mightyUnitOfWorkFactory = mightyUnitOfWorkFactory;
        _feedReaderService = feedReaderService;
        _feedSettings = feedSettings.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Handle();

            await Task.Delay(TimeSpan.FromSeconds(_feedSettings.RefreshPeriod), stoppingToken);
        }
    }

    private void Handle()
    {
        try
        {
            UpdateFeeds();
        }
        catch
        {
            // ignored
        }
    }

    private void UpdateFeeds()
    {
        using var unitOfWork = _mightyUnitOfWorkFactory.Create();

        var userFeedSources = unitOfWork.UserFeedSources.GetAll();

        foreach (var feedSource in userFeedSources)
            UpdateFeedSource(unitOfWork, feedSource.FeedSource);

        unitOfWork.Commit();
    }

    private void UpdateFeedSource(IMightyUnitOfWork unitOfWork, FeedSourceRecord feedSource)
    {
        var feedDetailsResult = _feedReaderService.Read(feedSource.RssUrl, feedSource.Reference);
        if (feedDetailsResult.IsFailure)
            return;

        var feedDetails = feedDetailsResult.Value;

        feedSource.Title = feedDetails.Title;
        feedSource.Description = feedDetails.Description;
        feedSource.RssUrl = feedDetails.RssUrl;
        feedSource.WebsiteUrl = feedDetails.WebsiteUrl;
        feedSource.Articles = feedDetails.Articles.ConvertAll(x => new FeedSourceRecord.Article
        {
            Url = x.Url,
            Title = x.Title,
            Summary = x.Summary,
            PublishedAt = x.PublishedAt,
            PublishedAtAsString = x.PublishedAtAsString,
            Author = x.Author
        });
        feedSource.ArticlesUpdatedAt = DateTime.UtcNow;

        unitOfWork.FeedSources.Update(feedSource);
    }
}