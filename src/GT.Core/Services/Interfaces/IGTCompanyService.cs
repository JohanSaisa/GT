using GT.Core.DTO.Impl;
using GT.Core.FilterModels.Interfaces;

namespace GT.Core.Services.Interfaces
{
	public interface IGTCompanyService : IGTService
	{
		Task<List<CompanyDTO>> GetAsync(ICompanyFilterModel? filter = null);
		Task<CompanyDTO> GetByIdAsync(string id);
		Task<CompanyDTO> AddAsync(CompanyDTO dto);
		Task UpdateAsync(CompanyDTO dto, string id);
		Task DeleteAsync(string id);
		Task<bool> ExistsByNameAsync(string name);
	}
}
