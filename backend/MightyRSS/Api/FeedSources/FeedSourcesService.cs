using Core.Models.Mappers;
using Data.Records;
using Data.UoW;
using MightyRSS.Api.FeedSources.Types;
using NetApiLibs.Type;
using System;
using System.Linq;

namespace MightyRSS.Api.FeedSources;

public interface IFeedSourcesService
{
    Result<AddFeedSourceResponse> AddFeedSource(UserRecord user, AddFeedSourceRequest request);
    Result<UpdateFeedSourceResponse> UpdateFeedSource(UserRecord user, Guid feedReference, UpdateFeedSourceRequest request);
    Result DeleteFeedSource(UserRecord user, Guid reference);
}

public sealed class FeedSourcesService : IFeedSourcesService
{
    private readonly IUnitOfWorkFactory<IMightyUnitOfWork> _mightyUnitOfWorkFactory;
    private readonly IFeedReaderService _feedReaderService;

    public FeedSourcesService(IUnitOfWorkFactory<IMightyUnitOfWork> mightyUnitOfWorkFactory, IFeedReaderService feedReaderService)
    {
        _mightyUnitOfWorkFactory = mightyUnitOfWorkFactory;
        _feedReaderService = feedReaderService;
    }

    public Result<AddFeedSourceResponse> AddFeedSource(UserRecord user, AddFeedSourceRequest request)
    {
        using var unitOfWork = _mightyUnitOfWorkFactory.Create();

        var feedSourceResult = unitOfWork.FeedSources.GetByRssUrl(request.Url);
        if (!feedSourceResult.TrySuccess(out var feedSource))
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
                ArticlesUpdatedAt = DateTime.UtcNow
            };

            unitOfWork.FeedSources.Save(feedSource);
        }

        var existingUserFeedSource = unitOfWork.UserFeedSources.GetByUserAndFeedSourceReference(user, feedSource.Reference);
        if (existingUserFeedSource.IsSuccess)
            return Result<AddFeedSourceResponse>.Failure("Feed source could not be added as it already exists.");

        var userFeedSource = new UserFeedSourceRecord
        {
            User = user,
            FeedSource = feedSource,
            Collection = null,
            Title = feedSource.Title
        };

        unitOfWork.UserFeedSources.Save(userFeedSource);

        unitOfWork.Commit();

        return new AddFeedSourceResponse
        {
            FeedSource = FeedSourceMapper.Map(feedSource, userFeedSource),
            Articles = feedSource.Articles.ConvertAll(x => new AddFeedSourceResponse.FeedArticle
            {
                Url = x.Url,
                Title = x.Title,
                Summary = x.Summary,
                Author = x.Author,
                PublishedAt = x.PublishedAt,
                PublishedAtAsString = x.PublishedAtAsString
            })
        };
    }

    public Result<UpdateFeedSourceResponse> UpdateFeedSource(UserRecord user, Guid feedReference, UpdateFeedSourceRequest request)
    {
        using var unitOfWork = _mightyUnitOfWorkFactory.Create();

        var userFeedSourceResult = unitOfWork.UserFeedSources.GetByUserAndFeedSourceReference(user, feedReference);
        if (!userFeedSourceResult.TrySuccess(out var userFeedSource))
            return Result<UpdateFeedSourceResponse>.FromFailure(userFeedSourceResult);

        userFeedSource.Collection = request.Collection;
        userFeedSource.Title = request.Title;

        unitOfWork.UserFeedSources.Update(userFeedSource);

        unitOfWork.Commit();

        return new UpdateFeedSourceResponse();
    }

    public Result DeleteFeedSource(UserRecord user, Guid reference)
    {
        using var unitOfWork = _mightyUnitOfWorkFactory.Create();

        var userFeedSourceResult = unitOfWork.UserFeedSources.GetByUserAndFeedSourceReference(user, reference);
        if (!userFeedSourceResult.TrySuccess(out var userFeedSource))
            return Result.FromFailure(userFeedSourceResult);

        unitOfWork.UserFeedSources.Delete(userFeedSource);

        unitOfWork.Commit();

        return Result.Success();
    }
}