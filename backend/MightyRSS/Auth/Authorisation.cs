using Data.UoW;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MightyRSS.Auth;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class Authorisation : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var authHeader = context.HttpContext.Request.Headers["Authorisation"];

        var jwtHelper = context.HttpContext.RequestServices.GetRequiredService<IJwtHelper>();

        if (!jwtHelper.TryParseToken(authHeader, out var authClaims))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var unitOfWorkFactory = context.HttpContext.RequestServices.GetRequiredService<IUnitOfWorkFactory<IMightyUnitOfWork>>();
        using var unitOfWork = unitOfWorkFactory.Create();

        var userResult = unitOfWork.Users.GetByReference(authClaims.UserReference);
        if (userResult.IsFailure)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        unitOfWork.Commit();

        var requestContext = context.HttpContext.RequestServices.GetRequiredService<IRequestContext>();

        requestContext.User = userResult.Value;
    }
}