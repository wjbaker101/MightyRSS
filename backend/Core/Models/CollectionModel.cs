namespace Core.Models;

public sealed class CollectionModel
{
    public required Guid Reference { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required string Name { get; init; }
}