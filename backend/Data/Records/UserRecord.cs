using FluentNHibernate.Mapping;
using System;

namespace MightyRSS.Data.Records;

public class UserRecord
{
    public virtual long Id { get; init; }
    public virtual Guid Reference { get; set; }
    public virtual string Username { get; set; }
    public virtual string Password { get; set; }
    public virtual Guid PasswordSalt { get; set; }
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