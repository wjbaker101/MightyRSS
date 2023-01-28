using Data.Records;
using Data.UoW;
using NetApiLibs.Type;
using NHibernate;

namespace Data.Repositories;

public interface ICollectionRepository : IRepository<FeedSourceRecord>
{
    Result<CollectionRecord> GetByReference(Guid collectionReference);
}

public sealed class CollectionRepository : Repository<FeedSourceRecord>, ICollectionRepository
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
}