﻿using FluentNHibernate.Mapping;
using MightyRSS.Data.Types;
using System;
using System.Collections.Generic;
using WJBCommon.Lib.Data.Type;

namespace MightyRSS.Data.Records
{
    public class FeedSourceRecord : DatabaseRecord
    {
        public virtual Guid Reference { get; init; }
        public virtual string Title { get; init; }
        public virtual string Description { get; init; }
        public virtual string RssUrl { get; init; }
        public virtual string WebsiteUrl { get; init; }
        public virtual List<Article> Articles { get; init; }
        public virtual DateTime ArticlesUpdatedAt { get; init; }

        public sealed class Article
        {
            public string Url { get; init; }
            public string Title { get; init; }
            public string Summary { get; init; }
            public string Author { get; init; }
            public DateTime? PublishedAt { get; init; }
            public string PublishedAtAsString { get; init; }
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