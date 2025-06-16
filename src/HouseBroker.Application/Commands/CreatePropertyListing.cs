using Api.Shared.Exceptions;
using HouseBroker.Application.Dtos;
using HouseBroker.Application.Extensions;
using HouseBroker.Application.Repositories;
using HouseBroker.Domain;
using HouseBroker.Domain.Enums;
using HouseBroker.Domain.Exceptions;
using HouseBroker.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace HouseBroker.Application.Commands;

public static class CreatePropertyListing
{
    public record Command(
        string UserId,
        string Name,
        string CurrencyCode,
        decimal Price,
        PropertyType PropertyType,
        IEnumerable<string> ImageUrls,
        string Country,
        string Street,
        string City
    ) : IRequest<PropertyListingDto>;

    public class Handler : IRequestHandler<Command, PropertyListingDto>
    {
        private readonly IPropertyListingWriteRepository _writeRepository;
        private readonly ILogger<Handler> _logger;

        public Handler(
            IPropertyListingWriteRepository writeRepository,
            ILogger<Handler> logger)
        {
            _writeRepository = writeRepository;
            _logger = logger;
        }

        public async Task<PropertyListingDto> Handle(Command request, CancellationToken cancellationToken)
        {
            try
            {
                var address = PropertyListingAddress.Create(
                    country: request.Country,
                    street: request.Street,
                    city: request.City
                );

                var listing = PropertyListing.Create(
                    name: request.Name,
                    currencyCode: request.CurrencyCode,
                    price: request.Price,
                    propertyType: request.PropertyType,
                    imageUrls: request.ImageUrls,
                    propertyListingAddress: address,
                    createdBy: request.UserId
                );

                _writeRepository.Add(listing);

                await _writeRepository.SaveChangesAsync(cancellationToken);

                return listing.ToDto();
            }
            catch (DomainValidationException ex)
            {

                throw new AppValidationException(
                    identifier: ex.Identifier,
                    message: ex.Message
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
