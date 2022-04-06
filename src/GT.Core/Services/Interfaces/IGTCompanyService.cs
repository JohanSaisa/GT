using GT.Core.DTO.Impl;

namespace GT.Core.Services.Interfaces
{
	public interface IGTCompanyService : IGTService
	{
		Task<CompanyDTO> AddAsync(CompanyDTO dto);

		Task<CompanyDTO> GetAsync();

		Task<CompanyDTO> GetByIdAsync(string companyId);

		Task<bool> ExistsByNameAsync(string name);

		void AddCompanyLogo(CompanyLogoDTO file, string companyId);

		void DeleteCompanyLogo(string companyLogoId);

		Task DeleteAsync(string companyId);
	}
}


