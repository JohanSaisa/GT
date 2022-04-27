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
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
		}

		public async Task<CompanyDTO> AddAsync(CompanyDTO dto)
		{
			if (dto is null || string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrEmpty(dto.Name))
			{
				_logger.LogWarning($"Attempted to add a null reference to the database.");
				return null;
			}
			try
			{
				dto.Name = dto.Name.Trim();
				dto.CompanyLogoId = null;

				if (await ExistsByNameAsync(dto.Name))
				{
					_logger.LogWarning($"Attempted to add a company whose name already exists in the database.");
					var entity = await _companyRepository.Get().Where(e => e.Name == dto.Name).FirstOrDefaultAsync();

					// TODO: Use IMapper
					if (entity is not null)
					{
						dto.Id = entity.Id;
						dto.Name = entity.Name;
						dto.CompanyLogoId = entity.CompanyLogoId;
					}

					return dto;
				}

				// TODO: Use IMapper
				var newEntity = new Company()
				{
					Id = Guid.NewGuid().ToString(),
					Name = dto.Name,
					CompanyLogoId = dto.CompanyLogoId,
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

		public async Task<bool> AddCompanyLogoAsync(CompanyLogoDTO dto)
		{
			// Create folder if it does not exist
			string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/CompanyLogos");
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			if (dto.File == null || dto.CompanyId == null)
			{
				_logger.LogWarning($"Can not use null arguments in method: {nameof(AddCompanyLogoAsync)}");
				return false;
			}

			var company = await GetByIdAsync(dto.CompanyId);
			if (company == null)
			{
				_logger.LogWarning($"Could not find a company in method: {nameof(AddCompanyLogoAsync)}");
				return false;
			}

			// Check if company already has a stored logo
			if (company.CompanyLogoId != null || company.CompanyLogoId != String.Empty)
			{
				AddFileToFolder(dto.File, Path.Combine(path, company.CompanyLogoId));
				return true;
			}
			else
			{
				// Update DB entity with new LogoID
				company.CompanyLogoId = company.Id;
				await UpdateAsync(company, company.Name);

				AddFileToFolder(dto.File, Path.Combine(path, company.CompanyLogoId));
				return true;
			}
		}

		public async Task DeleteAsync(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				_logger.LogWarning($"Can not use null arguments in method: {nameof(DeleteAsync)}");
				return;
			}
			try
			{
				var entity = await _companyRepository.Get()
					.Include(e => e.Locations)
					.Include(e => e.CompanyLogoId)
					.FirstOrDefaultAsync(e => e.Id == id);

				if (entity is not null)
				{
					await _companyRepository.DeleteAsync(entity);
				}
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
			}
		}

		public async Task<bool> DeleteCompanyLogoAsync(string id)
		{
			if (id == null || id.Length <= 0)
			{
				_logger.LogWarning($"Arguments cannot be null when using the method: { nameof(DeleteCompanyLogoAsync)}.");
				return false;
			}

			string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/CompanyLogos");
			string fileNameWithPath = Path.Combine(path, id);

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
			if (string.IsNullOrEmpty(name))
			{
				_logger.LogWarning($"Can not use null arguments in method: {nameof(DeleteAsync)}");
				return false;
			}

			try
			{
				return await _companyRepository.Get().Where(e => e.Name == name).AnyAsync();
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return false;
			}
		}

		public async Task<List<CompanyDTO>> GetAllAsync()
		{
			try
			{
				var entities = await _companyRepository.Get().ToListAsync();
				var companyDTOs = new List<CompanyDTO>();

				if (entities is null)
				{
					return null;
				}
				
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
			if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id))
			{
				_logger.LogWarning($"Attempted to get entity with null reference id argument. {nameof(GetByIdAsync)}");
				return null;
			}

			try
			{
				id = id.Trim();

				var entity = await _companyRepository
					.Get()
					.FirstOrDefaultAsync(e => e.Id == id);

				if (entity is null)
				{
					_logger.LogInformation($"Entity with id {id} not found.");
					return null;
				}

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

		public async Task<bool> UpdateAsync(CompanyDTO dto, string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				_logger.LogWarning($"Can not use null arguments in method: {nameof(UpdateAsync)}.");
				return false;
			}

			if (dto.Id != id)
			{
				_logger.LogWarning($"IDs are not matching in method: {nameof(UpdateAsync)}.");
				return false;
			}

			try
			{				
				if (await ExistsByNameAsync(dto.Name))
				{
					var entityToUpdate = await _companyRepository.Get().FirstOrDefaultAsync(e => e.Id == dto.Id);

					// TODO implement automapper
					entityToUpdate.Name = dto.Name;
					entityToUpdate.CompanyLogoId = dto.CompanyLogoId;

					await _companyRepository.UpdateAsync(entityToUpdate, entityToUpdate.Id);
					return true;
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
