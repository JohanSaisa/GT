using GT.Core.DTO.Listing;
using GT.Core.FilterModels.Interfaces;

namespace GT.Core.Services.Interfaces
{
	public interface IGTListingService
	{
		Task<bool> AddAsync(PostListingDTO dto, string signedInUserId);

		Task<List<ListingOverviewDTO>> GetAllByFilterAsync(IListingFilterModel? filter = null);

		Task<ListingDTO?> GetByIdAsync(string id);

		Task<bool> ExistsByIdAsync(string id);

		Task<bool> DeleteAsync(string id);

		Task<bool> UpdateAsync(PostListingDTO dto, string id);
	}
}
