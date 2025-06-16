using HouseBroker.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HouseBroker.Infrastructure.EntityConfigurations;

public class PropertyListingConfiguration : IEntityTypeConfiguration<PropertyListing>
{
    public void Configure(EntityTypeBuilder<PropertyListing> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .OwnsOne(x => x.PropertyListingAddress);
    }
}