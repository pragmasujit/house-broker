using HouseBroker.Api.ViewModels;
using HouseBroker.Application.Dtos;

namespace HouseBroker.Api.Extensions;

public static class ViewModelMappingExtensions
{
    public static PropertyListingViewModel ToViewModel(this PropertyListingDto dto)
    {
        return new PropertyListingViewModel(
            Guid: dto.Guid,
            Name: dto.Name,
            Price: dto.Price,
            CurrencyCode: dto.CurrencyCode,
            CreatedAt: dto.CreatedAt,
            CreatedBy: dto.CreatedBy
        );
    }
}