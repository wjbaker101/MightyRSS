using Microsoft.Extensions.Options;
using MightyRSS._Api.Feed.Types;
using MightyRSS.Data.Records;
using MightyRSS.Data.Repositories;
using System;
using System.Linq;

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
        private readonly IFeedSourceRepository _feedSourceRepository;
        private readonly IFeedReaderService _feedReaderService;
        private readonly IUserDataFeedSourceRepository _userDataFeedSourceRepository;

        private readonly TimeSpan _feedRefreshPeriod;

        public FeedService(
            IOptions<FeedSettings> feedSettings,
            IFeedSourceRepository feedSourceRepository,
            IFeedReaderService feedReaderService,
            IUserDataFeedSourceRepository userDataFeedSourceRepository)
        {
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

                feedSource = _feedSourceRepository.Save(new FeedSourceRecord
                {
                    Reference = feedReaderResult.Reference,
                    Title = feedReaderResult.Title,
                    Description = feedReaderResult.Description,
                    RssUrl = feedReaderResult.RssUrl,
                    WebsiteUrl = feedReaderResult.WebsiteUrl,
                    Articles = feedReaderResult.Articles
                        .Select(x => new FeedSourceArticleJsonb
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

            _userDataFeedSourceRepository.Save(new UserDataFeedSourceRecord
            {
                User = user,
                FeedSource = feedSource
            });

            return new AddFeedSourceResponse
            {
                Reference = feedSource.Reference,
                Title = feedSource.Title,
                Description = feedSource.Description,
                RssUrl = feedSource.RssUrl,
                WebsiteUrl = feedSource.WebsiteUrl,
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
                    Reference = source.Reference,
                    Title = source.Title,
                    Description = source.Description,
                    RssUrl = source.RssUrl,
                    WebsiteUrl = source.WebsiteUrl,
                    Articles = source.Articles.ConvertAll(article => new GetFeedResponse.FeedArticle
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
                if (feedSource.ArticlesUpdatedAt + _feedRefreshPeriod > DateTime.Now)
                    continue;

                UpdateFeedSource(feedSource);
            }
        }

        private void UpdateFeedSource(FeedSourceRecord feedSource)
        {
            var feed = _feedReaderService.Read(feedSource.RssUrl, feedSource.Reference);

            _feedSourceRepository.Update(new FeedSourceRecord
            {
                Id = feedSource.Id,
                Reference = feed.Reference,
                Title = feed.Title,
                Description = feed.Description,
                RssUrl = feed.RssUrl,
                WebsiteUrl = feed.WebsiteUrl,
                Articles = feed.Articles.ConvertAll(x => new FeedSourceArticleJsonb
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
            var userDataFeedSource = _userDataFeedSourceRepository.GetByUserAndFeedSourceReference(user, reference);
            if (userDataFeedSource == null)
                return;

            _userDataFeedSourceRepository.Delete(userDataFeedSource);
        }
    }
}