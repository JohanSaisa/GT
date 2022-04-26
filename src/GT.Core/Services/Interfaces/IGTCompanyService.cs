using GT.Core.DTO.Impl;

namespace GT.Core.Services.Interfaces
{
	public interface IGTCompanyService : IGTService
	{
		Task<CompanyDTO> AddAsync(CompanyDTO dto);

		Task<List<CompanyDTO>> GetAllAsync();

		Task<CompanyDTO> GetByIdAsync(string id);

		Task<bool> ExistsByNameAsync(string name);

		Task<bool> AddCompanyLogoAsync(CompanyLogoDTO dto);

		Task<bool> DeleteCompanyLogoAsync(string id);

		Task DeleteAsync(string id);

		Task<bool> UpdateAsync(CompanyDTO dto, string id);
	}
}
