using Data.Records;
using Data.UoW;
using NetApiLibs.Type;
using NHibernate;
using NHibernate.Linq;

namespace Data.Repositories;

public interface IFeedSourceRepository : IRepository<FeedSourceRecord>
{
    Task<Result<FeedSourceRecord>> GetByReference(Guid reference);
    Task<Result<FeedSourceRecord>> GetByRssUrl(string url);
}

public sealed class FeedSourceRepository : Repository<FeedSourceRecord>, IFeedSourceRepository
{
    public FeedSourceRepository(ISession session, CancellationToken cancellationToken) : base(session, cancellationToken)
    {
    }

    public async Task<Result<FeedSourceRecord>> GetByReference(Guid reference)
    {
        var feedSource = await Session
            .Query<FeedSourceRecord>()
            .SingleOrDefaultAsync(x => x.Reference == reference, CancellationToken);

        if (feedSource == null)
            return Result<FeedSourceRecord>.Failure($"Unable to find feed source with reference: {reference}.");

        return feedSource;
    }

    public async Task<Result<FeedSourceRecord>> GetByRssUrl(string url)
    {
        var feedSource = await Session
            .Query<FeedSourceRecord>()
            .SingleOrDefaultAsync(x => x.RssUrl.ToLower() == url.ToLower(), CancellationToken);

        if (feedSource == null)
            return Result<FeedSourceRecord>.Failure($"Unable to find feed source with url: {url}.");

        return feedSource;
    }
}