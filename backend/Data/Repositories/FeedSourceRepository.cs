using Data.Records;
using Data.UoW;
using NetApiLibs.Type;
using NHibernate;

namespace Data.Repositories;

public interface IFeedSourceRepository : IRepository<FeedSourceRecord>
{
    Result<FeedSourceRecord> GetByReference(Guid reference);
    Result<FeedSourceRecord> GetByRssUrl(string url);
}

public sealed class FeedSourceRepository : Repository<FeedSourceRecord>, IFeedSourceRepository
{
    public FeedSourceRepository(ISession session) : base(session)
    {
    }

    public Result<FeedSourceRecord> GetByReference(Guid reference)
    {
        var feedSource = Session
            .Query<FeedSourceRecord>()
            .SingleOrDefault(x => x.Reference == reference);

        if (feedSource == null)
            return Result<FeedSourceRecord>.Failure($"Unable to find feed source with reference: {reference}.");

        return feedSource;
    }

    public Result<FeedSourceRecord> GetByRssUrl(string url)
    {
        var feedSource = Session
            .Query<FeedSourceRecord>()
            .SingleOrDefault(x => x.RssUrl.ToLower() == url.ToLower());

        if (feedSource == null)
            return Result<FeedSourceRecord>.Failure($"Unable to find feed source with url: {url}.");

        return feedSource;
    }
}