using Microsoft.AspNetCore.Mvc;
using MightyRSS._Api.Feed.Types;
using MightyRSS.Auth;
using System;
using WJBCommon.Lib.Api.Controller;

namespace MightyRSS._Api.Feed
{
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
        [ServiceFilter(typeof(Authorisation))]
        public IActionResult AddFeedSource([FromServices] IRequestContext requestContext, [FromBody] AddFeedSourceRequest request)
        {
            var result = _feedService.AddFeedSource(requestContext.User, request);

            return ApiResponseFromResult(result);
        }

        [HttpGet]
        [Route("")]
        [ServiceFilter(typeof(Authorisation))]
        public IActionResult GetFeed([FromServices] IRequestContext requestContext)
        {
            var result = _feedService.GetFeed(requestContext.User);

            return ApiResponseFromResult(result);
        }

        [HttpDelete]
        [Route("source/{reference:guid}")]
        [ServiceFilter(typeof(Authorisation))]
        public IActionResult DeleteFeedSource([FromServices] IRequestContext requestContext, [FromRoute] Guid reference)
        {
            var result = _feedService.DeleteFeedSource(requestContext.User, reference);

            return ApiResponseFromResult(result);
        }
    }
}