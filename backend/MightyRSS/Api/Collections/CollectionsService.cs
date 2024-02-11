using Data.Records;
using Data.UoW;
using MightyRSS.Api.Collections.Types;
using MightyRSS.Models;
using MightyRSS.Models.Mappers;
using MightyRSS.Types;
using NetApiLibs.Extension;
using NetApiLibs.Type;

namespace MightyRSS.Api.Collections;

public interface ICollectionsService
{
    Task<Result<CreateCollectionResponse>> CreateCollection(IRequestContext requestContext, CreateCollectionRequest request, CancellationToken cancellationToken);
    Task<Result<UpdateCollectionResponse>> UpdateCollection(IRequestContext requestContext, Guid collectionReference, UpdateCollectionRequest request, CancellationToken cancellationToken);
    Task<Result<GetCollectionsResponse>> GetCollections(IRequestContext requestContext, CancellationToken cancellationToken);
}

public sealed class CollectionsService : ICollectionsService
{
    private readonly IUnitOfWorkFactory<IMightyUnitOfWork> _mightyUnitOfWorkFactory;

    public CollectionsService(IUnitOfWorkFactory<IMightyUnitOfWork> mightyUnitOfWorkFactory)
    {
        _mightyUnitOfWorkFactory = mightyUnitOfWorkFactory;
    }

    public async Task<Result<CreateCollectionResponse>> CreateCollection(IRequestContext requestContext, CreateCollectionRequest request, CancellationToken cancellationToken)
    {
        using var unitOfWork = _mightyUnitOfWorkFactory.Create(cancellationToken);

        var collection = await unitOfWork.Collections.Save(new CollectionRecord
        {
            Reference = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            User = requestContext.User,
            Name = request.Name
        });

        await unitOfWork.Commit();

        return new CreateCollectionResponse
        {
            Collection = CollectionMapper.Map(collection)
        };
    }

    public async Task<Result<UpdateCollectionResponse>> UpdateCollection(IRequestContext requestContext, Guid collectionReference, UpdateCollectionRequest request, CancellationToken cancellationToken)
    {
        using var unitOfWork = _mightyUnitOfWorkFactory.Create(cancellationToken);

        var collectionResult = await unitOfWork.Collections.GetByReference(collectionReference);
        if (!collectionResult.TrySuccess(out var collection))
            return Result<UpdateCollectionResponse>.FromFailure(collectionResult);

        if (collection.User.Reference != requestContext.User.Reference)
            return Result<UpdateCollectionResponse>.Failure("Cannot update a collection that you do not own.");

        collection.Name = request.Name;

        await unitOfWork.Collections.Update(collection);

        await unitOfWork.Commit();

        return new UpdateCollectionResponse
        {
            Collection = CollectionMapper.Map(collection)
        };
    }

    public async Task<Result<GetCollectionsResponse>> GetCollections(IRequestContext requestContext, CancellationToken cancellationToken)
    {
        using var unitOfWork = _mightyUnitOfWorkFactory.Create(cancellationToken);

        var collections = await unitOfWork.Collections.GetByUser(requestContext.User);
        var feedSources = await unitOfWork.UserFeedSources.GetFeedSources(requestContext.User);

        await unitOfWork.Commit();

        var lookup = feedSources.ToLookup(x => x.CollectionRecord);

        return new GetCollectionsResponse
        {
            FeedSourceCount = feedSources.Count,
            Collections = collections
                .Concat(new CollectionRecord?[] { null })
                .OrderBy(x => x?.Name)
                .ConvertAll(collection => new GetCollectionsResponse.CollectionDetails
                {
                    Collection = collection == null ? null : CollectionMapper.Map(collection),
                    FeedSources = !lookup.Contains(collection)
                        ? new List<FeedSourceModel>()
                        : lookup[collection]
                            .OrderBy(x => x.Title ?? x.FeedSource.Title)
                            .ConvertAll(x => FeedSourceMapper.Map(x.FeedSource, x))
                })
        };
    }
}