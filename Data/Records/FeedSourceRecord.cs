using FluentNHibernate.Mapping;
using MightyRSS.Data.Types;
using System;
using System.Collections.Generic;
using WJBCommon.Lib.Data.Type;

namespace MightyRSS.Data.Records
{
    public class FeedSourceRecord : DatabaseRecord
    {
        public virtual Guid Reference { get; set; }
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual string RssUrl { get; set; }
        public virtual string WebsiteUrl { get; set; }
        public virtual List<Article> Articles { get; set; }
        public virtual DateTime ArticlesUpdatedAt { get; set; }

        public sealed class Article
        {
            public string Url { get; set; }
            public string Title { get; set; }
            public string Summary { get; set; }
            public string Author { get; set; }
            public DateTime? PublishedAt { get; set; }
            public string PublishedAtAsString { get; set; }
        }
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
            Map(x => x.Articles, "articles").CustomType<JsonBlob<List<FeedSourceRecord.Article>>>();
            Map(x => x.ArticlesUpdatedAt, "articles_updated_at");
        }
    }
}