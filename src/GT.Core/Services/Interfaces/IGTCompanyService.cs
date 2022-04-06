﻿using GT.Core.DTO.Impl;

namespace GT.Core.Services.Interfaces
{
	public interface IGTCompanyService : IGTService
	{
		Task<CompanyDTO> AddAsync(CompanyDTO dto);

		Task<CompanyDTO> GetAsync();

		Task<CompanyDTO> GetByIdAsync(string companyId);

		Task<bool> ExistsByNameAsync(string name);

		string AddCompanyLogo(byte[] file);

		void DeleteCompanyLogo(string companyLogoId);

		Task DeleteAsync(string companyId);
	}
}
