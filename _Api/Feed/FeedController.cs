using Microsoft.AspNetCore.Mvc;
using MightyRSS._Api.Feed.Types;
using MightyRSS.Auth;
using System;

namespace MightyRSS._Api.Feed
{
    [Route("api/feed")]
    public sealed class FeedController : Controller
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
            var feedSource = _feedService.AddFeedSource(requestContext.User, request);
            if (feedSource == null)
                return BadRequest();

            return Created(feedSource.Reference.ToString(), feedSource);
        }

        [HttpGet]
        [Route("")]
        [ServiceFilter(typeof(Authorisation))]
        public IActionResult GetFeed([FromServices] IRequestContext requestContext)
        {
            var feed = _feedService.GetFeed(requestContext.User);

            return Ok(feed);
        }

        [HttpDelete]
        [Route("source/{reference:guid}")]
        [ServiceFilter(typeof(Authorisation))]
        public IActionResult DeleteFeedSource([FromServices] IRequestContext requestContext, [FromRoute] Guid reference)
        {
            _feedService.DeleteFeedSource(requestContext.User, reference);

            return NoContent();
        }
    }
}