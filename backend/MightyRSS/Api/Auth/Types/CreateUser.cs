using System;

namespace MightyRSS.Api.Auth.Types;

public sealed class CreateUserRequest
{
    public string Username { get; init; }
    public string Password { get; init; }
}

public sealed class CreateUserResponse
{
    public Guid Reference { get; init; }
    public string Username { get; init; }
}