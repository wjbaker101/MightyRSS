using MightyRSS.Data.Records;
using MightyRSS.Data.UoW;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MightyRSS.Data.Repositories;

public interface IUserFeedSourceRepository : IRepository<UserDataFeedSourceRecord>
{
    List<UserDataFeedSourceRecord> GetAll();
    UserDataFeedSourceRecord GetByUserAndFeedSourceReference(UserRecord user, Guid feedSourceReference);
    List<UserDataFeedSourceRecord> GetFeedSources(UserRecord user);
}

public sealed class UserFeedSourceRepository : Repository<UserDataFeedSourceRecord>, IUserFeedSourceRepository
{
    public UserFeedSourceRepository(ISession session) : base(session)
    {
    }

    public List<UserDataFeedSourceRecord> GetAll()
    {
        return Session
            .Query<UserDataFeedSourceRecord>()
            .Fetch(x => x.FeedSource)
            .ToList();
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