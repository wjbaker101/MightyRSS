using MightyRSS.Data.Records;
using MightyRSS.Data.UoW;
using NHibernate;
using System;
using System.Linq;

namespace MightyRSS.Data.Repositories;

public interface IFeedSourceRepository : IRepository<FeedSourceRecord>
{
    FeedSourceRecord GetByReference(Guid reference);
    FeedSourceRecord GetByRssUrl(string url);
}

public sealed class FeedSourceRepository : Repository<FeedSourceRecord>, IFeedSourceRepository
{
    public FeedSourceRepository(ISession session) : base(session)
    {
    }

    public FeedSourceRecord GetByReference(Guid reference)
    {
        return Session
            .Query<FeedSourceRecord>()
            .SingleOrDefault(x => x.Reference == reference);
    }

    public FeedSourceRecord GetByRssUrl(string url)
    {
        return Session
            .Query<FeedSourceRecord>()
            .SingleOrDefault(x => x.RssUrl.ToLower() == url.ToLower());
    }
}