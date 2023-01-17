using FluentNHibernate.Mapping;

namespace Data.Records;

public class UserDataFeedSourceRecord
{
    public virtual long Id { get; init; }
    public virtual UserRecord User { get; init; } = null!;
    public virtual FeedSourceRecord FeedSource { get; init; } = null!;
    public virtual string? Collection { get; set; }
    public virtual string? Title { get; set; }
}

public sealed class UserDataFeedSourceRecordMap : ClassMap<UserDataFeedSourceRecord>
{
    public UserDataFeedSourceRecordMap()
    {
        Schema("mighty_rss");
        Table("user_feed_source");
        Id(x => x.Id, "id").GeneratedBy.SequenceIdentity("feed_source_id_seq");
        References(x => x.User, "user_id");
        References(x => x.FeedSource, "feed_source_id");
        Map(x => x.Collection, "collection");
        Map(x => x.Title, "title");
    }
}