using Data.Records;

namespace MightyRSS.Models.Mappers;

public static class FeedSourceMapper
{
    public static FeedSourceModel Map(FeedSourceRecord feedSource, UserFeedSourceRecord userFeedSource)
    {
        return new FeedSourceModel
        {
            Reference = feedSource.Reference,
            Title = userFeedSource.Title ?? feedSource.Title,
            Description = feedSource.Description,
            RssUrl = feedSource.RssUrl,
            WebsiteUrl = feedSource.WebsiteUrl
        };
    }
}