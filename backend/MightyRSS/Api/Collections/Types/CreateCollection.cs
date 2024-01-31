using MightyRSS.Models;

namespace MightyRSS.Api.Collections.Types;

public sealed class CreateCollectionRequest
{
    public required string Name { get; init; }
}

public sealed class CreateCollectionResponse
{
    public required CollectionModel Collection { get; init; }
}