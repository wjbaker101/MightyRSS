using MightyRSS.Data.Records;
using System;
using System.Data;
using System.Linq;

namespace MightyRSS.Data.Repositories
{
    public interface IFeedSourceRepository
    {
        FeedSourceRecord GetByReference(Guid reference);
        FeedSourceRecord GetByRssUrl(string url);
        FeedSourceRecord Save(FeedSourceRecord feedSource);
        FeedSourceRecord Update(FeedSourceRecord feedSource);
    }

    public sealed class FeedSourceRepository : IFeedSourceRepository
    {
        private readonly IApiDatabase _database;

        public FeedSourceRepository(IApiDatabase database)
        {
            _database = database;
        }

        public FeedSourceRecord GetByReference(Guid reference)
        {
            using var session = _database.SessionFactory.OpenSession();
            using var transaction = session.BeginTransaction(IsolationLevel.ReadCommitted);

            var feedSource = session
                .Query<FeedSourceRecord>()
                .SingleOrDefault(x => x.Reference == reference);

            transaction.Commit();

            return feedSource;
        }

        public FeedSourceRecord GetByRssUrl(string url)
        {
            using var session = _database.SessionFactory.OpenSession();
            using var transaction = session.BeginTransaction(IsolationLevel.ReadCommitted);

            var feedSource = session
                .Query<FeedSourceRecord>()
                .SingleOrDefault(x => x.RssUrl.ToLower() == url.ToLower());

            transaction.Commit();

            return feedSource;
        }

        public FeedSourceRecord Save(FeedSourceRecord feedSource)
        {
            using var session = _database.SessionFactory.OpenSession();
            using var transaction = session.BeginTransaction(IsolationLevel.ReadCommitted);

            session.Save(feedSource);

            transaction.Commit();

            return feedSource;
        }

        public FeedSourceRecord Update(FeedSourceRecord feedSource)
        {
            using var session = _database.SessionFactory.OpenSession();
            using var transaction = session.BeginTransaction(IsolationLevel.ReadCommitted);

            session.Update(feedSource);

            transaction.Commit();

            return feedSource;
        }
    }
}