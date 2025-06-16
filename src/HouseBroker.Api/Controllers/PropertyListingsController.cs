using System.Security.Claims;
using HouseBroker.Api.Extensions;
using HouseBroker.Api.Requests;
using HouseBroker.Application.Commands;
using HouseBroker.Application.Queries;
using HouseBroker.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseBroker.Api.Controllers;

[ApiController]
[Route("api/v1/property-listings")]
public class PropertyListingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PropertyListingsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("filter")]
    public async Task<IActionResult> GetPropertyListing([FromBody] GetPropertyListingsRequest request, CancellationToken cancellationToken)
    {
        var query = new GetPropertyListings.Query(
            Guid: request.Guid,
            PropertyType: request.PropertyType,
            PriceFrom: request.PriceFrom,
            PriceTo: request.PriceTo,
            Country: request.Country,
            City: request.City,
            Street: request.Street
        );
        
        var items = await _mediator.Send(query, cancellationToken);
        var viewModels = items.Select(x => x.ToViewModel());
        return Ok(viewModels);
    }

    [HttpPost]
    [Authorize(Roles = "Broker")]
    public async Task<IActionResult> CreatePropertyListing([FromBody] CreatePropertyListingRequest request, CancellationToken cancellationToken)
    {
        var command = new CreatePropertyListing.Command(
            Name: request.Name,
            CurrencyCode: request.CurrencyCode,
            Price: request.Price,
            PropertyType: request.PropertyType,
            ImageUrls: request.ImageUrls,
            Country: request.Country,
            Street: request.Street,
            City: request.City,
            UserId: GetUserId() 
        );

        var createdId = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetPropertyListing), new { id = createdId }, null);
    }

    [HttpPut("{guid}")]
    [Authorize(Roles = "Broker")]
    public async Task<IActionResult> UpdatePropertyListing(
        Guid guid,
        [FromBody] UpdatePropertyListingRequest request, CancellationToken cancellationToken)
    {

        var command = new UpdatePropertyListing.Command(
            Guid: guid,
            Name: request.Name,
            CurrencyCode: request.CurrencyCode,
            Price: request.Price,
            PropertyType: request.PropertyType,
            ImageUrls: request.ImageUrls,
            Country: request.Country,
            Street: request.Street,
            City: request.City,
            UserId: GetUserId()
        );

        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    
    [HttpDelete("{guid}")]
    [Authorize(Roles = "Broker")]
    public async Task<IActionResult> RemovePropertyListing(
        Guid guid, CancellationToken cancellationToken)
    {

        var command = new RemovePropertyListing.Command(
            Guid: guid,
            UserId: GetUserId()
        );

        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    private string GetUserId()
    {
        return User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
    }
}
