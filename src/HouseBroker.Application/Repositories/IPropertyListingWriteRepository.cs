using HouseBroker.Application.Repositories.Abstracts;
using HouseBroker.Domain;

namespace HouseBroker.Application.Repositories
{
    public interface IPropertyListingWriteRepository: IUnitOfWork
    {
        void Add(PropertyListing listing);
        void Update(PropertyListing listing);
        void Remove(PropertyListing listing);
    }
}