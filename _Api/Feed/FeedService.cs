using MightyRSS._Api.Feed.Types;
using MightyRSS.Data.Records;
using MightyRSS.Data.Repositories;
using System;
using System.Collections.Generic;
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
        private readonly TimeSpan _feedRefreshPeriod = TimeSpan.FromSeconds(20);

        private readonly IFeedSourceRepository _feedSourceRepository;
        private readonly IFeedReaderService _feedReaderService;
        private readonly IUserDataFeedSourceRepository _userDataFeedSourceRepository;

        public FeedService(
            IFeedSourceRepository feedSourceRepository,
            IFeedReaderService feedReaderService,
            IUserDataFeedSourceRepository userDataFeedSourceRepository)
        {
            _feedSourceRepository = feedSourceRepository;
            _feedReaderService = feedReaderService;
            _userDataFeedSourceRepository = userDataFeedSourceRepository;
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
            var feedSources = _userDataFeedSourceRepository.GetFeedSources(user);

            var responseFeedSources = new List<GetFeedResponse.FeedSource>(feedSources.Count);
            foreach (var feedSource in feedSources)
            {
                if (DateTime.Now - feedSource.ArticlesUpdatedAt > _feedRefreshPeriod)
                {
                    var feedReaderResult = _feedReaderService.Read(feedSource.RssUrl, feedSource.Reference);

                    var updatedFeedSource = new FeedSourceRecord
                    {
                        Id = feedSource.Id,
                        Reference = feedReaderResult.Reference,
                        Title = feedReaderResult.Title,
                        Description = feedReaderResult.Description,
                        RssUrl = feedReaderResult.RssUrl,
                        WebsiteUrl = feedReaderResult.WebsiteUrl,
                        Articles = feedReaderResult.Articles.ConvertAll(x => new FeedSourceArticleJsonb
                        {
                            Url = x.Url,
                            Title = x.Title,
                            Summary = x.Summary,
                            PublishedAt = x.PublishedAt,
                            PublishedAtAsString = x.PublishedAtAsString,
                            Author = x.Author
                        }),
                        ArticlesUpdatedAt = DateTime.Now.ToLocalTime()
                    };

                    _feedSourceRepository.Update(updatedFeedSource);

                    responseFeedSources.Add(new GetFeedResponse.FeedSource
                    {
                        Reference = updatedFeedSource.Reference,
                        Title = updatedFeedSource.Title,
                        Description = updatedFeedSource.Description,
                        RssUrl = updatedFeedSource.RssUrl,
                        WebsiteUrl = updatedFeedSource.WebsiteUrl,
                        Articles = updatedFeedSource.Articles.ConvertAll(article => new GetFeedResponse.FeedArticle
                        {
                            Url = article.Url,
                            Title = article.Title,
                            Summary = article.Summary,
                            PublishedAt = article.PublishedAt,
                            PublishedAtAsString = article.PublishedAtAsString,
                            Author = article.Author
                        })
                    });

                    continue;
                }

                responseFeedSources.Add(new GetFeedResponse.FeedSource
                {
                    Reference = feedSource.Reference,
                    Title = feedSource.Title,
                    Description = feedSource.Description,
                    RssUrl = feedSource.RssUrl,
                    WebsiteUrl = feedSource.WebsiteUrl,
                    Articles = feedSource.Articles.ConvertAll(article => new GetFeedResponse.FeedArticle
                    {
                        Url = article.Url,
                        Title = article.Title,
                        Summary = article.Summary,
                        PublishedAt = article.PublishedAt,
                        PublishedAtAsString = article.PublishedAtAsString,
                        Author = article.Author
                    })
                });
            }

            return new GetFeedResponse
            {
                Sources = responseFeedSources
            };
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