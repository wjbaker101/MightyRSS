using Data.UoW;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using MightyRSS.Types;
using System;

namespace MightyRSS.Api.Auth.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class Authorisation : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue("Authorisation", out var authHeader))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var loginTokenService = context.HttpContext.RequestServices.GetRequiredService<ILoginTokenService>();

        var userReferenceResult = loginTokenService.GetUserReferenceByToken(authHeader.ToString());
        if (userReferenceResult.IsFailure)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var unitOfWorkFactory = context.HttpContext.RequestServices.GetRequiredService<IUnitOfWorkFactory<IMightyUnitOfWork>>();
        using var unitOfWork = unitOfWorkFactory.Create();

        var userResult = unitOfWork.Users.GetByReference(userReferenceResult.Value);
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