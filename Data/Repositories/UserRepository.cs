using MightyRSS.Data.Records;
using System;
using System.Data;
using System.Linq;
using WJBCommon.Lib.Data;

namespace MightyRSS.Data.Repositories
{
    public interface IUserRepository
    {
        UserRecord GetByReference(Guid reference);
        UserRecord GetByUsername(string username);
        UserRecord Save(UserRecord user);
    }

    public sealed class UserRepository : IUserRepository
    {
        private readonly IApiDatabase _database;

        public UserRepository(IApiDatabase database)
        {
            _database = database;
        }

        public UserRecord GetByReference(Guid reference)
        {
            using var session = _database.SessionFactory().OpenSession();
            using var transaction = session.BeginTransaction(IsolationLevel.ReadCommitted);

            var user = session
                .Query<UserRecord>()
                .SingleOrDefault(x => x.Reference == reference);

            transaction.Commit();

            return user;
        }

        public UserRecord GetByUsername(string username)
        {
            using var session = _database.SessionFactory().OpenSession();
            using var transaction = session.BeginTransaction(IsolationLevel.ReadCommitted);

            var user = session
                .Query<UserRecord>()
                .SingleOrDefault(x => x.Username.ToLower() == username.ToLower());

            transaction.Commit();

            return user;
        }

        public UserRecord Save(UserRecord user)
        {
            using var session = _database.SessionFactory().OpenSession();
            using var transaction = session.BeginTransaction(IsolationLevel.ReadCommitted);

            session.Save(user);

            transaction.Commit();

            return user;
        }
    }
}