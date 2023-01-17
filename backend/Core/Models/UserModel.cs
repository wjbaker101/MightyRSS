namespace Core.Models;

public sealed class UserModel
{
    public Guid Reference { get; init; }
    public string Username { get; init; } = null!;
}