using GT.Core.DTO.Impl;
using Microsoft.AspNetCore.Http;

namespace GT.Core.Services.Interfaces
{
	public interface IGTCompanyService : IGTService
	{
		Task<CompanyDTO> AddAsync(CompanyDTO dto);

		Task<List<CompanyDTO>> GetAsync();

		Task<CompanyDTO> GetByIdAsync(string companyId);

		Task<bool> ExistsByNameAsync(string name);

		void AddCompanyLogo(CompanyLogoDTO companyLogoDTO);

		void DeleteCompanyLogo(string companyLogoId);

		Task DeleteAsync(string companyId);
	}
}


