using GT.Core.DTO.Impl;

namespace GT.Core.Services.Interfaces
{
	public interface IGTLocationService : IGTService
	{
		Task<List<LocationDTO>?> GetAllAsync();
		Task<LocationDTO> AddAsync(LocationDTO dto);
		Task<bool> ExistsByNameAsync(string name);
	}
}
