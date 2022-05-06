using GT.Core.DTO.ExperienceLevel;

namespace GT.Core.Services.Interfaces
{
	public interface IGTExperienceLevelService : IGTService
	{
		Task<List<ExperienceLevelDTO>> GetAllAsync();

		Task<ExperienceLevelDTO?> GetByIdAsync(string id);

		Task<ExperienceLevelDTO> AddAsync(ExperienceLevelDTO dto);

		Task<bool> ExistsByNameAsync(string name);

		Task UpdateAsync(ExperienceLevelDTO dto, string name);

		Task DeleteAsync(string id);
	}
}
