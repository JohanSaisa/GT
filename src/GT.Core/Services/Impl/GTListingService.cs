﻿using GT.Core.DTO.Impl;
using GT.Core.FilterModels.Impl;
using GT.Core.FilterModels.Interfaces;
using GT.Core.Services.Interfaces;
using GT.Data.Data.GTAppDb.Entities;
using GT.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GT.Core.Services.Impl
{
	/// <summary>
	/// Contains business logic for listing objects. Used to map and convert data transfer objects and entities.
	/// </summary>
	public class GTListingService : IGTListingService
	{
		private readonly ILogger<GTListingService> _logger;

		private readonly IGTCompanyService _companyService;
		private readonly IGTExperienceLevelService _experienceLevelService;
		private readonly IGTLocationService _locationService;

		private readonly IGTGenericRepository<Listing> _listingRepository;
		private readonly IGTGenericRepository<Company> _companyRepository;
		private readonly IGTGenericRepository<Location> _locationRepository;
		private readonly IGTGenericRepository<ExperienceLevel> _experienceLevelRepository;

		public GTListingService(
			ILogger<GTListingService> logger,
			IGTCompanyService companyService,
			IGTExperienceLevelService experienceLevelService,
			IGTLocationService locationService,
			IGTGenericRepository<Listing> listingRepository,
			IGTGenericRepository<Company> companyRepository,
			IGTGenericRepository<Location> locationRepository,
			IGTGenericRepository<ExperienceLevel> experienceLevelRepository)
		{
			_logger = logger;
			_companyService = companyService;
			_experienceLevelService = experienceLevelService;
			_locationService = locationService;
			_listingRepository = listingRepository;
			_companyRepository = companyRepository;
			_locationRepository = locationRepository;
			_experienceLevelRepository = experienceLevelRepository;
		}

		public async Task<ListingDTO> AddAsync(ListingDTO listingDTO, string signedInUserId)
		{
			try
			{
				if (listingDTO is null)
				{
					_logger.LogError($"Attempted to add a null reference to the database.");
					return null;
				}
				var newListing = await CreateListingEntityWithSubEntities(listingDTO, signedInUserId);
				await _listingRepository.AddAsync(newListing);

				// Assigning the updated id to the DTO.
				listingDTO.Id = newListing.Id;
			}
			catch (Exception e)
			{
				_logger.LogError($"{e.Message}");
				return null;
			}
			return listingDTO;
		}

		public async Task<Listing> CreateListingEntityWithSubEntities(ListingDTO listingDTO, string signedInUserId)
		{
			var newListing = new Listing();

			newListing.Id = Guid.NewGuid().ToString();
			newListing.ListingTitle = listingDTO.ListingTitle;
			newListing.Description = listingDTO.Description;
			newListing.SalaryMin = listingDTO.SalaryMin;
			newListing.SalaryMax = listingDTO.SalaryMax;
			newListing.JobTitle = listingDTO.JobTitle;
			newListing.FTE = listingDTO.FTE;
			newListing.CreatedDate = listingDTO.CreatedDate;
			newListing.CreatedById = signedInUserId;

			// Add sub entity Company
			try
			{
				if (listingDTO.Employer is not null)
				{
					if (!await _companyService.ExistsByNameAsync(listingDTO.Employer))
					{
						await _companyService.AddAsync(new CompanyDTO() { Name = listingDTO.Employer });
					}
					newListing.Employer = await _companyRepository.GetAll().Where(e => e.Name == listingDTO.Employer).FirstOrDefaultAsync();
				}
			}
			catch (Exception e)
			{
				_logger.LogError($"{e}");
			}

			// Add sub entity Location
			try
			{
				if (listingDTO.Location is not null)
				{
					if (!await _locationService.ExistsByNameAsync(listingDTO.Location))
					{
						await _locationService.AddAsync(new LocationDTO() { Name = listingDTO.Location });
					}
					newListing.Location = await _locationRepository.GetAll().Where(e => e.Name == listingDTO.Location).FirstOrDefaultAsync();
				}
			}
			catch (Exception e)
			{
				_logger.LogError($"{e}");
			}

			// Add sub entity ExperienceLevel
			try
			{
				if (listingDTO.ExperienceLevel is not null)
				{
					if (!await _experienceLevelService.ExistsByNameAsync(listingDTO.ExperienceLevel))
					{
						await _experienceLevelService.AddAsync(new ExperienceLevelDTO() { Name = listingDTO.ExperienceLevel });
					}
					newListing.ExperienceLevel = await _experienceLevelRepository.GetAll().Where(e => e.Name == listingDTO.ExperienceLevel).FirstOrDefaultAsync();
				}
			}
			catch (Exception e)
			{
				_logger.LogError($"{e}");
			}

			return newListing;
		}

		public async Task DeleteAsync(string id)
		{
			if (_listingRepository.GetAll().Any(e => e.Id == id))
			{
				await _listingRepository.DeleteAsync(id);
			}
		}

		public async Task<List<ListingPartialDTO>> GetAsync(IListingFilterModel? filter = null)
		{
			// Gets entities from database
			if (filter is null)
			{
				filter = new ListingFilterModel();
			}

			var query = _listingRepository
				.GetAll()
				.Include(e => e.ExperienceLevel)
				.Include(e => e.Location)

				.Where(e => filter.ExperienceLevels == null
					|| filter.ExperienceLevels.Count <= 0
					|| (e.ExperienceLevel != null && (e.ExperienceLevel.Name != null
					|| filter.ExperienceLevels.Contains(e.ExperienceLevel.Name))))

				.Where(e =>
					filter.FTE == null
					|| e.FTE == filter.FTE)

				.Where(e =>
					filter.Location == null
					|| (e.Location != null
						&& e.Location.Name != null
						&& string.Equals(filter.Location, e.Location.Name)))

				.Where(e =>
					filter.SalaryMin == null
					|| (e.SalaryMax != null
						&& e.SalaryMax >= filter.SalaryMin))

				.Where(e =>
					filter.SalaryMax == null
					|| (e.SalaryMin != null
						&& e.SalaryMin <= filter.SalaryMax))

				.Where(e => filter.IncludeListingsFromDate == null
					|| (e.CreatedDate != null
						&& filter.IncludeListingsFromDate < e.CreatedDate));

			if (filter.Keywords is not null)
			{
				foreach (var keyword in filter.Keywords)
				{
					if (keyword is not null)
					{
						query = query.Where(e =>
							(e.JobTitle != null && e.JobTitle.Contains(keyword))
							|| (e.Description != null && e.Description.Contains(keyword))
							|| (e.Location != null && e.Location.Name != null && e.Location.Name.Contains(keyword)));
					}
				}
			}

			var entities = await query.ToListAsync();

			// Map entities to DTOs

			var listings = new List<ListingPartialDTO>();

			foreach (var entity in entities)
			{
				listings.Add(new ListingPartialDTO
				{
					Id = entity.Id,
					ListingTitle = entity.ListingTitle,
					SalaryMax = entity.SalaryMax,
					SalaryMin = entity.SalaryMin,
					FTE = entity.FTE,
					CreatedDate = entity.CreatedDate,
					JobTitle = entity.JobTitle,
					Location = entity.Location.Name,
					ExperienceLevel = entity.ExperienceLevel.Name
				});
			}

			// Return results

			return listings;
		}

		public async Task<ListingDTO> GetByIdAsync(string id)
		{
			// Get entity
			var entity = await _listingRepository
				.GetAll()
				.Include(e => e.Employer)
				.Include(e => e.Location)
				.FirstOrDefaultAsync(e => e.Id == id);

			if (entity == null)
			{
				_logger.LogInformation($"Entity with id {id} not found.");
				return null;
			}

			// Map entity to DTO

			var listing = new ListingDTO()
			{
				Id = entity.Id,
				ListingTitle = entity.ListingTitle,
				Description = entity.Description,
				Employer = entity.Employer?.Name,
				SalaryMin = entity.SalaryMin,
				SalaryMax = entity.SalaryMax,
				JobTitle = entity.JobTitle,
				Location = entity.Location?.Name,
				FTE = entity.FTE,
				CreatedDate = entity.CreatedDate,
				ExperienceLevel = entity.ExperienceLevel?.Name
			};

			// Return DTO
			return listing;
		}

		public async Task UpdateAsync(ListingDTO listingDTO, string id)
		{

			throw new NotImplementedException();

			//// TODO - Check this link out https://docs.microsoft.com/en-us/archive/msdn-magazine/2009/brownfield/n-tier-application-patterns

			//// Check if exists. Error handling.

			//if (listingDTO.Id == id && _listingRepository.GetAll().Any(e => e.Id == id))
			//{
			//	// TODO map to entity

			//	await _listingRepository.UpdateAsync( , id);

			//}

		}

		public async Task<bool> ExistsByIdAsync(string id)
		{
			return await _listingRepository.GetAll().Where(e => e.Id == id).AnyAsync();
		}
	}
}

