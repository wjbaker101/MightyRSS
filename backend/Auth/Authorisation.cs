using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MightyRSS.Data.UoW;
using System;

namespace MightyRSS.Auth;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class Authorisation : Attribute, IAuthorizationFilter
{
    private readonly IJwtHelper _jwtHelper;
    private readonly IUnitOfWorkFactory<IMightyUnitOfWork> _mightyUnitOfWork;
    private readonly IRequestContext _requestContext;

    public Authorisation(
        IJwtHelper jwtHelper,
        IUnitOfWorkFactory<IMightyUnitOfWork> mightyUnitOfWork,
        IRequestContext requestContext)
    {
        _jwtHelper = jwtHelper;
        _mightyUnitOfWork = mightyUnitOfWork;
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

        using var unitOfWork = _mightyUnitOfWork.Create();

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