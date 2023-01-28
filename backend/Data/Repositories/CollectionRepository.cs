using Data.Records;
using Data.UoW;
using NetApiLibs.Type;
using NHibernate;

namespace Data.Repositories;

public interface ICollectionRepository : IRepository<CollectionRecord>
{
    Result<CollectionRecord> GetByReference(Guid collectionReference);
    List<CollectionRecord> GetByUser(UserRecord user);
}

public sealed class CollectionRepository : Repository<CollectionRecord>, ICollectionRepository
{
    public CollectionRepository(ISession session) : base(session)
    {
    }

    public Result<CollectionRecord> GetByReference(Guid collectionReference)
    {
        var collection = Session
            .Query<CollectionRecord>()
            .SingleOrDefault(x => x.Reference == collectionReference);

        if (collection == null)
            return Result<CollectionRecord>.Failure($"Unable to find collection with reference: {collectionReference}.");

        return collection;
    }

    public List<CollectionRecord> GetByUser(UserRecord user)
    {
        return Session
            .Query<CollectionRecord>()
            .Where(x => x.User == user)
            .ToList();
    }
}