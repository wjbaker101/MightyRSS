using Microsoft.AspNetCore.Mvc;
using MightyRSS.Api.Auth.Attributes;
using MightyRSS.Types;
using NetApiLibs.Api;

namespace MightyRSS.Api.Configuration;

[ApiController]
[Route("api/configuration")]
public sealed class ConfigurationController : ApiController
{
    private readonly IConfigurationService _configurationService;

    public ConfigurationController(IConfigurationService configurationService)
    {
        _configurationService = configurationService;
    }

    [HttpGet]
    [Route("")]
    [Authorisation]
    public IActionResult GetConfiguration([FromServices] IRequestContext requestContext)
    {
        var result = _configurationService.GetConfiguration(requestContext);

        return ToApiResponse(result);
    }
}