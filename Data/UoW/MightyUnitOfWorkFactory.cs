using WJBCommon.Lib.Data;

namespace MightyRSS.Data.UoW
{
    public sealed class MightyUnitOfWorkFactory : IUnitOfWorkFactory<IMightyUnitOfWork>
    {
        private readonly IApiDatabase _database;

        public MightyUnitOfWorkFactory(IApiDatabase database)
        {
            _database = database;
        }

        public IMightyUnitOfWork Create()
        {
            return new MightyUnitOfWork(_database);
        }
    }
}