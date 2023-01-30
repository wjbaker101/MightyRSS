using Microsoft.AspNetCore.Mvc;
using MightyRSS.Api.Auth.Attributes;
using MightyRSS.Api.FeedSources.Types;
using MightyRSS.Types;
using NetApiLibs.Api;
using System;

namespace MightyRSS.Api.FeedSources;

[ApiController]
[Route("api/feed")]
public sealed class FeedSourcesController : ApiController
{
    private readonly IFeedSourcesService _feedSourcesService;

    public FeedSourcesController(IFeedSourcesService feedSourcesService)
    {
        _feedSourcesService = feedSourcesService;
    }

    [HttpPost]
    [Route("")]
    [Authorisation]
    public IActionResult AddFeedSource([FromServices] IRequestContext requestContext, [FromBody] AddFeedSourceRequest request)
    {
        var result = _feedSourcesService.AddFeedSource(requestContext.User, request);

        return ToApiResponse(result);
    }

    [HttpPut]
    [Route("source/{reference:guid}")]
    [Authorisation]
    public IActionResult UpdateFeedSource([FromServices] IRequestContext requestContext, [FromRoute] Guid reference, [FromBody] UpdateFeedSourceRequest request)
    {
        var result = _feedSourcesService.UpdateFeedSource(requestContext.User, reference, request);

        return ToApiResponse(result);
    }

    [HttpDelete]
    [Route("source/{reference:guid}")]
    [Authorisation]
    public IActionResult DeleteFeedSource([FromServices] IRequestContext requestContext, [FromRoute] Guid reference)
    {
        var result = _feedSourcesService.DeleteFeedSource(requestContext.User, reference);

        return ToApiResponse(result);
    }
}