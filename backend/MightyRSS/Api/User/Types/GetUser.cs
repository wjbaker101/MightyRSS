using System;

namespace MightyRSS.Api.User.Types;

public sealed class GetUserResponse
{
    public Guid Reference { get; init; }
    public string Username { get; init; }
}