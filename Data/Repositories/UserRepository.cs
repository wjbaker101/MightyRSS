using Data.Records;
using Data.UoW;
using NetApiLibs.Type;
using NHibernate;

namespace Data.Repositories;

public interface IUserRepository : IRepository<UserRecord>
{
    Result<UserRecord> GetByReference(Guid reference);
    Result<UserRecord> GetByUsername(string username);
}

public sealed class UserRepository : Repository<UserRecord>, IUserRepository
{
    public UserRepository(ISession session) : base(session)
    {
    }

    public Result<UserRecord> GetByReference(Guid reference)
    {
        var user = Session
            .Query<UserRecord>()
            .SingleOrDefault(x => x.Reference == reference);

        if (user == null)
            return Result<UserRecord>.Failure($"Unable to find user with reference: {reference}.");

        return user;
    }

    public Result<UserRecord> GetByUsername(string username)
    {
        var user = Session
            .Query<UserRecord>()
            .SingleOrDefault(x => x.Username.ToLower() == username.ToLower());

        if (user == null)
            return Result<UserRecord>.Failure($"Unable to find user with username: {username}.");

        return user;
    }
}