using Microsoft.AspNetCore.Mvc;
using MightyRSS.Api.Auth.Attributes;
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
}