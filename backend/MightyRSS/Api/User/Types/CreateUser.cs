using System;

namespace MightyRSS.Api.User.Types;

public sealed class CreateUserRequest
{
    public required string Username { get; init; }
    public required string Password { get; init; }
}

public sealed class CreateUserResponse
{
    public required Guid Reference { get; init; }
    public required string Username { get; init; }
}