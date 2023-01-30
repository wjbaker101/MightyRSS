using Core.Models;
using System.Collections.Generic;

namespace MightyRSS.Api.Collections.Types;

public sealed class GetCollectionsResponse
{
    public required List<CollectionDetails> Collections { get; init; }
    public required int FeedSourceCount { get; init; }

    public sealed class CollectionDetails
    {
        public required CollectionModel? Collection { get; init; }
        public required List<FeedSourceModel> FeedSources { get; init; }
    }
}