using Microsoft.AspNetCore.Mvc;
using MightyRSS._Api.Auth.Attributes;
using MightyRSS._Api.Feed.Types;
using MightyRSS.Auth;
using NetApiLibs.Api;
using System;

namespace MightyRSS._Api.Feed;

[Route("api/feed")]
public sealed class FeedController : ApiController
{
    private readonly IFeedService _feedService;

    public FeedController(IFeedService feedService)
    {
        _feedService = feedService;
    }

    [HttpPost]
    [Route("")]
    [Authorisation]
    public IActionResult AddFeedSource([FromServices] IRequestContext requestContext, [FromBody] AddFeedSourceRequest request)
    {
        var result = _feedService.AddFeedSource(requestContext.User, request);

        return ToApiResponse(result);
    }

    [HttpGet]
    [Route("")]
    [Authorisation]
    public IActionResult GetFeed([FromServices] IRequestContext requestContext)
    {
        var result = _feedService.GetFeed(requestContext.User);

        return ToApiResponse(result);
    }

    [HttpDelete]
    [Route("source/{reference:guid}")]
    [Authorisation]
    public IActionResult DeleteFeedSource([FromServices] IRequestContext requestContext, [FromRoute] Guid reference)
    {
        var result = _feedService.DeleteFeedSource(requestContext.User, reference);

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

    [HttpPut]
    [Route("source/{reference:guid}")]
    [Authorisation]
    public IActionResult UpdateFeedSource([FromServices] IRequestContext requestContext, [FromRoute] Guid reference, [FromBody] UpdateFeedSourceRequest request)
    {
        var result = _feedService.UpdateFeedSource(requestContext.User, reference, request);

        return ToApiResponse(result);
    }
}