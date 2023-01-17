using Data.Records;
using Data.UoW;
using NetApiLibs.Type;
using NHibernate;
using NHibernate.Linq;
using System.Net;

namespace Data.Repositories;

public interface IUserFeedSourceRepository : IRepository<UserDataFeedSourceRecord>
{
    List<UserDataFeedSourceRecord> GetAll();
    Result<UserDataFeedSourceRecord> GetByUserAndFeedSourceReference(UserRecord user, Guid reference);
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

    public Result<UserDataFeedSourceRecord> GetByUserAndFeedSourceReference(UserRecord user, Guid reference)
    {
        var userFeedSource = Session
            .Query<UserDataFeedSourceRecord>()
            .SingleOrDefault(x => x.User == user && x.FeedSource.Reference == reference);

        if (userFeedSource == null)
            return Result<UserDataFeedSourceRecord>.Failure("The feed source could not be found in your feed.", HttpStatusCode.NotFound);

        return userFeedSource;
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