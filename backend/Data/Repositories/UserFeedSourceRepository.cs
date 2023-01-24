using Data.Records;
using Data.UoW;
using NetApiLibs.Type;
using NHibernate;
using NHibernate.Linq;
using System.Net;

namespace Data.Repositories;

public interface IUserFeedSourceRepository : IRepository<UserFeedSourceRecord>
{
    List<UserFeedSourceRecord> GetAll();
    Result<UserFeedSourceRecord> GetByUserAndFeedSourceReference(UserRecord user, Guid reference);
    List<UserFeedSourceRecord> GetFeedSources(UserRecord user);
}

public sealed class UserFeedSourceRepository : Repository<UserFeedSourceRecord>, IUserFeedSourceRepository
{
    public UserFeedSourceRepository(ISession session) : base(session)
    {
    }

    public List<UserFeedSourceRecord> GetAll()
    {
        return Session
            .Query<UserFeedSourceRecord>()
            .Fetch(x => x.FeedSource)
            .ToList();
    }

    public Result<UserFeedSourceRecord> GetByUserAndFeedSourceReference(UserRecord user, Guid reference)
    {
        var userFeedSource = Session
            .Query<UserFeedSourceRecord>()
            .SingleOrDefault(x => x.User == user && x.FeedSource.Reference == reference);

        if (userFeedSource == null)
            return Result<UserFeedSourceRecord>.Failure("The feed source could not be found in your feed.", HttpStatusCode.NotFound);

        return userFeedSource;
    }

    public List<UserFeedSourceRecord> GetFeedSources(UserRecord user)
    {
        return Session
            .Query<UserFeedSourceRecord>()
            .Fetch(x => x.FeedSource)
            .Where(x => x.User == user)
            .ToList();
    }
}