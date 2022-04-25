using GT.Core.DTO.Impl;

namespace GT.Core.Services.Interfaces
{
	public interface IGTExperienceLevelService : IGTService
	{
		Task<List<ExperienceLevelDTO>> GetAllAsync();

		Task<ExperienceLevelDTO> AddAsync(ExperienceLevelDTO dto);

		Task<bool> ExistsByNameAsync(string name);
	}
}
