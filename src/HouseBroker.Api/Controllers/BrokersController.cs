using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HouseBroker.Api.Controllers;

[ApiController]
[Route("api/v1/brokers")]
[Authorize]
public class BrokersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ILogger<BrokersController> _logger;

    public BrokersController(
        UserManager<IdentityUser> userManager,
        ILogger<BrokersController> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }
    
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetBroker(
        string userId
        )
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            return Ok(new { Email = user.Email, Id = user.Id });
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
    }
}
