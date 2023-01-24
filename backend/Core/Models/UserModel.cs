namespace Core.Models;

public sealed class UserModel
{
    public required Guid Reference { get; init; }
    public required string Username { get; init; }
}