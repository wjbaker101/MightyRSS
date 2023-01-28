using Data.Records;
using Data.UoW;
using NHibernate;

namespace Data.Repositories;

public interface ICollectionRepository : IRepository<FeedSourceRecord>
{
}

public sealed class CollectionRepository : Repository<FeedSourceRecord>, ICollectionRepository
{
    public CollectionRepository(ISession session) : base(session)
    {
    }
}