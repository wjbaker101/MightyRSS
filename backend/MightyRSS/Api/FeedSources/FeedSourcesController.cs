using Microsoft.AspNetCore.Mvc;
using MightyRSS.Api.Auth.Attributes;
using MightyRSS.Api.FeedSources.Types;
using MightyRSS.Types;
using NetApiLibs.Api;
using System;
using System.Threading;
using System.Threading.Tasks;

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
    public async Task<IActionResult> AddFeedSource([FromServices] IRequestContext requestContext, [FromBody] AddFeedSourceRequest request, CancellationToken cancellationToken)
    {
        var result = await _feedSourcesService.AddFeedSource(requestContext.User, request, cancellationToken);

        return ToApiResponse(result);
    }

    [HttpPut]
    [Route("source/{reference:guid}")]
    [Authorisation]
    public async Task<IActionResult> UpdateFeedSource([FromServices] IRequestContext requestContext, [FromRoute] Guid reference, [FromBody] UpdateFeedSourceRequest request, CancellationToken cancellationToken)
    {
        var result = await _feedSourcesService.UpdateFeedSource(requestContext.User, reference, request, cancellationToken);

        return ToApiResponse(result);
    }

    [HttpDelete]
    [Route("source/{reference:guid}")]
    [Authorisation]
    public async Task<IActionResult> DeleteFeedSource([FromServices] IRequestContext requestContext, [FromRoute] Guid reference, CancellationToken cancellationToken)
    {
        var result = await _feedSourcesService.DeleteFeedSource(requestContext.User, reference, cancellationToken);

        return ToApiResponse(result);
    }
}