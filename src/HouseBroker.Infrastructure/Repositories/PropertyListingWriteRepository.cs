using HouseBroker.Application.Repositories;
using HouseBroker.Domain;
using HouseBroker.Infrastructure.Data;

namespace HouseBroker.Infrastructure.Repositories;

public class PropertyListingWriteRepository: IPropertyListingWriteRepository
{
    private readonly HouseBrokerDbContext _houseBrokerDbContext;

    public PropertyListingWriteRepository(HouseBrokerDbContext houseBrokerDbContext)
    {
        _houseBrokerDbContext = houseBrokerDbContext;
    }
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _houseBrokerDbContext.SaveChangesAsync(cancellationToken);
    }

    public void Add(PropertyListing listing)
    {
        _houseBrokerDbContext.Add(listing);
    }

    public void Update(PropertyListing listing)
    {
        _houseBrokerDbContext.Update(listing);
    }

    public void Remove(PropertyListing propertyListing)
    {
        _houseBrokerDbContext.Remove(propertyListing);
    }
}