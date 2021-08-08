using Microsoft.Extensions.Options;
using MightyRSS._Api.Feed.Types;
using MightyRSS.Data.Records;
using MightyRSS.Data.Repositories;
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
        private readonly IFeedSourceRepository _feedSourceRepository;
        private readonly IFeedReaderService _feedReaderService;
        private readonly IUserDataFeedSourceRepository _userDataFeedSourceRepository;

        private readonly TimeSpan _feedRefreshPeriod;

        public FeedService(
            IOptions<FeedSettings> feedSettings,
            IUnitOfWorkFactory<IMightyUnitOfWork> mightyUnitOfWorkFactory,
            IFeedSourceRepository feedSourceRepository,
            IFeedReaderService feedReaderService,
            IUserDataFeedSourceRepository userDataFeedSourceRepository)
        {
            _mightyUnitOfWorkFactory = mightyUnitOfWorkFactory;
            _feedSourceRepository = feedSourceRepository;
            _feedReaderService = feedReaderService;
            _userDataFeedSourceRepository = userDataFeedSourceRepository;

            _feedRefreshPeriod = TimeSpan.FromSeconds(feedSettings.Value.RefreshPeriod);
        }

        public AddFeedSourceResponse AddFeedSource(UserRecord user, AddFeedSourceRequest request)
        {
            var feedSource = _feedSourceRepository.GetByRssUrl(request.Url);
            if (feedSource == null)
            {
                var feedReaderResult = _feedReaderService.Read(request.Url, null);
                if (feedReaderResult == null)
                    return null;

                feedSource = _feedSourceRepository.Save(new FeedSourceRecord
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
                });
            }

            var userDataFeedSource = _userDataFeedSourceRepository.Save(new UserDataFeedSourceRecord
            {
                User = user,
                FeedSource = feedSource,
                Collection = null
            });

            return new AddFeedSourceResponse
            {
                Reference = feedSource.Reference,
                Title = feedSource.Title,
                Description = feedSource.Description,
                RssUrl = feedSource.RssUrl,
                WebsiteUrl = feedSource.WebsiteUrl,
                Collection = userDataFeedSource.Collection,
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
            UpdateFeedSources(user);

            var feedSources = _userDataFeedSourceRepository.GetFeedSources(user);

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

        private void UpdateFeedSources(UserRecord user)
        {
            var feedSources = _userDataFeedSourceRepository.GetFeedSources(user);

            foreach (var feedSource in feedSources)
            {
                if (feedSource.FeedSource.ArticlesUpdatedAt + _feedRefreshPeriod > DateTime.Now)
                    continue;

                UpdateFeedSource(feedSource.FeedSource);
            }
        }

        private void UpdateFeedSource(FeedSourceRecord feedSource)
        {
            var feed = _feedReaderService.Read(feedSource.RssUrl, feedSource.Reference);
            if (feed == null)
                return;

            _feedSourceRepository.Update(new FeedSourceRecord
            {
                Id = feedSource.Id,
                Reference = feed.Reference,
                Title = feed.Title,
                Description = feed.Description,
                RssUrl = feed.RssUrl,
                WebsiteUrl = feed.WebsiteUrl,
                Articles = feed.Articles.ConvertAll(x => new FeedSourceRecord.Article
                {
                    Url = x.Url,
                    Title = x.Title,
                    Summary = x.Summary,
                    PublishedAt = x.PublishedAt,
                    PublishedAtAsString = x.PublishedAtAsString,
                    Author = x.Author
                }),
                ArticlesUpdatedAt = DateTime.Now.ToLocalTime()
            });
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