using FluentNHibernate.Mapping;

namespace MightyRSS.Data.Records;

public class UserDataFeedSourceRecord
{
    public virtual long Id { get; init; }
    public virtual UserRecord User { get; set; }
    public virtual FeedSourceRecord FeedSource { get; set; }
    public virtual string Collection { get; set; }
    public virtual string Title { get; set; }
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
        Map(x => x.Collection, "collection");
        Map(x => x.Title, "title");
    }
}