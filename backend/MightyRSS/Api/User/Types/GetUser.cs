using System;

namespace MightyRSS.Api.User.Types;

public sealed class GetUserResponse
{
    public required Guid Reference { get; init; }
    public required string Username { get; init; }
}