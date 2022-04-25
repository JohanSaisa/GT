using GT.Core.DTO.Interfaces;

namespace GT.Core.Services.Interfaces;

public interface IHasGetAllAsyncWithoutParameters<TDataTransferObject>
	where TDataTransferObject : IGTDataTransferObject
{
	Task<List<TDataTransferObject>?> GetAllAsync();
}
