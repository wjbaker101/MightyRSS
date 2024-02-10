using Data.Records;
using Data.UoW;
using NetApiLibs.Type;
using NHibernate;
using NHibernate.Linq;

namespace Data.Repositories;

public interface ICollectionRepository : IRepository<CollectionRecord>
{
    Task<Result<CollectionRecord>> GetByReference(Guid collectionReference);
    Task<List<CollectionRecord>> GetByUser(UserRecord user);
}

public sealed class CollectionRepository : Repository<CollectionRecord>, ICollectionRepository
{
    public CollectionRepository(ISession session, CancellationToken cancellationToken) : base(session, cancellationToken)
    {
    }

    public async Task<Result<CollectionRecord>> GetByReference(Guid collectionReference)
    {
        var collection = await Session
            .Query<CollectionRecord>()
            .SingleOrDefaultAsync(x => x.Reference == collectionReference, CancellationToken);

        if (collection == null)
            return Result<CollectionRecord>.Failure($"Unable to find collection with reference: {collectionReference}.");

        return collection;
    }

    public async Task<List<CollectionRecord>> GetByUser(UserRecord user)
    {
        return await Session
            .Query<CollectionRecord>()
            .Where(x => x.User == user)
            .ToListAsync(CancellationToken);
    }
}