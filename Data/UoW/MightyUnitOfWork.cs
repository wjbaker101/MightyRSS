using MightyRSS.Data.Repositories;
using WJBCommon.Lib.Data;

namespace MightyRSS.Data.UoW
{
    public interface IMightyUnitOfWork : IUnitOfWork
    {
        public IFeedSourceRepositoryV2 FeedSources { get; }

        public IUserFeedSourceRepositoryV2 UserFeedSources { get; }

        public IUserRepositoryV2 Users { get; }
    }

    public sealed class MightyUnitOfWork : UnitOfWork, IMightyUnitOfWork
    {
        public IFeedSourceRepositoryV2 FeedSources { get; }

        public IUserFeedSourceRepositoryV2 UserFeedSources { get; }

        public IUserRepositoryV2 Users { get; }

        public MightyUnitOfWork(IApiDatabase database) : base(database)
        {
            FeedSources = new FeedSourceRepositoryV2(Session);

            UserFeedSources = new UserFeedSourceRepositoryV2(Session);

            Users = new UserRepositoryV2(Session);
        }
    }
}