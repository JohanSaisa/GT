using GT.Core.DTO.Impl;

namespace GT.Core.Services.Interfaces
{
	public interface IGTExperienceLevelService : IGTService
	{
		Task<List<ExperienceLevelDTO>> GetAllAsync();

		Task<ExperienceLevelDTO?> GetByIdAsync(string id);

		Task<ExperienceLevelDTO> AddAsync(ExperienceLevelDTO dto);

		Task<bool> ExistsByNameAsync(string name);

		Task UpdateAsync(ExperienceLevelDTO experienceLevelDTO, string name);

		Task DeleteAsync(string id);
	}
}
