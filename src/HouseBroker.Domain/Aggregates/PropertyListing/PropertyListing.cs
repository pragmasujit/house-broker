using HouseBroker.Domain.Abstracts;
using HouseBroker.Domain.Enums;
using HouseBroker.Domain.ValueObjects;
using HouseBroker.Shared.Validation;

namespace HouseBroker.Domain;

public class PropertyListing : AuditableEntity
{
    public string Name { get; init; }
    public string? Description { get; init; }
    public PropertyListingAddress PropertyListingAddress { get; init; }
    public decimal Price { get; init; }
    public string CurrencyCode { get; init; }
    public PropertyType PropertyType { get; init; }
    public IEnumerable<string> ImageUrls { get; init; } = new List<string>();

    public PropertyListing() { }

    public static PropertyListing Create(
        string name,
        string currencyCode,
        decimal price,
        PropertyType propertyType,
        IEnumerable<string> imageUrls,
        PropertyListingAddress propertyListingAddress,
        string createdBy
    )
    {
        imageUrls ??= new List<string>();

        var propertyListing = new PropertyListing
        {
            CreatedBy = createdBy,
            Name = name,
            CurrencyCode = currencyCode,
            Price = price,
            PropertyType = propertyType,
            ImageUrls = imageUrls,
            PropertyListingAddress = propertyListingAddress,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        var validator = new PropertyListingValidator();
        
        validator.ValidateAndThrow(propertyListing);
        
        return propertyListing;
    }

    public PropertyListing Update(
        string name,
        string currencyCode,
        decimal price,
        PropertyType propertyType,
        IEnumerable<string> imageUrls,
        string updatedBy,
        PropertyListingAddress propertyListingAddress
    )
    {
        imageUrls ??= new List<string>();

        var listing = new PropertyListing
        {
            Id = this.Id,
            Guid = this.Guid,
            Name = name,
            CurrencyCode = currencyCode,
            Price = price,
            PropertyType = propertyType,
            ImageUrls = imageUrls,
            PropertyListingAddress = propertyListingAddress,
            CreatedAt = this.CreatedAt,
            UpdatedAt = DateTime.UtcNow,
            UpdatedBy = updatedBy
        };
        
        var validator = new PropertyListingValidator();
        validator.ValidateAndThrow(listing);
        
        return listing;
    }
}
