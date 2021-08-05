using MightyRSS.Data.Records;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MightyRSS.Data.Repositories
{
    public interface IUserDataFeedSourceRepository
    {
        UserDataFeedSourceRecord GetByUserAndFeedSourceReference(UserRecord user, Guid feedSourceReference);
        List<UserDataFeedSourceRecord> GetFeedSources(UserRecord user);
        UserDataFeedSourceRecord Save(UserDataFeedSourceRecord userDataFeedSource);
        void Delete(UserDataFeedSourceRecord userDataFeedSource);
    }

    public sealed class UserDataFeedSourceRepository : IUserDataFeedSourceRepository
    {
        private readonly IApiDatabase _database;

        public UserDataFeedSourceRepository(IApiDatabase database)
        {
            _database = database;
        }

        public UserDataFeedSourceRecord GetByUserAndFeedSourceReference(UserRecord user, Guid feedSourceReference)
        {
            using var session = _database.SessionFactory.OpenSession();
            using var transaction = session.BeginTransaction(IsolationLevel.ReadCommitted);

            var userDataFeedSource = session
                .Query<UserDataFeedSourceRecord>()
                .SingleOrDefault(x => x.User == user && x.FeedSource.Reference == feedSourceReference);

            transaction.Commit();

            return userDataFeedSource;
        }

        public List<UserDataFeedSourceRecord> GetFeedSources(UserRecord user)
        {
            using var session = _database.SessionFactory.OpenSession();
            using var transaction = session.BeginTransaction(IsolationLevel.ReadCommitted);

            var feedSources = session
                .Query<UserDataFeedSourceRecord>()
                .Fetch(x => x.FeedSource)
                .Where(x => x.User == user)
                .ToList();

            transaction.Commit();

            return feedSources;
        }

        public UserDataFeedSourceRecord Save(UserDataFeedSourceRecord userDataFeedSource)
        {
            using var session = _database.SessionFactory.OpenSession();
            using var transaction = session.BeginTransaction(IsolationLevel.ReadCommitted);

            session.Save(userDataFeedSource);

            transaction.Commit();

            return userDataFeedSource;
        }

        public void Delete(UserDataFeedSourceRecord userDataFeedSource)
        {
            using var session = _database.SessionFactory.OpenSession();
            using var transaction = session.BeginTransaction(IsolationLevel.ReadCommitted);

            session.Delete(userDataFeedSource);

            transaction.Commit();
        }
    }
}