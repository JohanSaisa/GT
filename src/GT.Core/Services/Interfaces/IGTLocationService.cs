using GT.Core.DTO.Impl;

namespace GT.Core.Services.Interfaces
{
	public interface IGTLocationService
	{
		Task<bool> AddAsync(PostLocationDTO dto);

		Task<List<LocationDTO>> GetAllAsync();

		Task<LocationDTO> GetByIdAsync(string id);

		Task<bool> ExistsByNameAsync(string name);

		Task<bool> DeleteAsync(string id);

		Task<bool> UpdateAsync(PostLocationDTO dto, string id);
	}
}
