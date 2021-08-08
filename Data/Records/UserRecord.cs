using FluentNHibernate.Mapping;
using System;
using WJBCommon.Lib.Data.Type;

namespace MightyRSS.Data.Records
{
    public class UserRecord : DatabaseRecord
    {
        public virtual Guid Reference { get; init; }
        public virtual string Username { get; init; }
        public virtual string Password { get; init; }
        public virtual Guid PasswordSalt { get; init; }
    }

    public sealed class UserRecordMap : ClassMap<UserRecord>
    {
        public UserRecordMap()
        {
            Schema("auth");
            Table("user");
            Id(x => x.Id, "id").GeneratedBy.SequenceIdentity("user_id_seq");
            Map(x => x.Reference, "reference");
            Map(x => x.Username, "username");
            Map(x => x.Password, "password");
            Map(x => x.PasswordSalt, "password_salt");
        }
    }
}