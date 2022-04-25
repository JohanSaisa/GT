using GT.Core.DTO.Interfaces;

namespace GT.Core.Services.Interfaces;

public interface IHasAddAsyncWithUserId<TDataTransferObject>
	where TDataTransferObject : IGTDataTransferObject
{
	/// <summary>
	/// Converts a DTO to entities and sends a request to update the database.
	/// Requires the signed in users Id for assignment of CreatedBy property.
	/// </summary>
	/// <returns>The input DTO with an updated Id.</returns>
	Task<TDataTransferObject> AddAsync(TDataTransferObject dto, string userId);
}
