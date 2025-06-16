using HouseBroker.Domain.Enums;

namespace HouseBroker.Api.Requests;

public record CreatePropertyListingRequest(
    string Name,
    string CurrencyCode,
    decimal Price,
    PropertyType PropertyType,
    IEnumerable<string> ImageUrls,
    string Street,
    string City,
    string Country
);