using GT.Core.DTO.Impl;
using GT.Core.FilterModels.Interfaces;

namespace GT.Core.Services.Interfaces
{
	public interface IGTListingService : 
		IGTService<ListingDTO>,
		IHasAddAsyncWithUserId<ListingDTO>
	{
		/// <summary>
		/// Get high level view of all relevant listings with possibly applied filter for display.
		/// </summary>
		/// <param name="filter">Optional filter paramater.</param>
		/// <returns>Returns all listings which match the provided filtered values. Returns all listings if no filter is provided.</returns>
		Task<List<ListingOverviewDTO>> GetAllByFilterAsync(IListingFilterModel? filter = null);

		/// <summary>
		/// Asks the repository if an entity with the assigned id exists.
		/// </summary>
		/// <returns>Confirmation as true if entity is found, false if not.</returns>
		Task<bool> ExistsByIdAsync(string id);
	}
}
