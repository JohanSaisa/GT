using GT.Core.DTO.Impl;

namespace GT.Core.Services.Interfaces
{
	public interface IGTCompanyService : IGTService
	{
		Task<CompanyDTO> AddAsync(CompanyDTO dto);
		Task<bool> ExistsByNameAsync(string name);
	}
}
