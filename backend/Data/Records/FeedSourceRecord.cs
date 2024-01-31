using Data.Records.Types;
using FluentNHibernate.Mapping;

namespace Data.Records;

public class FeedSourceRecord : IApiRecord
{
    public virtual long Id { get; init; }
    public virtual required Guid Reference { get; init; }
    public virtual required string Title { get; set; }
    public virtual required string Description { get; set; }
    public virtual required string RssUrl { get; set; }
    public virtual required string WebsiteUrl { get; set; }
    public virtual required List<Article>? Articles { get; set; }
    public virtual required DateTime? ArticlesUpdatedAt { get; set; }

    public sealed class Article
    {
        public required string Url { get; set; }
        public required string Title { get; set; }
        public required string Summary { get; set; }
        public required string Author { get; set; }
        public required DateTime? PublishedAt { get; set; }
        public required string PublishedAtAsString { get; set; }
    }
}

public sealed class FeedSourceRecordMap : ClassMap<FeedSourceRecord>
{
    public FeedSourceRecordMap()
    {
        Schema("mighty_rss");
        Table("feed_source");
        Id(x => x.Id, "id").GeneratedBy.SequenceIdentity("feed_source_id_seq");
        Map(x => x.Reference, "reference");
        Map(x => x.Title, "title");
        Map(x => x.Description, "description");
        Map(x => x.RssUrl, "rss_url");
        Map(x => x.WebsiteUrl, "website_url");
        Map(x => x.Articles, "articles").CustomType<JsonBlob<List<FeedSourceRecord.Article>>>();
        Map(x => x.ArticlesUpdatedAt, "articles_updated_at");
    }
}