using GT.Core.DTO.Company;
using GT.Core.Services.Interfaces;
using GT.Data.Data.GTAppDb.Entities;
using GT.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GT.Core.Services.Impl
{
	public class CompanyService : ICompanyService
	{
		private readonly IGTGenericRepository<Company> _companyRepository;

		public CompanyService(IGTGenericRepository<Company> companyRepository)
		{
			_companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
		}

		public async Task<bool> AddAsync(PostCompanyDTO dto)
		{
			if (dto is null || string.IsNullOrEmpty(dto.Name))
			{
				throw new ArgumentNullException(nameof(dto));
			}

			dto.Name = dto.Name.Trim();
			if (await ExistsByNameAsync(dto.Name))
			{
				throw new ArgumentException($"Company with name {dto.Name} already exists.");
			}

			// TODO: Use IMapper
			var newEntity = new Company()
			{
				Id = Guid.NewGuid().ToString(),
				Name = dto.Name
			};

			await _companyRepository.AddAsync(newEntity);

			return await _companyRepository.SaveAsync();
		}

		public async Task<bool> AddCompanyLogoAsync(CompanyLogoDTO dto)
		{
			throw new NotImplementedException();
			//// Create folder if it does not exist
			//string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/CompanyLogos");
			//if (!Directory.Exists(path))
			//	Directory.CreateDirectory(path);

			//if (dto.Logo == null || dto.CompanyId == null)
			//{
			//	throw new ArgumentNullException(nameof(dto));
			//}

			//var company = await GetByIdAsync(dto.CompanyId);
			//if (company == null)
			//{
			//	throw new ArgumentNullException(nameof(AddCompanyLogoAsync));
			//	return false;
			//}

			//// Check if company already has a stored logo
			//if (company.CompanyLogoId != null || company.CompanyLogoId != String.Empty)
			//{
			//	AddFileToFolder(dto.File, Path.Combine(path, company.CompanyLogoId));
			//	return true;
			//}
			//else
			//{
			//	// Update DB entity with new LogoID
			//	company.CompanyLogoId = company.Id;
			//	await UpdateAsync(company, company.Name);

			//	AddFileToFolder(dto.File, Path.Combine(path, company.CompanyLogoId));
			//	return true;
			//}
		}

		public async Task<bool> DeleteAsync(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				throw new ArgumentException("No id to delete was submitted.");
			}

			var entity = await _companyRepository.Get()
				.Include(e => e.Locations)
				.Include(e => e.CompanyLogoId)
				.FirstOrDefaultAsync(e => e.Id == id);

			if (entity is null)
			{
				throw new ArgumentException($"Company with id {id} does not exist.");
			}

			await _companyRepository.DeleteAsync(entity);
			return await _companyRepository.SaveAsync();
		}

		public async Task<bool> DeleteCompanyLogoAsync(string id)
		{
			throw new NotImplementedException();
			//if (string.IsNullOrEmpty(id))
			//{
			//	throw new ArgumentException("No id to delete was submitted.");
			//}

			//string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/CompanyLogos");
			//string fileNameWithPath = Path.Combine(path, id);

			//using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
			//{
			//	try
			//	{
			//		File.Delete(fileNameWithPath);
			//		return true;
			//	}
			//	catch (Exception e)
			//	{
			//		_logger.LogError(e.Message);
			//		return false;
			//	}
			//}
		}

		public async Task<bool> ExistsByNameAsync(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException("No name was submitted.");
			}

			name = name.Trim();
			return await _companyRepository.Get()!.Where(e => e.Name == name).AnyAsync();
		}

		public async Task<List<CompanyDTO>> GetAllAsync()
		{
			// *********************************************************** TODO ****************************************************
			var companyDTOs = await _companyRepository.Get()!.ToListAsync();
			//var companyDTOs = new List<CompanyDTO>();

			//foreach (var entity in entities)
			//{
			//	// TODO add automapper
			//	companyDTOs.Add(new CompanyDTO
			//	{
			//		Id = entity.Id,
			//		Name = entity.Name,
			//		Locations = entity.CompanyLogoId,
			//	});
			//}

			return companyDTOs;
		}

		public async Task<CompanyDTO> GetByIdAsync(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				throw new ArgumentException("No id was submitted.");
			}

			// Get entity
			var entity = await _companyRepository
				.Get()
				.FirstOrDefaultAsync(e => e.Id == id);

			if (entity is null)
			{
				throw new Exception($"No entity with id: {id} was found.");
			}

			// TODO: Add Automapper
			var company = new CompanyDTO()
			{
				Id = entity.Id,
				Name = entity.Name,
			};

			return company;
		}

		public async Task<bool> UpdateAsync(PostCompanyDTO dto, string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				throw new ArgumentException("No id was submitted.");
			}

			if (dto is null)
			{
				throw new ArgumentNullException("No dto was submitted.");
			}

			if (await ExistsByNameAsync(dto.Name))
			{
					private var entityToUpdate = await _companyRepository.Get().FirstOrDefaultAsync(e => e.Id == dto.Id);

		// TODO implement automapper
		entityToUpdate.Id = dto.Id;
					entityToUpdate.Name = dto.Name;
					entityToUpdate.CompanyLogoId = dto.CompanyLogoId;

					await _companyRepository.UpdateAsync(entityToUpdate, entityToUpdate.Id);

					return true;
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
