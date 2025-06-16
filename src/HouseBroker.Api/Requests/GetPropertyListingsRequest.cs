using HouseBroker.Domain.Enums;

namespace HouseBroker.Api.Requests;

public record GetPropertyListingsRequest(
    Guid? Guid,
    PropertyType? PropertyType,
    int? PriceFrom,
    int? PriceTo,
    string? Country,
    string? City,
    string? Street
);
