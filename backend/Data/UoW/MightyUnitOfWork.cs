using Data.Repositories;

namespace Data.UoW;

public interface IMightyUnitOfWork : IUnitOfWork
{
    public IFeedSourceRepository FeedSources { get; }
    public IUserFeedSourceRepository UserFeedSources { get; }
    public IUserRepository Users { get; }
    public ICollectionRepository Collections { get; }
}

public sealed class MightyUnitOfWork : UnitOfWork, IMightyUnitOfWork
{
    public IFeedSourceRepository FeedSources { get; }
    public IUserFeedSourceRepository UserFeedSources { get; }
    public IUserRepository Users { get; }
    public ICollectionRepository Collections { get; }

    public MightyUnitOfWork(IApiDatabase database, CancellationToken cancellationToken) : base(database, cancellationToken)
    {
        FeedSources = new FeedSourceRepository(Session, cancellationToken);
        UserFeedSources = new UserFeedSourceRepository(Session, cancellationToken);
        Users = new UserRepository(Session, cancellationToken);
        Collections = new CollectionRepository(Session, cancellationToken);
    }
}