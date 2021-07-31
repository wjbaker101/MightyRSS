using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MightyRSS.Data.Repositories;
using System;

namespace MightyRSS.Auth
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class Authorisation : Attribute, IAuthorizationFilter
    {
        private readonly IJwtHelper _jwtHelper;
        private readonly IUserRepository _userRepository;
        private readonly IRequestContext _requestContext;

        public Authorisation(IJwtHelper jwtHelper, IUserRepository userRepository, IRequestContext requestContext)
        {
            _jwtHelper = jwtHelper;
            _userRepository = userRepository;
            _requestContext = requestContext;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var authHeader = context.HttpContext.Request.Headers["Authorisation"];

            if (!_jwtHelper.TryParseToken(authHeader, out var authClaims))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var user = _userRepository.GetByReference(authClaims.UserReference);
            if (user == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            _requestContext.User = user;
        }
    }
}