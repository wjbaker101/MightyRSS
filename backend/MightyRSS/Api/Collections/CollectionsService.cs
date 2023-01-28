using MightyRSS.Api.Collections.Types;
using MightyRSS.Types;
using NetApiLibs.Type;

namespace MightyRSS.Api.Collections;

public interface ICollectionsService
{
    Result<CreateCollectionResponse> CreateCollection(IRequestContext requestContext, CreateCollectionRequest request);
}

public sealed class CollectionsService : ICollectionsService
{
    public CollectionsService()
    {
    }

    public Result<CreateCollectionResponse> CreateCollection(IRequestContext requestContext, CreateCollectionRequest request)
    {
        return new CreateCollectionResponse();
    }
}