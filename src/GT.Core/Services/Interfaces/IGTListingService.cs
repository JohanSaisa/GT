using GT.Core.DTO.Impl;
using GT.Core.FilterModels.Interfaces;

namespace GT.Core.Services.Interfaces
{
	public interface IGTListingService : IGTService
	{
		/// <summary>
		/// Get high level view of all relevant listings with possibly applied filter for display.
		/// </summary>
		/// <param name="filter">Optional filter paramater.</param>
		/// <returns>Returns all listings which match the provided filtered values. Returns all listings if no filter is provided.</returns>
		Task<List<ListingOverviewDTO>> GetAllByFilterAsync(IListingFilterModel? filter = null);

		/// <summary>
		/// Sends request to the repository to find by id and map an entity to DTO from the database.
		/// </summary>
		Task<ListingDTO?> GetByIdAsync(string id);

		/// <summary>
		/// Converts a DTO to entities and sends a request to update the database.
		/// Requires the signed in users Id for assignment of CreatedBy property.
		/// </summary>
		/// <returns>The input DTO with an updated Id.</returns>
		Task<ListingDTO?> AddAsync(ListingDTO dto, string signedInUserId);

		/// <summary>
		/// Converts a DTO to entities and sends a request to update the database.
		/// Requires the listingId for model validation.
		/// </summary>
		Task UpdateAsync(ListingDTO dto, string id);

		/// <summary>
		/// Sends request to the repository to delete the entity from the database.
		/// </summary>
		Task DeleteAsync(string id);

		/// <summary>
		/// Asks the repository if an entity with the assigned id exists.
		/// </summary>
		/// <returns>Confirmation as true if entity is found, false if not.</returns>
		Task<bool> ExistsByIdAsync(string id);
	}
}
