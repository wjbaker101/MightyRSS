using System;

namespace MightyRSS._Api.Feed.Types;

public sealed class AddFeedToCollectionRequest
{
    public string Collection { get; init; }
}