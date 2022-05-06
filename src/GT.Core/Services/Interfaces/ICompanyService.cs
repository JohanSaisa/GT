using GT.Core.DTO.Company;

namespace GT.Core.Services.Interfaces
{
	public interface ICompanyService
	{
		Task<bool> AddAsync(PostCompanyDTO dto);

		Task<List<CompanyDTO>> GetAllAsync();

		Task<CompanyDTO?> GetByIdAsync(string id);

		Task<bool> ExistsByNameAsync(string name);

		Task<bool> DeleteAsync(string id);

		Task<bool> UpdateAsync(PostCompanyDTO dto, string id);

		Task<bool> UpdateCompanyLocationsAsync(PatchCompanyLocationsDTO dto, string id);

		Task<bool> UpdateCompanyNameAsync(PatchCompanyNameDTO dto, string id);

		Task<bool> AddCompanyLogoAsync(CompanyLogoDTO dto);

		Task<bool> DeleteCompanyLogoAsync(string id);
	}
}
