using GT.Core.DTO.Impl;
using GT.Core.FilterModels.Interfaces;
using GT.Data.Data.GTAppDb.Entities;

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

		/// <summary>
		/// Maps a listing DTO to a listing entity. If the listing entitys sub-entities do not exist 
		/// the method creates new entitites and populates the database.
		/// </summary>
		/// <param name="listingDTO"></param>
		/// <param name="signedInUserId">Current signed in user which will the the mapped to the CreatedBy property.</param>
		/// <returns></returns>
		Task<Listing> CreateListingEntityWithSubEntities(ListingDTO listingDTO, string signedInUserId);
	}
}
