using Microsoft.AspNetCore.Mvc;
using MightyRSS.Api.Auth.Types;
using NetApiLibs.Api;

namespace MightyRSS.Api.Auth;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ApiController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> LogIn([FromBody] LogInRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.LogIn(request, cancellationToken);

        return ToApiResponse(result);
    }
}