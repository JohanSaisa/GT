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

		public async Task<bool> AddCompanyLogo(CompanyLogoDTO companyLogoDTO)
		{
			// Create folder if it does not exist
			string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/CompanyLogos");
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			if (companyLogoDTO.File == null || companyLogoDTO.CompanyId == null)
			{
				_logger.LogWarning($"Can not use null arguments in method: {nameof(AddCompanyLogo)}");
				return false;
			}

			var company = await GetByIdAsync(companyLogoDTO.CompanyId);
			if (company == null)
			{
				_logger.LogWarning($"Could not find a company in method: {nameof(AddCompanyLogo)}");
				return false;
			}

			// Check if company already has a stored logo
			if (company.CompanyLogoId != null || company.CompanyLogoId != String.Empty)
			{
				AddFileToFolder(companyLogoDTO.File, Path.Combine(path, company.CompanyLogoId));
				return true;
			}
			else
			{
				// Create new ID for entity and image
				string companyLogoId, fileName;
				companyLogoId = fileName = Guid.NewGuid().ToString();

				// Update DB entity with new LogoID
				company.CompanyLogoId = companyLogoId;
				await UpdateAsync(company, company.Name);

				AddFileToFolder(companyLogoDTO.File, Path.Combine(path, fileName));
				return true;
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

		public async Task<bool> DeleteCompanyLogo(string companyLogoId)
		{
			if (companyLogoId == null || companyLogoId.Length <= 0)
			{
				_logger.LogWarning($"Arguments cannot be null when using the method: { nameof(DeleteCompanyLogo)}.");
				return false;
			}

			string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/CompanyLogos");
			string fileNameWithPath = Path.Combine(path, companyLogoId);

			using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
			{
				try
				{
					File.Delete(fileNameWithPath);
					return true;
				}
				catch (Exception e)
				{
					_logger.LogError(e.Message);
					return false;
				}
			}
		}

		public async Task<bool> ExistsByNameAsync(string name)
		{
			try
			{
				return await _companyRepository.GetAll().Where(e => e.Name == name).AnyAsync();
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

		public async Task<bool> UpdateAsync(CompanyDTO companyDTO, string name)
		{
			try
			{
				if (companyDTO.Id is not null && name is not null)
				{
					if (await ExistsByNameAsync(name))
					{
						var entityToUpdate = await _companyRepository.GetAll().Where(e => e.Id == companyDTO.Id).FirstOrDefaultAsync();

						// TODO implement automapper
						entityToUpdate.Id = companyDTO.Id;
						entityToUpdate.Name = name;
						entityToUpdate.CompanyLogoId = companyDTO.CompanyLogoId;

						await _companyRepository.UpdateAsync(entityToUpdate, entityToUpdate.Id);
						return true;
					}
					return false;
				}
				else
				{
					_logger.LogWarning($"Arguments cannot be null when using the method: {nameof(UpdateAsync)}.");
					return false;
				}
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return false;
			}
		}

		/// <summary>
		/// Creates a filestream and adds the file to the specified folder as a PNG.
		/// Will overwrite if file with the same name including filetype already exists.
		/// </summary>
		private void AddFileToFolder(IFormFile file, string fileNameWithPath)
		{
			using (var stream = new FileStream(fileNameWithPath + ".png", FileMode.Create))
			{
				file.CopyTo(stream);
			}
		}
	}
}
