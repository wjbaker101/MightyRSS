﻿using Data.Records;

namespace MightyRSS.Models.Mappers;

public static class CollectionMapper
{
    public static CollectionModel Map(CollectionRecord collection)
    {
        return new CollectionModel
        {
            Reference = collection.Reference,
            CreatedAt = collection.CreatedAt,
            Name = collection.Name
        };
    }
}