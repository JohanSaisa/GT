using GT.Core.DTO.Impl;
using GT.Core.FilterModels.Interfaces;

namespace GT.Core.Services.Interfaces
{
	public interface IGTLocationService : IGTService
	{
		Task<List<LocationDTO>> GetAsync(ILocationFilterModel? filter = null);
		Task<LocationDTO> GetByIdAsync(string id);
		Task<LocationDTO> AddAsync(LocationDTO dto);
		Task UpdateAsync(LocationDTO dto, string id);
		Task DeleteAsync(string id);
		Task<bool> Exists(string name);
	}
}
