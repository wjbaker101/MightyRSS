using Microsoft.AspNetCore.Mvc;
using MightyRSS.Api.Auth.Attributes;
using MightyRSS.Types;
using NetApiLibs.Api;

namespace MightyRSS.Api.Feed;

[ApiController]
[Route("api/feed")]
public sealed class FeedController : ApiController
{
    private readonly IFeedService _feedService;

    public FeedController(IFeedService feedService)
    {
        _feedService = feedService;
    }

    [HttpGet]
    [Route("")]
    [Authorisation]
    public IActionResult GetFeed([FromServices] IRequestContext requestContext)
    {
        var result = _feedService.GetFeed(requestContext.User);

        return ToApiResponse(result);
    }
}