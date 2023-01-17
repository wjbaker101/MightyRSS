using Data.Types;
using FluentNHibernate.Mapping;

namespace Data.Records;

public class FeedSourceRecord
{
    public virtual long Id { get; init; }
    public virtual Guid Reference { get; set; }
    public virtual string Title { get; set; } = null!;
    public virtual string Description { get; set; } = null!;
    public virtual string RssUrl { get; set; } = null!;
    public virtual string WebsiteUrl { get; set; } = null!;
    public virtual List<Article>? Articles { get; set; }
    public virtual DateTime? ArticlesUpdatedAt { get; set; }

    public sealed class Article
    {
        public string Url { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Summary { get; set; } = null!;
        public string Author { get; set; } = null!;
        public DateTime? PublishedAt { get; set; } = null!;
        public string PublishedAtAsString { get; set; } = null!;
    }
}

public sealed class FeedSourceRecordMap : ClassMap<FeedSourceRecord>
{
    public FeedSourceRecordMap()
    {
        Schema("mighty_rss");
        Table("feed_source");
        Id(x => x.Id, "id").GeneratedBy.SequenceIdentity("source_id_seq");
        Map(x => x.Reference, "reference");
        Map(x => x.Title, "title");
        Map(x => x.Description, "description");
        Map(x => x.RssUrl, "rss_url");
        Map(x => x.WebsiteUrl, "website_url");
        Map(x => x.Articles, "articles").CustomType<JsonBlob<List<FeedSourceRecord.Article>>>();
        Map(x => x.ArticlesUpdatedAt, "articles_updated_at");
    }
}