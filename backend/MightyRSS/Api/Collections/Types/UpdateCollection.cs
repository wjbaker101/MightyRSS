using MightyRSS.Models;

namespace MightyRSS.Api.Collections.Types;

public sealed class UpdateCollectionRequest
{
    public required string Name { get; init; }
}

public sealed class UpdateCollectionResponse
{
    public required CollectionModel Collection { get; init; }
}