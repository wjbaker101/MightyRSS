using Data.Records;

namespace Core.Models.Mappers;

public static class FeedSourceMapper
{
    public static FeedSourceModel Map(FeedSourceRecord feedSource)
    {
        return new FeedSourceModel
        {
            Reference = feedSource.Reference,
            Title = feedSource.Title,
            Description = feedSource.Description,
            RssUrl = feedSource.RssUrl,
            WebsiteUrl = feedSource.WebsiteUrl
        };
    }
}