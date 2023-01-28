using FluentNHibernate.Mapping;

namespace Data.Records;

public class CollectionRecord
{
    public long Id { get; init; }
    public required Guid Reference { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required UserRecord User { get; init; }
    public required string Name { get; init; }
}

public sealed class CollectionRecordMap : ClassMap<CollectionRecord>
{
    public CollectionRecordMap()
    {
        Schema("mighty_rss");
        Table("collection");
        Id(x => x.Id, "id").GeneratedBy.SequenceIdentity("collection_id_seq");
        Map(x => x.Reference, "reference");
        Map(x => x.CreatedAt, "created_at");
        References(x => x.User, "user_id");
        Map(x => x.Name, "name");
    }
}