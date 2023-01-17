using Microsoft.AspNetCore.Mvc;
using MightyRSS._Api.Auth.Types;
using NetApiLibs.Api;
using System;

namespace MightyRSS._Api.Auth;

[Route("api/auth")]
public sealed class AuthController : ApiController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    [Route("user/{reference:guid}")]
    public IActionResult GetUser([FromRoute] Guid reference)
    {
        var result = _authService.GetUser(reference);

        return ToApiResponse(result);
    }

    [HttpPost]
    [Route("user")]
    public IActionResult CreateUser([FromBody] CreateUserRequest request)
    {
        var result = _authService.CreateUser(request);

        return ToApiResponse(result);
    }

    [HttpPost]
    [Route("login")]
    public IActionResult LogIn([FromBody] LogInRequest request)
    {
        var result = _authService.LogIn(request);

        return ToApiResponse(result);
    }
}