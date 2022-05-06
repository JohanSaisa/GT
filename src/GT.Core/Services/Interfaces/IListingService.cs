using GT.Core.DTO.Listing;
using GT.Core.FilterModels.Impl;

namespace GT.Core.Services.Interfaces
{
	public interface IListingService
	{
		Task<bool> AddAsync(PostListingDTO dto, string signedInUserId);

		Task<List<ListingOverviewDTO>> GetAllByFilterAsync(PostListingFilterDTO? filter);

		Task<ListingDTO?> GetByIdAsync(string id);

		Task<bool> ExistsByIdAsync(string id);

		Task<bool> DeleteAsync(string id);

		Task<bool> UpdateAsync(PostListingDTO dto, string id);
	}
}
