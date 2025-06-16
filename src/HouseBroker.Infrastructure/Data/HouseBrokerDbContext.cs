using HouseBroker.Domain;
using HouseBroker.Infrastructure.EntityConfigurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HouseBroker.Infrastructure.Data;

public class HouseBrokerDbContext: IdentityDbContext
{
    public HouseBrokerDbContext(DbContextOptions<HouseBrokerDbContext> options)
        : base(options)
    {
        
    }

    public DbSet<PropertyListing> PropertyListings { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .ApplyConfiguration(new IdentityUserConfiguration())
            .ApplyConfiguration(new IdentityRoleConfiguration())
            .ApplyConfiguration(new IdentityUserRoleConfiguration())
            .ApplyConfiguration(new PropertyListingConfiguration());
    }
}