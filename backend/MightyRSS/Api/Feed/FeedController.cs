using Microsoft.AspNetCore.Mvc;
using MightyRSS.Api.Auth.Attributes;
using MightyRSS.Api.Feed.Types;
using MightyRSS.Types;
using NetApiLibs.Api;
using System;

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

    [HttpPost]
    [Route("source/{reference:guid}/collection")]
    [Authorisation]
    public IActionResult AddFeedToCollection([FromServices] IRequestContext requestContext, [FromRoute] Guid reference, [FromBody] AddFeedToCollectionRequest request)
    {
        var result = _feedService.AddFeedToCollection(requestContext.User, reference, request);

        return ToApiResponse(result);
    }
}