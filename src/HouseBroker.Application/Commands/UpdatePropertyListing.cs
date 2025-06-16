using Api.Shared.Exceptions;
using HouseBroker.Application.Dtos;
using HouseBroker.Application.Extensions;
using HouseBroker.Application.Repositories;
using HouseBroker.Application.Specifications;
using HouseBroker.Application.Specifications.Abstracts;
using HouseBroker.Domain;
using HouseBroker.Domain.Enums;
using HouseBroker.Domain.Exceptions;
using HouseBroker.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace HouseBroker.Application.Commands;

public static class UpdatePropertyListing
{
    public record Command(
        Guid Guid,
        string Name,
        string CurrencyCode,
        decimal Price,
        PropertyType PropertyType,
        IEnumerable<string> ImageUrls,
        string Country,
        string Street,
        string City,
        string UserId
    ) : IRequest<PropertyListingDto>;

    public class Handler : IRequestHandler<Command, PropertyListingDto>
    {
        private readonly IPropertyListingReadRepository _readRepository;
        private readonly IPropertyListingWriteRepository _writeRepository;
        private readonly ILogger<Handler> _logger;

        public Handler(
            IPropertyListingReadRepository readRepository,
            IPropertyListingWriteRepository writeRepository,
            ILogger<Handler> logger
        )
        {
            _readRepository = readRepository;
            _writeRepository = writeRepository;
            _logger = logger;
        }

        public async Task<PropertyListingDto> Handle(Command request, CancellationToken cancellationToken)
        {
            try
            {
                var listings =
                    (await _readRepository.GetAllAsync(
                        new List<ISpecification<PropertyListing>>()
                        {
                            ShouldHaveGuidSpecification.Create(request.Guid),
                            ShouldBeCreatedBySpecification.Create(request.UserId)  
                        },
                        cancellationToken));
                var listing = listings.FirstOrDefault();

                if (listing is null)
                    throw new AppValidationException(
                        message: "Listing not found",
                        identifier: nameof(request.Guid)
                    );

                var address = PropertyListingAddress.Create(
                    country: request.Country,
                    street: request.Street,
                    city: request.City
                );


                var updated = listing.Update(
                    name: request.Name,
                    currencyCode: request.CurrencyCode,
                    price: request.Price,
                    propertyType: request.PropertyType,
                    imageUrls: request.ImageUrls,
                    propertyListingAddress: address,
                    updatedBy: request.UserId
                );

                _writeRepository.Update(updated);
                await _writeRepository.SaveChangesAsync(cancellationToken);

                return updated.ToDto();
            }
            catch (DomainValidationException ex)
            {
                throw new AppValidationException(ex.Identifier, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}