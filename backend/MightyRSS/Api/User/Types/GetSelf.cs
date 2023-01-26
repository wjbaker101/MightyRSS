namespace MightyRSS.Api.User.Types;

public sealed class GetSelfResponse
{
    public required UserModel User { get; init; }
}