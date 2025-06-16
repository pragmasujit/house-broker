using HouseBroker.Domain.Exceptions;
using HouseBroker.Domain.DomainValidators;

namespace HouseBroker.Domain.ValueObjects;

public sealed class PropertyListingAddress : IEquatable<PropertyListingAddress>
{
    public string Street { get; init; }
    public string City { get; init; }
    public string Country { get; init; }
    
    public PropertyListingAddress() { }
    
    public static PropertyListingAddress Create(string street, string city, string country)
    {
        var address = new PropertyListingAddress
        {
            Street = street,
            City = city,
            Country = country
        };

        var validator = new PropertyListingAddressValidator();
        var result = validator.Validate(address);

        if (!result.IsValid)
        {
            var failure = result.Errors[0];
            throw new DomainValidationException(failure.PropertyName, failure.ErrorMessage);
        }

        return address;
    }

    public override bool Equals(object? obj) => Equals(obj as PropertyListingAddress);

    public bool Equals(PropertyListingAddress? other) =>
        other != null &&
        Street == other.Street &&
        City == other.City &&
        Country == other.Country;

    public override int GetHashCode() => HashCode.Combine(Street, City, Country);
}

