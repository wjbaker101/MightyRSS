using Core.Models.Mappers;
using Data.Records;
using Data.UoW;
using MightyRSS.Api.FeedSources.Types;
using NetApiLibs.Type;

namespace MightyRSS.Api.Feed;

public interface IFeedService
{
    Result<GetFeedResponse> GetFeed(UserRecord user);
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
}