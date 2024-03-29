﻿using Microsoft.AspNetCore.Mvc;
using MightyRSS.Api.Auth.Attributes;
using MightyRSS.Api.Collections.Types;
using MightyRSS.Types;
using NetApiLibs.Api;
using System;

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
    public IActionResult CreateCollection([FromServices] IRequestContext requestContext, [FromBody] CreateCollectionRequest request)
    {
        var result = _collectionsService.CreateCollection(requestContext, request);

        return ToApiResponse(result);
    }

    [HttpPut]
    [Route("{collectionReference:guid}")]
    [Authorisation]
    public IActionResult UpdateCollection([FromServices] IRequestContext requestContext, [FromRoute] Guid collectionReference, [FromBody] UpdateCollectionRequest request)
    {
        var result = _collectionsService.UpdateCollection(requestContext, collectionReference, request);

        return ToApiResponse(result);
    }

    [HttpGet]
    [Route("")]
    [Authorisation]
    public IActionResult GetCollections([FromServices] IRequestContext requestContext)
    {
        var result = _collectionsService.GetCollections(requestContext);

        return ToApiResponse(result);
    }
}