using Core.Models;
using System.Collections.Generic;

namespace MightyRSS.Api.Configuration.Types;

public sealed class GetConfigurationResponse
{
    public required List<FeedSourceCollection> Collections { get; init; }

    public sealed class FeedSourceCollection
    {
        public required string? Collection { get; init; }
        public required List<FeedSourceDetails> FeedSources { get; init; }
    }

    public sealed class FeedSourceDetails
    {
        public required FeedSourceModel FeedSource { get; init; }
        public required UserFeedSourceModel UserFeedSource { get; init; }
    }
}