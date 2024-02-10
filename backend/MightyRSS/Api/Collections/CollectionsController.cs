using Microsoft.AspNetCore.Mvc;
using MightyRSS.Api.Auth.Attributes;
using MightyRSS.Api.Collections.Types;
using MightyRSS.Types;
using NetApiLibs.Api;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MightyRSS.Api.Collections;

[ApiController]
[Route("api/collections")]
public sealed class CollectionsController : ApiController
{
    private readonly ICollectionsService _collectionsService;

    public CollectionsController(ICollectionsService collectionsService)
    {
        _collectionsService = collectionsService;
    }

    [HttpPost]
    [Route("")]
    [Authorisation]
    public async Task<IActionResult> CreateCollection([FromServices] IRequestContext requestContext, [FromBody] CreateCollectionRequest request, CancellationToken cancellationToken)
    {
        var result = await _collectionsService.CreateCollection(requestContext, request, cancellationToken);

        return ToApiResponse(result);
    }

    [HttpPut]
    [Route("{collectionReference:guid}")]
    [Authorisation]
    public async Task<IActionResult> UpdateCollection([FromServices] IRequestContext requestContext, [FromRoute] Guid collectionReference, [FromBody] UpdateCollectionRequest request, CancellationToken cancellationToken)
    {
        var result = await _collectionsService.UpdateCollection(requestContext, collectionReference, request, cancellationToken);

        return ToApiResponse(result);
    }

    [HttpGet]
    [Route("")]
    [Authorisation]
    public async Task<IActionResult> GetCollections([FromServices] IRequestContext requestContext, CancellationToken cancellationToken)
    {
        var result = await _collectionsService.GetCollections(requestContext, cancellationToken);

        return ToApiResponse(result);
    }
}