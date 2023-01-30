using Core.Models.Mappers;
using Data.Records;
using Data.UoW;
using MightyRSS.Api.Collections.Types;
using MightyRSS.Types;
using NetApiLibs.Extension;
using NetApiLibs.Type;
using System;
using System.Linq;

namespace MightyRSS.Api.Collections;

public interface ICollectionsService
{
    Result<CreateCollectionResponse> CreateCollection(IRequestContext requestContext, CreateCollectionRequest request);
    Result<UpdateCollectionResponse> UpdateCollection(IRequestContext requestContext, Guid collectionReference, UpdateCollectionRequest request);
    Result<GetCollectionsResponse> GetCollections(IRequestContext requestContext);
}

public sealed class CollectionsService : ICollectionsService
{
    private readonly IUnitOfWorkFactory<IMightyUnitOfWork> _mightyUnitOfWorkFactory;

    public CollectionsService(IUnitOfWorkFactory<IMightyUnitOfWork> mightyUnitOfWorkFactory)
    {
        _mightyUnitOfWorkFactory = mightyUnitOfWorkFactory;
    }

    public Result<CreateCollectionResponse> CreateCollection(IRequestContext requestContext, CreateCollectionRequest request)
    {
        using var unitOfWork = _mightyUnitOfWorkFactory.Create();

        var collection = unitOfWork.Collections.Save(new CollectionRecord
        {
            Reference = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            User = requestContext.User,
            Name = request.Name
        });

        unitOfWork.Commit();

        return new CreateCollectionResponse
        {
            Collection = CollectionMapper.Map(collection)
        };
    }

    public Result<UpdateCollectionResponse> UpdateCollection(IRequestContext requestContext, Guid collectionReference, UpdateCollectionRequest request)
    {
        using var unitOfWork = _mightyUnitOfWorkFactory.Create();

        var collectionResult = unitOfWork.Collections.GetByReference(collectionReference);
        if (!collectionResult.TrySuccess(out var collection))
            return Result<UpdateCollectionResponse>.FromFailure(collectionResult);

        if (collection.User.Reference != requestContext.User.Reference)
            return Result<UpdateCollectionResponse>.Failure("Cannot update a collection that you do not own.");

        collection.Name = request.Name;

        unitOfWork.Collections.Update(collection);

        unitOfWork.Commit();

        return new UpdateCollectionResponse
        {
            Collection = CollectionMapper.Map(collection)
        };
    }

    public Result<GetCollectionsResponse> GetCollections(IRequestContext requestContext)
    {
        using var unitOfWork = _mightyUnitOfWorkFactory.Create();

        var collections = unitOfWork.Collections.GetByUser(requestContext.User);

        unitOfWork.Commit();

        return new GetCollectionsResponse
        {
            FeedSourceCount = collections.SelectMany(x => x).Count(),
            Collections = collections
                .OrderBy(x => x.Key?.Name)
                .ConvertAll(grouping => new GetCollectionsResponse.CollectionDetails
                {
                    Collection = grouping.Key == null ? null : CollectionMapper.Map(grouping.Key),
                    FeedSources = grouping
                        .OrderBy(x => x.Title ?? x.FeedSource.Title)
                        .ConvertAll(x => FeedSourceMapper.Map(x.FeedSource, x))
                })
        };
    }
}