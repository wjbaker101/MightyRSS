using Data.Records.Types;
using FluentNHibernate.Mapping;

namespace Data.Records;

public class UserFeedSourceRecord : IApiRecord
{
    public virtual long Id { get; init; }
    public virtual required UserRecord User { get; init; }
    public virtual required FeedSourceRecord FeedSource { get; init; }
    public virtual required string? Collection { get; set; }
    public virtual required string? Title { get; set; }
    public virtual required CollectionRecord? CollectionRecord { get; set; }
}

public sealed class UserFeedSourceRecordMap : ClassMap<UserFeedSourceRecord>
{
    public UserFeedSourceRecordMap()
    {
        Schema("mighty_rss");
        Table("user_feed_source");
        Id(x => x.Id, "id").GeneratedBy.SequenceIdentity("feed_source_id_seq");
        References(x => x.User, "user_id");
        References(x => x.FeedSource, "feed_source_id");
        Map(x => x.Collection, "collection");
        Map(x => x.Title, "title");
        References(x => x.CollectionRecord, "collection_id");
    }
}