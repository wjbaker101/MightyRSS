using FluentNHibernate.Mapping;

namespace MightyRSS.Data.Records
{
    public class UserDataFeedSourceRecord
    {
        public virtual int Id { get; init; }
        public virtual UserRecord User { get; init; }
        public virtual FeedSourceRecord FeedSource { get; init; }
    }

    public sealed class UserDataFeedSourceRecordMap : ClassMap<UserDataFeedSourceRecord>
    {
        public UserDataFeedSourceRecordMap()
        {
            Schema("user_data");
            Table("feed_source");
            Id(x => x.Id, "id").GeneratedBy.SequenceIdentity("feed_source_id_seq");
            References(x => x.User, "user_id");
            References(x => x.FeedSource, "feed_source_id");
        }
    }
}