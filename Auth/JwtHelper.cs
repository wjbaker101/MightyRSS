using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using MightyRSS.Auth.Types;

namespace MightyRSS.Auth
{
    public interface IJwtHelper
    {
        string CreateToken(AuthClaims authClaims);
        bool TryParseToken(string jwtToken, out AuthClaims authClaims);
    }

    public sealed class JwtHelper : IJwtHelper
    {
        private readonly SecurityKey _securityKey;

        public JwtHelper()
        {
            _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("d41a1136-3314-42e3-a56f-881ad059e20b"));
        }

        public string CreateToken(AuthClaims authClaims)
        {
            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new List<Claim>
                {
                    new(AuthClaimType.UserReference, authClaims.UserReference.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(descriptor);

            return handler.WriteToken(securityToken);
        }

        public bool TryParseToken(string jwtToken, out AuthClaims authClaims)
        {
            authClaims = null;

            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = _securityKey
            };

            try
            {
                var principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, parameters, out _);

                var userReference = principal.Claims.Single(x => x.Type == AuthClaimType.UserReference).Value;

                authClaims = new AuthClaims
                {
                    UserReference = Guid.Parse(userReference)
                };

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}