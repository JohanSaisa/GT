using GT.Core.DTO.Interfaces;

namespace GT.Core.Services.Interfaces
{
	public interface IGTService<TDataTransferObject> 
		where TDataTransferObject : IGTDataTransferObject
	{
		/// <summary>
		/// Sends request to the repository to find by id and map an entity to DTO from the database.
		/// </summary>
		Task<TDataTransferObject?> GetByIdAsync(string id);

		/// <summary>
		/// Converts a DTO to entities and sends a request to update the database.
		/// Requires the listingId for model validation.
		/// </summary>
		Task UpdateAsync(TDataTransferObject dto, string id);

		/// <summary>
		/// Sends request to the repository to delete the entity from the database.
		/// </summary>
		Task DeleteAsync(string id);
	}
}
