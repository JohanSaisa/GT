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

		/// <summary>
		/// Converts a DTO to entities and updates the database.
		/// Requires the signed in users Id for assignment of CreatedBy property.
		/// </summary>
		/// <param name="listingDTO"></param>
		/// <returns>The input DTO with an updated Id.</returns>
		Task<ListingDTO> AddAsync(ListingDTO listingDTO, string signedInUserId);
		Task UpdateAsync(ListingDTO listingDTO, string id);
		Task DeleteAsync(string id);
		Task<bool> ExistsByIdAsync(string id);
	}
}
