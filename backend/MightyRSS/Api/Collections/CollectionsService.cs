﻿using Core.Models.Mappers;
using Data.Records;
using Data.UoW;
using MightyRSS.Api.Collections.Types;
using MightyRSS.Types;
using NetApiLibs.Type;
using System;

namespace MightyRSS.Api.Collections;

public interface ICollectionsService
{
    Result<CreateCollectionResponse> CreateCollection(IRequestContext requestContext, CreateCollectionRequest request);
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

    public Result<GetCollectionsResponse> GetCollections(IRequestContext requestContext)
    {
        using var unitOfWork = _mightyUnitOfWorkFactory.Create();

        var collections = unitOfWork.Collections.GetByUser(requestContext.User);

        unitOfWork.Commit();

        return new GetCollectionsResponse
        {
            Collections = collections.ConvertAll(CollectionMapper.Map)
        };
    }
}