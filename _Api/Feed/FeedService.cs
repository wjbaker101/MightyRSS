using Microsoft.Extensions.Options;
using MightyRSS._Api.Feed.Types;
using MightyRSS.Data.Records;
using MightyRSS.Data.UoW;
using MightyRSS.Settings;
using System;
using System.Linq;
using WJBCommon.Lib.Data;

namespace MightyRSS._Api.Feed
{
    public interface IFeedService
    {
        AddFeedSourceResponse AddFeedSource(UserRecord user, AddFeedSourceRequest request);
        GetFeedResponse GetFeed(UserRecord user);
        void DeleteFeedSource(UserRecord user, Guid reference);
    }

    public sealed class FeedService: IFeedService
    {
        private readonly IUnitOfWorkFactory<IMightyUnitOfWork> _mightyUnitOfWorkFactory;
        private readonly IFeedReaderService _feedReaderService;

        private readonly TimeSpan _feedRefreshPeriod;

        public FeedService(
            IOptions<FeedSettings> feedSettings,
            IUnitOfWorkFactory<IMightyUnitOfWork> mightyUnitOfWorkFactory,
            IFeedReaderService feedReaderService)
        {
            _mightyUnitOfWorkFactory = mightyUnitOfWorkFactory;
            _feedReaderService = feedReaderService;

            _feedRefreshPeriod = TimeSpan.FromSeconds(feedSettings.Value.RefreshPeriod);
        }

        public AddFeedSourceResponse AddFeedSource(UserRecord user, AddFeedSourceRequest request)
        {
            using var unitOfWork = _mightyUnitOfWorkFactory.Create();

            var feedSource = unitOfWork.FeedSources.GetByRssUrl(request.Url);
            if (feedSource == null)
            {
                var feedReaderResult = _feedReaderService.Read(request.Url, null);
                if (feedReaderResult == null)
                    return null;

                feedSource = new FeedSourceRecord
                {
                    Reference = feedReaderResult.Reference,
                    Title = feedReaderResult.Title,
                    Description = feedReaderResult.Description,
                    RssUrl = feedReaderResult.RssUrl,
                    WebsiteUrl = feedReaderResult.WebsiteUrl,
                    Articles = feedReaderResult.Articles
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

            return new AddFeedSourceResponse
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
            };
        }

        public GetFeedResponse GetFeed(UserRecord user)
        {
            using var unitOfWork = _mightyUnitOfWorkFactory.Create();

            UpdateFeedSources(unitOfWork, user);

            var feedSources = unitOfWork.UserFeedSources.GetFeedSources(user);

            unitOfWork.Commit();

            return new GetFeedResponse
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
            };
        }

        private void UpdateFeedSources(IMightyUnitOfWork unitOfWork, UserRecord user)
        {
            var feedSources = unitOfWork.UserFeedSources.GetFeedSources(user);

            foreach (var feedSource in feedSources)
            {
                if (feedSource.FeedSource.ArticlesUpdatedAt + _feedRefreshPeriod > DateTime.Now)
                    continue;

                UpdateFeedSource(unitOfWork, feedSource.FeedSource);
            }
        }

        private void UpdateFeedSource(IMightyUnitOfWork unitOfWork, FeedSourceRecord feedSource)
        {
            var feed = _feedReaderService.Read(feedSource.RssUrl, feedSource.Reference);
            if (feed == null)
                return;

            feedSource.Title = feed.Title;
            feedSource.Description = feed.Description;
            feedSource.RssUrl = feed.RssUrl;
            feedSource.WebsiteUrl = feed.WebsiteUrl;
            feedSource.Articles = feed.Articles.ConvertAll(x => new FeedSourceRecord.Article
            {
                Url = x.Url,
                Title = x.Title,
                Summary = x.Summary,
                PublishedAt = x.PublishedAt,
                PublishedAtAsString = x.PublishedAtAsString,
                Author = x.Author
            });
            feedSource.ArticlesUpdatedAt = DateTime.Now.ToLocalTime();

            unitOfWork.FeedSources.Update(feedSource);
        }

        public void DeleteFeedSource(UserRecord user, Guid reference)
        {
            using var unitOfWork = _mightyUnitOfWorkFactory.Create();

            var userFeedSource = unitOfWork.UserFeedSources.GetByUserAndFeedSourceReference(user, reference);
            if (userFeedSource == null)
                return;

            unitOfWork.UserFeedSources.Delete(userFeedSource);

            unitOfWork.Commit();
        }
    }
}