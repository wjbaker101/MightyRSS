using MightyRSS.Data.Records;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using WJBCommon.Lib.Data;

namespace MightyRSS.Data.Repositories
{
    public interface IUserFeedSourceRepositoryV2 : IRepository<UserDataFeedSourceRecord>
    {
        UserDataFeedSourceRecord GetByUserAndFeedSourceReference(UserRecord user, Guid feedSourceReference);
        List<UserDataFeedSourceRecord> GetFeedSources(UserRecord user);
    }

    public sealed class UserFeedSourceRepositoryV2 : Repository<UserDataFeedSourceRecord>, IUserFeedSourceRepositoryV2
    {
        public UserFeedSourceRepositoryV2(ISession session) : base(session)
        {
        }

        public UserDataFeedSourceRecord GetByUserAndFeedSourceReference(UserRecord user, Guid feedSourceReference)
        {
            return Session
                .Query<UserDataFeedSourceRecord>()
                .SingleOrDefault(x => x.User == user && x.FeedSource.Reference == feedSourceReference);
        }

        public List<UserDataFeedSourceRecord> GetFeedSources(UserRecord user)
        {
            return Session
                .Query<UserDataFeedSourceRecord>()
                .Fetch(x => x.FeedSource)
                .Where(x => x.User == user)
                .ToList();
        }
    }
}