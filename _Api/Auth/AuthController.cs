using Microsoft.AspNetCore.Mvc;
using MightyRSS._Api.Auth.Types;
using System;
using WJBCommon.Lib.Api.Controller;

namespace MightyRSS._Api.Auth
{
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

            return ApiResponseFromResult(result);
        }

        [HttpPost]
        [Route("user")]
        public IActionResult CreateUser([FromBody] CreateUserRequest request)
        {
            var result = _authService.CreateUser(request);

            return ApiResponseFromResult(result);
        }

        [HttpPost]
        [Route("login")]
        public IActionResult LogIn([FromBody] LogInRequest request)
        {
            var result = _authService.LogIn(request);

            return ApiResponseFromResult(result);
        }
    }
}