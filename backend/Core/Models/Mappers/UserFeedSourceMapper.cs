using Data.Records;

namespace Core.Models.Mappers;

public static class UserFeedSourceMapper
{
    public static UserFeedSourceModel Map(UserFeedSourceRecord userFeedSource)
    {
        return new UserFeedSourceModel
        {
            Collection = userFeedSource.Collection,
            TitleAlias = userFeedSource.Title
        };
    }
}