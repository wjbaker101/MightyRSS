using Microsoft.IdentityModel.Tokens;
using NetApiLibs.Type;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace MightyRSS._Api.Auth;

public interface ILoginTokenService
{
    string CreateToken(Guid userReference);
    Result<Guid> GetUserReferenceByToken(string loginToken);
}

public sealed class LoginTokenService : ILoginTokenService
{
    private const string USER_REFERENCE_CLAIM_TYPE = "UserReference";

    private readonly SecurityKey _securityKey;

    public LoginTokenService()
    {
        _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("d41a1136-3314-42e3-a56f-881ad059e20b"));
    }

    public string CreateToken(Guid userReference)
    {
        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new List<Claim>
            {
                new(USER_REFERENCE_CLAIM_TYPE, userReference.ToString())
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256Signature)
        };

        var handler = new JwtSecurityTokenHandler();
        var securityToken = handler.CreateToken(descriptor);

        return handler.WriteToken(securityToken);
    }

    public Result<Guid> GetUserReferenceByToken(string loginToken)
    {
        var parameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            IssuerSigningKey = _securityKey
        };

        try
        {
            var principal = new JwtSecurityTokenHandler().ValidateToken(loginToken, parameters, out var _);

            var userReference = principal.Claims.Single(x => x.Type == USER_REFERENCE_CLAIM_TYPE).Value;

            return Guid.Parse(userReference);
        }
        catch (Exception)
        {
            return Result<Guid>.Failure("Unable to parse login token.");
        }
    }
}