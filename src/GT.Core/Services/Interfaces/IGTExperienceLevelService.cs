using GT.Core.DTO.Impl;

namespace GT.Core.Services.Interfaces
{
	public interface IGTExperienceLevelService : IGTService
	{
		Task<ExperienceLevelDTO> AddAsync(ExperienceLevelDTO dto);
		Task<bool> ExistsByNameAsync(string name);
	}
}
