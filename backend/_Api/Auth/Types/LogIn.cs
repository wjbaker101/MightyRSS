using System;

namespace MightyRSS._Api.Auth.Types
{
    public sealed class LogInRequest
    {
        public string Username { get; init; }
        public string Password { get; init; }
    }

    public sealed class LogInResponse
    {
        public string JwtToken { get; init; }
    }
}