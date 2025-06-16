using Api.Shared.Exceptions;
using HouseBroker.Application.Dtos;
using HouseBroker.Application.Extensions;
using HouseBroker.Application.Repositories;
using HouseBroker.Application.Specifications;
using HouseBroker.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace HouseBroker.Application.Commands;

public static class RemovePropertyListing
{
    public record Command(
        Guid Guid
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
                        ShouldHaveGuidSpecification.Create(request.Guid),
                        cancellationToken));

                var listing = listings.FirstOrDefault();

                if (listing is null)
                    throw new AppValidationException(
                        message: "Listing not found",
                        identifier: nameof(request.Guid)
                    );

                _writeRepository.Remove(listing);

                await _writeRepository.SaveChangesAsync(cancellationToken);

                return listing.ToDto();
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