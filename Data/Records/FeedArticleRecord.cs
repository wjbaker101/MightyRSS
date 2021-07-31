using FluentNHibernate.Mapping;
using System;

namespace MightyRSS.Data.Records
{
    public class FeedArticleRecord
    {
        public virtual int Id { get; init; }
        public virtual string Url { get; init; }
        public virtual FeedSourceRecord FeedSource { get; init; }
        public virtual string Title { get; init; }
        public virtual string Summary { get; init; }
        public virtual string Author { get; init; }
        public virtual DateTime? PublishedAt { get; init; }
    }

    public sealed class FeedArticleRecordMap : ClassMap<FeedArticleRecord>
    {
        public FeedArticleRecordMap()
        {
            Schema("feed");
            Table("article");
            Id(x => x.Id, "id").GeneratedBy.SequenceIdentity("article_id_seq");
            Map(x => x.Url, "url");
            References(x => x.FeedSource).Column("feed_source_reference").ForeignKey("reference");
            Map(x => x.Title, "title");
            Map(x => x.Summary, "summary");
            Map(x => x.Author, "author");
            Map(x => x.PublishedAt, "published_at");
        }
    }
}