using Core.Models;
using System.Collections.Generic;

namespace MightyRSS.Api.Collections.Types;

public sealed class GetCollectionsResponse
{
    public required List<CollectionModel> Collections { get; init; }
}