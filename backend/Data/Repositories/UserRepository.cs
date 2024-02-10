using Data.Records;
using Data.UoW;
using NetApiLibs.Type;
using NHibernate;
using NHibernate.Linq;

namespace Data.Repositories;

public interface IUserRepository : IRepository<UserRecord>
{
    Task<Result<UserRecord>> GetByReference(Guid reference);
    Task<Result<UserRecord>> GetByUsername(string username);
}

public sealed class UserRepository : Repository<UserRecord>, IUserRepository
{
    public UserRepository(ISession session, CancellationToken cancellationToken) : base(session, cancellationToken)
    {
    }

    public async Task<Result<UserRecord>> GetByReference(Guid reference)
    {
        var user = await Session
            .Query<UserRecord>()
            .SingleOrDefaultAsync(x => x.Reference == reference, CancellationToken);

        if (user == null)
            return Result<UserRecord>.Failure($"Unable to find user with reference: {reference}.");

        return user;
    }

    public async Task<Result<UserRecord>> GetByUsername(string username)
    {
        var user = await Session
            .Query<UserRecord>()
            .SingleOrDefaultAsync(x => x.Username.ToLower() == username.ToLower(), CancellationToken);

        if (user == null)
            return Result<UserRecord>.Failure($"Unable to find user with username: {username}.");

        return user;
    }
}