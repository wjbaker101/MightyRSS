using MightyRSS.Data.Records;
using NHibernate;
using System;
using System.Linq;
using WJBCommon.Lib.Data;

namespace MightyRSS.Data.Repositories
{
    public interface IFeedSourceRepositoryV2 : IRepository<FeedSourceRecord>
    {
        FeedSourceRecord GetByReference(Guid reference);
        FeedSourceRecord GetByRssUrl(string url);
    }

    public sealed class FeedSourceRepositoryV2 : Repository<FeedSourceRecord>, IFeedSourceRepositoryV2
    {
        public FeedSourceRepositoryV2(ISession session) : base(session)
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
}