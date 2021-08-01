using FluentNHibernate.Mapping;
using MightyRSS.Data.Types;
using System;
using System.Collections.Generic;

namespace MightyRSS.Data.Records
{
    public class FeedSourceRecord
    {
        public virtual int Id { get; init; }
        public virtual Guid Reference { get; init; }
        public virtual string Title { get; init; }
        public virtual string Description { get; init; }
        public virtual string RssUrl { get; init; }
        public virtual string WebsiteUrl { get; init; }
        public virtual List<FeedSourceArticleJsonb> Articles { get; init; }
        public virtual DateTime ArticlesUpdatedAt { get; init; }
    }

    public sealed class FeedSourceRecordMap : ClassMap<FeedSourceRecord>
    {
        public FeedSourceRecordMap()
        {
            Schema("feed");
            Table("source");
            Id(x => x.Id, "id").GeneratedBy.SequenceIdentity("source_id_seq");
            Map(x => x.Reference, "reference");
            Map(x => x.Title, "title");
            Map(x => x.Description, "description");
            Map(x => x.RssUrl, "rss_url");
            Map(x => x.WebsiteUrl, "website_url");
            Map(x => x.Articles, "articles").CustomType<JsonBlob<List<FeedSourceArticleJsonb>>>();
            Map(x => x.ArticlesUpdatedAt, "articles_updated_at");
        }
    }
}