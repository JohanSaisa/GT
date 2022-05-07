using GT.Core.DTO.ExperienceLevel;

namespace GT.Core.Services.Interfaces
{
	public interface IExperienceLevelService
	{
		Task<bool> AddAsync(PostExperienceLevelDTO dto);

		Task<List<ExperienceLevelDTO>> GetAllAsync();

		Task<ExperienceLevelDTO> GetByIdAsync(string id);

		Task<bool> ExistsByNameAsync(string name);

		Task<bool> DeleteAsync(string id);

		Task<bool> UpdateAsync(PostExperienceLevelDTO dto, string id);
	}
}
