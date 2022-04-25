using GT.Core.DTO.Interfaces;

namespace GT.Core.Services.Interfaces;

public interface IHasAddAsyncWithoutUserId<TDataTransferObject>
	where TDataTransferObject : IGTDataTransferObject
{
	/// <summary>
	/// Converts a DTO to entities and sends a request to update the database.
	/// </summary>
	/// <returns>The input DTO with an updated Id.</returns>
	Task<TDataTransferObject> AddAsync(TDataTransferObject dto);
}
