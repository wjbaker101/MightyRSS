using MightyRSS._Api.Feed.Types;
using MightyRSS.Data.Records;
using MightyRSS.Data.UoW;
using NetApiLibs.Type;
using System;
using System.Linq;
using System.Net;

namespace MightyRSS._Api.Feed;

public interface IFeedService
{
    Result<AddFeedSourceResponse> AddFeedSource(UserRecord user, AddFeedSourceRequest request);
    Result<GetFeedResponse> GetFeed(UserRecord user);
    Result DeleteFeedSource(UserRecord user, Guid reference);
    Result AddFeedToCollection(UserRecord user, Guid feedReference, AddFeedToCollectionRequest request);
    Result UpdateFeedSource(UserRecord user, Guid feedReference, UpdateFeedSourceRequest request);
}

public sealed class FeedService: IFeedService
{
    private readonly IUnitOfWorkFactory<IMightyUnitOfWork> _mightyUnitOfWorkFactory;
    private readonly IFeedReaderService _feedReaderService;

    public FeedService(
        IUnitOfWorkFactory<IMightyUnitOfWork> mightyUnitOfWorkFactory,
        IFeedReaderService feedReaderService)
    {
        _mightyUnitOfWorkFactory = mightyUnitOfWorkFactory;
        _feedReaderService = feedReaderService;
    }

    public Result<AddFeedSourceResponse> AddFeedSource(UserRecord user, AddFeedSourceRequest request)
    {
        using var unitOfWork = _mightyUnitOfWorkFactory.Create();

        var feedSource = unitOfWork.FeedSources.GetByRssUrl(request.Url);
        if (feedSource == null)
        {
            var feedDetailsResult = _feedReaderService.Read(request.Url, null);
            if (feedDetailsResult.IsFailure)
                return Result<AddFeedSourceResponse>.FromFailure(feedDetailsResult);

            var feedDetails = feedDetailsResult.Value;

            feedSource = new FeedSourceRecord
            {
                Reference = feedDetails.Reference,
                Title = feedDetails.Title,
                Description = feedDetails.Description,
                RssUrl = feedDetails.RssUrl,
                WebsiteUrl = feedDetails.WebsiteUrl,
                Articles = feedDetails.Articles
                    .Select(x => new FeedSourceRecord.Article
                    {
                        Url = x.Url,
                        Title = x.Title,
                        Summary = x.Summary,
                        PublishedAt = x.PublishedAt,
                        PublishedAtAsString = x.PublishedAtAsString,
                        Author = x.Author
                    })
                    .ToList(),
                ArticlesUpdatedAt = DateTime.Now.ToLocalTime()
            };

            unitOfWork.FeedSources.Save(feedSource);
        }

        var userFeedSource = new UserDataFeedSourceRecord
        {
            User = user,
            FeedSource = feedSource,
            Collection = null,
            Title = feedSource.Title
        };

        unitOfWork.UserFeedSources.Save(userFeedSource);

        unitOfWork.Commit();

        return Result<AddFeedSourceResponse>.Of(new AddFeedSourceResponse
        {
            Reference = feedSource.Reference,
            Title = feedSource.Title,
            Description = feedSource.Description,
            RssUrl = feedSource.RssUrl,
            WebsiteUrl = feedSource.WebsiteUrl,
            Collection = userFeedSource.Collection,
            Articles = feedSource.Articles.ConvertAll(x => new AddFeedSourceResponse.FeedArticle
            {
                Url = x.Url,
                Title = x.Title,
                Summary = x.Summary,
                Author = x.Author,
                PublishedAt = x.PublishedAt,
                PublishedAtAsString = x.PublishedAtAsString
            })
        });
    }

    public Result<GetFeedResponse> GetFeed(UserRecord user)
    {
        using var unitOfWork = _mightyUnitOfWorkFactory.Create();

        var feedSources = unitOfWork.UserFeedSources.GetFeedSources(user);

        unitOfWork.Commit();

        return Result<GetFeedResponse>.Of(new GetFeedResponse
        {
            Sources = feedSources.ConvertAll(source => new GetFeedResponse.FeedSource
            {
                Reference = source.FeedSource.Reference,
                Title = source.FeedSource.Title,
                Description = source.FeedSource.Description,
                RssUrl = source.FeedSource.RssUrl,
                WebsiteUrl = source.FeedSource.WebsiteUrl,
                Collection = source.Collection,
                Articles = source.FeedSource.Articles.ConvertAll(article => new GetFeedResponse.FeedArticle
                {
                    Url = article.Url,
                    Title = article.Title,
                    Summary = article.Summary,
                    Author = article.Author,
                    PublishedAt = article.PublishedAt,
                    PublishedAtAsString = article.PublishedAtAsString
                })
            })
        });
    }

    public Result DeleteFeedSource(UserRecord user, Guid reference)
    {
        using var unitOfWork = _mightyUnitOfWorkFactory.Create();

        var userFeedSourceResult = unitOfWork.UserFeedSources.GetByUserAndFeedSourceReference(user, reference);
        if (!userFeedSourceResult.TrySuccess(out var userFeedSource))
            return Result.FromFailure(userFeedSourceResult);

        unitOfWork.UserFeedSources.Delete(userFeedSource);

        unitOfWork.Commit();

        return Result.Success(HttpStatusCode.NoContent);
    }

    public Result AddFeedToCollection(UserRecord user, Guid feedReference, AddFeedToCollectionRequest request)
    {
        var unitOfWork = _mightyUnitOfWorkFactory.Create();

        var userFeedSourceResult = unitOfWork.UserFeedSources.GetByUserAndFeedSourceReference(user, feedReference);
        if (!userFeedSourceResult.TrySuccess(out var userFeedSource))
            return Result.FromFailure(userFeedSourceResult);

        userFeedSource.Collection = request.Collection;

        unitOfWork.UserFeedSources.Update(userFeedSource);

        unitOfWork.Commit();

        return Result.Success();
    }

    public Result UpdateFeedSource(UserRecord user, Guid feedReference, UpdateFeedSourceRequest request)
    {
        using var unitOfWork = _mightyUnitOfWorkFactory.Create();

        var userFeedSourceResult = unitOfWork.UserFeedSources.GetByUserAndFeedSourceReference(user, feedReference);
        if (!userFeedSourceResult.TrySuccess(out var userFeedSource))
            return Result.FromFailure(userFeedSourceResult);

        userFeedSource.Collection = request.Collection;
        userFeedSource.Title = request.Title;

        unitOfWork.UserFeedSources.Update(userFeedSource);

        unitOfWork.Commit();

        return Result.Success(HttpStatusCode.Created);
    }
}