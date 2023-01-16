using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using MightyRSS.Data.UoW;
using System;

namespace MightyRSS.Auth;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class Authorisation : Attribute, IAuthorizationFilter
{
    private readonly IRequestContext _requestContext;

    public Authorisation(IRequestContext requestContext)
    {
        _requestContext = requestContext;
    }

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

        var user = unitOfWork.Users.GetByReference(authClaims.UserReference);
        if (user == null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        unitOfWork.Commit();

        _requestContext.User = user;
    }
}