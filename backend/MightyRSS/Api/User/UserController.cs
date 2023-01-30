using Microsoft.AspNetCore.Mvc;
using MightyRSS.Api.Auth.Attributes;
using MightyRSS.Api.User.Types;
using MightyRSS.Types;
using NetApiLibs.Api;
using System;

namespace MightyRSS.Api.User;

[ApiController]
[Route("api/users")]
public sealed class UserController : ApiController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [Route("")]
    [Authorisation]
    public IActionResult GetSelf([FromServices] IRequestContext requestContext)
    {
        var result = _userService.GetSelf(requestContext);

        return ToApiResponse(result);
    }

    [HttpGet]
    [Route("{reference:guid}")]
    public IActionResult GetUser([FromRoute] Guid reference)
    {
        var result = _userService.GetUser(reference);

        return ToApiResponse(result);
    }

    [HttpPost]
    [Route("")]
    public IActionResult CreateUser([FromBody] CreateUserRequest request)
    {
        var result = _userService.CreateUser(request);

        return ToApiResponse(result);
    }
}