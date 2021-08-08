using Microsoft.AspNetCore.Mvc;
using MightyRSS._Api.Auth.Types;
using System;
using System.Net;
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
            var getUserResponse = _authService.GetUser(reference);
            if (getUserResponse == null)
                return NotFound();

            return Ok(getUserResponse);
        }

        [HttpPost]
        [Route("user")]
        public IActionResult CreateUser([FromBody] CreateUserRequest request)
        {
            return StatusCode((int)HttpStatusCode.Created, _authService.CreateUser(request));
        }

        [HttpPost]
        [Route("login")]
        public IActionResult LogIn([FromBody] LogInRequest request)
        {
            var logInResponse = _authService.LogIn(request);
            if (logInResponse == null)
                return Unauthorized();

            return Ok(logInResponse);
        }
    }
}