using Data.Repositories;

namespace Data.UoW;

public interface IMightyUnitOfWork : IUnitOfWork
{
    public IFeedSourceRepository FeedSources { get; }

    public IUserFeedSourceRepository UserFeedSources { get; }

    public IUserRepository Users { get; }
}

public sealed class MightyUnitOfWork : UnitOfWork, IMightyUnitOfWork
{
    public IFeedSourceRepository FeedSources { get; }

    public IUserFeedSourceRepository UserFeedSources { get; }

    public IUserRepository Users { get; }

    public MightyUnitOfWork(IApiDatabase database) : base(database)
    {
        FeedSources = new FeedSourceRepository(Session);

        UserFeedSources = new UserFeedSourceRepository(Session);

        Users = new UserRepository(Session);
    }
}