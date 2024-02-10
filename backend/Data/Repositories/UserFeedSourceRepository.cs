using Data.Records;
using Data.UoW;
using NetApiLibs.Type;
using NHibernate;
using NHibernate.Linq;
using System.Net;

namespace Data.Repositories;

public interface IUserFeedSourceRepository : IRepository<UserFeedSourceRecord>
{
    Task<List<UserFeedSourceRecord>> GetAll();
    Task<Result<UserFeedSourceRecord>> GetByUserAndFeedSourceReference(UserRecord user, Guid reference);
    Task<List<UserFeedSourceRecord>> GetFeedSources(UserRecord user);
}

public sealed class UserFeedSourceRepository : Repository<UserFeedSourceRecord>, IUserFeedSourceRepository
{
    public UserFeedSourceRepository(ISession session, CancellationToken cancellationToken) : base(session, cancellationToken)
    {
    }

    public async Task<List<UserFeedSourceRecord>> GetAll()
    {
        return await Session
            .Query<UserFeedSourceRecord>()
            .Fetch(x => x.FeedSource)
            .ToListAsync(CancellationToken);
    }

    public async Task<Result<UserFeedSourceRecord>> GetByUserAndFeedSourceReference(UserRecord user, Guid reference)
    {
        var userFeedSource = await Session
            .Query<UserFeedSourceRecord>()
            .SingleOrDefaultAsync(x => x.User == user && x.FeedSource.Reference == reference, CancellationToken);

        if (userFeedSource == null)
            return Result<UserFeedSourceRecord>.Failure("The feed source could not be found in your feed.", HttpStatusCode.NotFound);

        return userFeedSource;
    }

    public async Task<List<UserFeedSourceRecord>> GetFeedSources(UserRecord user)
    {
        return await Session
            .Query<UserFeedSourceRecord>()
            .Fetch(x => x.FeedSource)
            .Where(x => x.User == user)
            .ToListAsync(CancellationToken);
    }
}