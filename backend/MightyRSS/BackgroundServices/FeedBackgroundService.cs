using Core.Settings;
using Data.Records;
using Data.UoW;
using Microsoft.Extensions.Options;
using MightyRSS.Api.FeedSources;

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

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await Handle(cancellationToken);

            await Task.Delay(TimeSpan.FromSeconds(_feedSettings.RefreshPeriod), cancellationToken);
        }
    }

    private async Task Handle(CancellationToken cancellationToken)
    {
        try
        {
            await UpdateFeeds(cancellationToken);
        }
        catch
        {
            // ignored
        }
    }

    private async Task UpdateFeeds(CancellationToken cancellationToken)
    {
        using var unitOfWork = _mightyUnitOfWorkFactory.Create(cancellationToken);

        var userFeedSources = await unitOfWork.UserFeedSources.GetAll();

        foreach (var feedSource in userFeedSources)
            await UpdateFeedSource(unitOfWork, feedSource.FeedSource);

        await unitOfWork.Commit();
    }

    private async Task UpdateFeedSource(IMightyUnitOfWork unitOfWork, FeedSourceRecord feedSource)
    {
        var feedDetailsResult = await _feedReaderService.Read(feedSource.RssUrl, feedSource.Reference);
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

        await unitOfWork.FeedSources.Update(feedSource);
    }
}