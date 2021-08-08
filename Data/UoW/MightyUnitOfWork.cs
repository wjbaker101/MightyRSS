using MightyRSS.Data.Repositories;
using WJBCommon.Lib.Data;

namespace MightyRSS.Data.UoW
{
    public interface IMightyUnitOfWork : IUnitOfWork
    {
        public IFeedSourceRepositoryV2 FeedSources { get; }
    }

    public sealed class MightyUnitOfWork : UnitOfWork, IMightyUnitOfWork
    {
        public IFeedSourceRepositoryV2 FeedSources { get; }

        public MightyUnitOfWork(IApiDatabase database) : base(database)
        {
            FeedSources = new FeedSourceRepositoryV2(Session);
        }
    }
}