using GT.Core.DTO.Impl;
using GT.Core.FilterModels.Interfaces;

namespace GT.Core.Services.Interfaces
{
	public interface IGTExperienceLevelService : IGTService
	{
		Task<List<ExperienceLevelDTO>> GetAsync(IExperienceLevelFilterModel? filter = null);
		Task<ExperienceLevelDTO> GetByIdAsync(string id);
		Task<ExperienceLevelDTO> AddAsync(ExperienceLevelDTO dto);
		Task UpdateAsync(ExperienceLevelDTO dto, string id);
		Task DeleteAsync(string id);
		Task<bool> ExistsByNameAsync(string name);
	}
}
