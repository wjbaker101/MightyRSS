using MightyRSS._Api.Feed.Types;
using MightyRSS.Data.Records;
using MightyRSS.Data.UoW;
using System;
using System.Linq;
using System.Net;
using WJBCommon.Lib.Api.Type;
using WJBCommon.Lib.Data;

namespace MightyRSS._Api.Feed
{
    public interface IFeedService
    {
        Result<AddFeedSourceResponse> AddFeedSource(UserRecord user, AddFeedSourceRequest request);
        Result<GetFeedResponse> GetFeed(UserRecord user);
        Result DeleteFeedSource(UserRecord user, Guid reference);
        Result AddFeedToCollection(UserRecord user, Guid feedReference, AddFeedToCollectionRequest request);
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
                    return Result<AddFeedSourceResponse>.Error(feedDetailsResult.ErrorMessage);

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
                Collection = null
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

            var userFeedSource = unitOfWork.UserFeedSources.GetByUserAndFeedSourceReference(user, reference);
            if (userFeedSource == null)
                return Result.Error("Sorry, unable to delete feed source as it could not be found.");

            unitOfWork.UserFeedSources.Delete(userFeedSource);

            unitOfWork.Commit();

            return Result.Success(HttpStatusCode.NoContent);
        }

        public Result AddFeedToCollection(UserRecord user, Guid feedReference, AddFeedToCollectionRequest request)
        {
            var unitOfWork = _mightyUnitOfWorkFactory.Create();

            var userFeedSource = unitOfWork.UserFeedSources.GetByUserAndFeedSourceReference(user, feedReference);
            if (userFeedSource == null)
                return Result.Error("The feed source with the given reference could not be found for you.", HttpStatusCode.NotFound);

            userFeedSource.Collection = request.Collection;

            unitOfWork.UserFeedSources.Update(userFeedSource);

            unitOfWork.Commit();

            return Result.Success();
        }
    }
}