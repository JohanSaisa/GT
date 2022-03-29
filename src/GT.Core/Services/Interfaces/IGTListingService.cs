using GT.Core.DTO.Impl;
using GT.Core.FilterModels.Interfaces;
using GT.Data.Data.GTAppDb.Entities;

namespace GT.Core.Services.Interfaces
{
	public interface IGTListingService : IGTService
	{
		/// <summary>
		/// Gets a list of listings meant for list view in UI, where some properties have been stripped from the view model.
		/// </summary>
		/// <param name="filter">Optional filter paramater.</param>
		/// <returns>Returns all listings which match the provided filtered values. Returns all listings if no filter is provided.</returns>
		Task<List<ListingPartialDTO>> GetAsync(IListingFilterModel? filter = null);

		Task<ListingDTO?> GetByIdAsync(string listingId);

		/// <summary>
		/// Converts a DTO to entities and updates the database.
		/// Requires the signed in users Id for assignment of CreatedBy property.
		/// </summary>
		/// <param name="listingDTO"></param>
		/// <returns>The input DTO with an updated Id.</returns>
		Task<ListingDTO?> AddAsync(ListingDTO listingDTO, string signedInUserId);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="listingDTO"></param>
		/// <param name="listingId"></param>
		/// <returns></returns>
		Task UpdateAsync(ListingDTO listingDTO, string listingId);
		Task DeleteAsync(string listingId);
		Task<bool> ExistsByIdAsync(string listingId);

		/// <summary>
		/// Generates and maps a listing entity. If the listing entitys sub-entities do not exist 
		/// the method creates new entitites and populates the database.
		/// </summary>
		/// <param name="listingDTO"></param>
		/// <param name="signedInUserId">Optional parameter used when creating a new listing.</param>
		/// <param name="listingId">Optional parameter used when updating an existing listing.</param>
		/// <returns></returns>
		Task<Listing> CreateListingEntityWithSubEntities(ListingDTO listingDTO, string? signedInUserId = null, string? listingId = null);

	}
}
