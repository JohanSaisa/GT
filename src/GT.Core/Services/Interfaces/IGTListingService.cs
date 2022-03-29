using GT.Core.DTO.Impl;
using GT.Core.FilterModels.Interfaces;

namespace GT.Core.Services.Interfaces
{
	public interface IGTListingService : IGTService
	{
		/// <summary>
		/// Gets partial listings for list view in UI.
		/// </summary>
		/// <param name="filter"></param>
		Task<List<ListingPartialDTO>> GetAsync(IListingFilterModel? filter = null);
		Task<ListingDTO> GetByIdAsync(string id);
		Task<ListingDTO> AddAsync(ListingDTO listingDTO, string signedInUserId);
		Task UpdateAsync(ListingDTO listingDTO, string id);
		Task DeleteAsync(string id);
		Task<bool> ExistsById(string id);
	}
}
