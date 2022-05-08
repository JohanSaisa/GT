using AutoMapper;
using AutoMapper.QueryableExtensions;
using GT.Core.DTO.Company;
using GT.Core.Services.Interfaces;
using GT.Data.Data.AppDb.Entities;
using GT.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GT.Core.Services.Impl
{
	public class CompanyService : ICompanyService
	{
		private readonly IMapper _mapper;
		private readonly IGenericRepository<Company> _companyRepository;
		private readonly IGenericRepository<Location> _locationRepository;

		public CompanyService(
			IMapper mapper,
			IGenericRepository<Company> companyRepository,
			IGenericRepository<Location> locationRepository)
		{
			_mapper = mapper;
			_companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
			_locationRepository = locationRepository ?? throw new ArgumentNullException(nameof(locationRepository));
		}

		public async Task<bool> AddAsync(PostCompanyDTO dto)
		{
			if (string.IsNullOrWhiteSpace(dto.Name))
			{
				throw new ArgumentException($"Name cannot be null or empty.");
			}

			dto.Name = dto.Name.Trim();

			if (await ExistsByNameAsync(dto.Name))
			{
				throw new ArgumentException($"Company with name {dto.Name} already exists.");
			}

			var companyEntityToAdd = new Company()
			{
				Id = Guid.NewGuid().ToString(),
				Name = dto.Name
			};

			foreach (var locationName in dto.Locations!)
			{
				var locationEntity = await _locationRepository.Get()
					.Where(e => e.Name == locationName).SingleOrDefaultAsync();

				if (locationEntity is null)
				{
					throw new Exception($"No Company with name '{dto.Name}' was found.");
				}

				companyEntityToAdd.Locations!.Add(locationEntity);
			}

			await _companyRepository.AddAsync(companyEntityToAdd);

			return await _companyRepository.SaveAsync();
		}

		public async Task<List<CompanyDTO>> GetAllAsync()
		{
			var companyDTOs = await _companyRepository.Get()
				.Include(e => e.Locations)
				.ProjectTo<CompanyDTO>(_mapper.ConfigurationProvider)
				.ToListAsync();

			return companyDTOs;
		}

		public async Task<CompanyDTO?> GetByIdAsync(string id)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				throw new ArgumentException("Id cannot be null or empty.");
			}

			var dto = await _companyRepository
				.Get()
				.Where(e => e.Id == id)
				.Include(e => e.Locations)
				.ProjectTo<CompanyDTO>(_mapper.ConfigurationProvider)
				.SingleOrDefaultAsync();

			return dto;
		}

		public async Task<bool> ExistsByNameAsync(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentException($"Name cannot be null or empty.");
			}

			name = name.Trim();

			return await _companyRepository.Get().Where(e => e.Name == name).AnyAsync();
		}

		public async Task<bool> DeleteAsync(string id)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				throw new ArgumentException("Id cannot be null or empty.");
			}

			var entity = await _companyRepository.Get()
				.Include(e => e.Locations)
				.FirstOrDefaultAsync(e => e.Id == id);

			if (entity is null)
			{
				throw new ArgumentException($"No Company with id '{id}' was found.");
			}

			_companyRepository.Delete(entity);

			return await _companyRepository.SaveAsync();
		}

		public async Task<bool> UpdateAsync(PostCompanyDTO dto, string id)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				throw new ArgumentException($"Id cannot be null or empty.");
			}

			if (dto is null)
			{
				throw new ArgumentNullException("Company object cannot be null.");
			}

			if (string.IsNullOrWhiteSpace(dto.Name))
			{
				throw new ArgumentNullException($"Name cannot be null or empty.");
			}

			dto.Name = dto.Name.Trim();

			// Check if another company exists with the same name as the DTO to prevent duplicate companies
			if (await _companyRepository.Get().AnyAsync((e => e.Id != id && e.Name == dto.Name)))
			{
				throw new ArgumentException($"Company with name: {dto.Name} already exist.");
			}

			var entityToUpdate = await _companyRepository.Get().Where(e => e.Id == id).SingleOrDefaultAsync();

			if (entityToUpdate is null)
			{
				throw new ArgumentException($"No Company with id '{id}' was found.");
			}

			entityToUpdate.Name = dto.Name;

			foreach (var locationName in dto.Locations!)
			{
				var location = await _locationRepository.Get().Where(e => e.Name == locationName).SingleOrDefaultAsync();
				if (location is null)
				{
					throw new Exception($"No Company with name '{locationName}' was found.");
				}

				entityToUpdate.Locations!.Add(location);
			}

			_companyRepository.Update(entityToUpdate);

			return await _companyRepository.SaveAsync();
		}

		public Task<bool> UpdateCompanyLocationsAsync(PatchCompanyLocationsDTO dto, string id)
		{
			throw new NotImplementedException();
		}

		public Task<bool> UpdateCompanyNameAsync(PatchCompanyNameDTO dto, string id)
		{
			throw new NotImplementedException();
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

		private void AddFileToFolder(IFormFile file, string fileNameWithPath)
		{
			throw new NotImplementedException();
			//using (var stream = new FileStream(fileNameWithPath + ".png", FileMode.Create))
			//{
			//	file.CopyTo(stream);
			//}
		}
	}
}
