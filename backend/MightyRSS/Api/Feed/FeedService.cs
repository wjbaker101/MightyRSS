using Core.Models.Mappers;
using Data.Records;
using Data.UoW;
using MightyRSS.Api.Feed.Types;
using NetApiLibs.Type;
using System;

namespace MightyRSS.Api.Feed;

public interface IFeedService
{
    Result<GetFeedResponse> GetFeed(UserRecord user);
    Result AddFeedToCollection(UserRecord user, Guid feedReference, AddFeedToCollectionRequest request);
}

public sealed class FeedService : IFeedService
{
    private readonly IUnitOfWorkFactory<IMightyUnitOfWork> _mightyUnitOfWorkFactory;

    public FeedService(IUnitOfWorkFactory<IMightyUnitOfWork> mightyUnitOfWorkFactory)
    {
        _mightyUnitOfWorkFactory = mightyUnitOfWorkFactory;
    }

    public Result<GetFeedResponse> GetFeed(UserRecord user)
    {
        using var unitOfWork = _mightyUnitOfWorkFactory.Create();

        var userFeedSources = unitOfWork.UserFeedSources.GetFeedSources(user);

        unitOfWork.Commit();

        return new GetFeedResponse
        {
            Sources = userFeedSources.ConvertAll(userFeedSource => new GetFeedResponse.FeedSourceDetails
            {
                FeedSource = FeedSourceMapper.Map(userFeedSource.FeedSource, userFeedSource),
                Articles = userFeedSource.FeedSource.Articles.ConvertAll(article => new GetFeedResponse.FeedArticle
                {
                    Url = article.Url,
                    Title = article.Title,
                    Summary = article.Summary,
                    Author = article.Author,
                    PublishedAt = article.PublishedAt,
                    PublishedAtAsString = article.PublishedAtAsString
                })
            })
        };
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
}