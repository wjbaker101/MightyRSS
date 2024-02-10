using Microsoft.AspNetCore.Mvc;
using MightyRSS.Api.Auth.Attributes;
using MightyRSS.Types;
using NetApiLibs.Api;
using System.Threading;
using System.Threading.Tasks;

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
    public async Task<IActionResult> GetFeed([FromServices] IRequestContext requestContext, CancellationToken cancellationToken)
    {
        var result = await _feedService.GetFeed(requestContext.User, cancellationToken);

        return ToApiResponse(result);
    }
}