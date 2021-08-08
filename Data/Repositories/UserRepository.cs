using MightyRSS.Data.Records;
using NHibernate;
using System;
using System.Linq;
using WJBCommon.Lib.Data;

namespace MightyRSS.Data.Repositories
{
    public interface IUserRepository : IRepository<UserRecord>
    {
        UserRecord GetByReference(Guid reference);
        UserRecord GetByUsername(string username);
    }

    public sealed class UserRepository : Repository<UserRecord>, IUserRepository
    {
        public UserRepository(ISession session) : base(session)
        {
        }

        public UserRecord GetByReference(Guid reference)
        {
            return Session
                .Query<UserRecord>()
                .SingleOrDefault(x => x.Reference == reference);
        }

        public UserRecord GetByUsername(string username)
        {
            return Session
                .Query<UserRecord>()
                .SingleOrDefault(x => x.Username.ToLower() == username.ToLower());
        }
    }
}