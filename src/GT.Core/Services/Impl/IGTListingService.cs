using GT.Core.DTO;
using GT.Core.FilterModels.Interfaces;

namespace GT.Core.Services.Impl
{
	public interface IGTListingService
	{
		Task<List<ListingPartialDTO>> GetAsync(IListingFilterModel? filter = null);
		Task<ListingDTO> GetByIdAsync(string id);
		Task<ListingDTO> AddAsync(ListingDTO listingDTO);
		Task UpdateAsync(ListingDTO listingDTO, string id);
		Task DeleteAsync(string id);
	}
}
