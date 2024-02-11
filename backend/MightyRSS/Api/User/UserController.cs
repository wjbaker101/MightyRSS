using Microsoft.AspNetCore.Mvc;
using MightyRSS.Api.Auth.Attributes;
using MightyRSS.Api.User.Types;
using MightyRSS.Types;
using NetApiLibs.Api;

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
    public async Task<IActionResult> GetUser([FromRoute] Guid reference, CancellationToken cancellationToken)
    {
        var result = await _userService.GetUser(reference, cancellationToken);

        return ToApiResponse(result);
    }

    [HttpPost]
    [Route("")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        var result = await _userService.CreateUser(request, cancellationToken);

        return ToApiResponse(result);
    }
}