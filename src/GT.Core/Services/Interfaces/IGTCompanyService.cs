using GT.Core.DTO.Impl;

namespace GT.Core.Services.Interfaces
{
	public interface IGTCompanyService : 
		IGTService<CompanyDTO>,
		IHasGetAllAsyncWithoutParameters<CompanyDTO>,
		IHasAddAsyncWithoutUserId<CompanyDTO>,
		IHasExistsByNameAsync
	{
		Task<bool> AddCompanyLogoAsync(CompanyLogoDTO dto);

		Task<bool> DeleteCompanyLogoAsync(string id);
	}
}
