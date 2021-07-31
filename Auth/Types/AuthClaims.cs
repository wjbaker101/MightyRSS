using System;

namespace MightyRSS.Auth.Types
{
    public static class AuthClaimType
    {
        public const string UserReference = "UserReference";
    }

    public sealed class AuthClaims
    {
        public Guid UserReference { get; init; }
    }
}