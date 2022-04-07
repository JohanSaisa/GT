using GT.Core.DTO.Impl;

namespace GT.Core.Services.Interfaces
{
	public interface IGTCompanyService : IGTService
	{
		Task<CompanyDTO> AddAsync(CompanyDTO dto);

		Task<List<CompanyDTO>> GetAsync();

		Task<CompanyDTO> GetByIdAsync(string companyId);

		Task<bool> ExistsByNameAsync(string name);

		Task<bool> AddCompanyLogo(CompanyLogoDTO companyLogoDTO);

		Task<bool> DeleteCompanyLogo(string companyLogoId);

		Task DeleteAsync(string companyId);

		Task<bool> UpdateAsync(CompanyDTO companyDTO, string id);

		Task<CompanyLogoDTO> GetCompanyLogo(string companyId);
	}
}
