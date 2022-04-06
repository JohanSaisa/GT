using GT.Core.DTO.Impl;
using GT.Core.Services.Interfaces;
using GT.Data.Data.GTAppDb.Entities;
using GT.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
namespace GT.Core.Services.Impl
{
	public class GTCompanyService : IGTCompanyService
	{
		private readonly ILogger<GTCompanyService> _logger;
		private readonly IGTGenericRepository<Company> _companyRepository;

		public GTCompanyService(ILogger<GTCompanyService> logger,
			IGTGenericRepository<Company> companyRepository)
		{
			_logger = logger;
			_companyRepository = companyRepository;
		}

		public async Task<CompanyDTO> AddAsync(CompanyDTO dto)
		{
			try
			{
				if (dto is null || dto.Name == null)
				{
					_logger.LogWarning($"Attempted to add a null reference to the database.");
					return null;
				}

				if (await ExistsByNameAsync(dto.Name))
				{
					_logger.LogWarning($"Attempted to add a company whose name already exists in the database.");
					var entity = await _companyRepository.GetAll().Where(e => e.Name == dto.Name).FirstOrDefaultAsync();

					// TODO - Use IMapper
					if (entity is not null)
					{
						dto.Id = entity.Id;
						dto.Name = entity.Name;
					}

					return dto;
				}

				// TODO - Use IMapper
				var newEntity = new Company()
				{
					Id = Guid.NewGuid().ToString(),
					Name = dto.Name
				};

				await _companyRepository.AddAsync(newEntity);

				// Assigning the updated id to the DTO as it is the only property with a new value.
				dto.Id = newEntity.Id;
				return dto;
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return null;
			}
		}

		public async void AddCompanyLogo(CompanyLogoDTO companyLogoDTO)
		{
			string path = Path.Combine(Directory.GetCurrentDirectory(), "src/GT.UI/wwwroot/img");

			var company = await GetByIdAsync(companyLogoDTO.CompanyId);
			if(company == null)
			{
				return;
			}

			if(company.CompanyLogoId == null || company.CompanyLogoId == String.Empty)
			{
				company.CompanyLogoId = Guid.NewGuid().ToString();
			}


		}

		public async Task DeleteAsync(string id)
		{
			try
			{
				await _companyRepository.DeleteAsync(id);
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
			}
		}

		public void DeleteCompanyLogo(string companyLogoId)
		{
			throw new NotImplementedException();
		}

		public async Task<bool> ExistsByNameAsync(string name)
		{
			try
			{
				return await _companyRepository.GetAll().Where(c => c.Name == name).AnyAsync();
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return false;
			}
		}

		public async Task<List<CompanyDTO>> GetAsync()
		{
			try
			{
				var entities = await _companyRepository.GetAll().ToListAsync();
				var companyDTOs = new List<CompanyDTO>();

				foreach (var entity in entities)
				{
					// TODO add automapper
					companyDTOs.Add(new CompanyDTO
					{
						Id = entity.Id,
						Name = entity.Name,
						CompanyLogoId = entity.CompanyLogoId,
					});
				}

				return companyDTOs;
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return null;
			}
		}

		public async Task<CompanyDTO> GetByIdAsync(string id)
		{
			if (id is null)
			{
				_logger.LogWarning($"Attempted to get entity with null reference id argument. {nameof(GetByIdAsync)}");
				return null;
			}

			try
			{
				// Get entity
				var entity = await _companyRepository
					.GetAll()
					.FirstOrDefaultAsync(e => e.Id == id);

				if (entity == null)
				{
					_logger.LogInformation($"Entity with id {id} not found.");
					return null;
				}

				// Map entity to DTO
				var company = new CompanyDTO()
				{
					Id = entity.Id,
					Name = entity.Name,
					CompanyLogoId = entity.CompanyLogoId,
				};

				return company;
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return null;
			}
		}
	}
}
