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

		/// <summary>
		/// Converts a DTO to entities and updates the database. 
		/// Requires the signed in users Id for assignment of CreatedBy property.
		/// </summary>
		/// <param name="listingDTO"></param>
		/// <returns>The input DTO with an updated Id.</returns>
		public async Task<ListingDTO?> AddAsync(ListingDTO listingDTO, string signedInUserId)
		{
			try
			{
				if (listingDTO is null)
				{
					_logger.LogWarning($"Attempted to add a null reference to the database.");
					return null;
				}
				if (signedInUserId is null)
				{
					_logger.LogWarning($"Attempted to add a new listing with a null reference on signedInUserId.");
					return null;
				}

				var newListing = await GetNewListingEntityWithSubEntities(listingDTO, signedInUserId);

				await _listingRepository.AddAsync(newListing);

				// Assigning the updated id to the DTO as it is the only property with a new value.
				listingDTO.Id = newListing.Id;
				return listingDTO;
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return null;
			}
		}

		private async Task<Listing?> AddSubEntitiesToListing(ListingDTO listingDTO, Listing entity)
		{
			try
			{
				if (listingDTO.Employer is not null)
				{
					if (!await _companyService.ExistsByNameAsync(listingDTO.Employer))
					{
						await _companyService.AddAsync(new CompanyDTO() { Name = listingDTO.Employer });
					}
					entity.Employer = await _companyRepository.GetAll().Where(e => e.Name == listingDTO.Employer).FirstOrDefaultAsync();
				}

				// Add sub entity Location
				if (listingDTO.Location is not null)
				{
					if (!await _locationService.ExistsByNameAsync(listingDTO.Location))
					{
						await _locationService.AddAsync(new LocationDTO() { Name = listingDTO.Location });
					}
					entity.Location = await _locationRepository.GetAll().Where(e => e.Name == listingDTO.Location).FirstOrDefaultAsync();
				}

				// Add sub entity ExperienceLevel
				if (listingDTO.ExperienceLevel is not null)
				{
					if (!await _experienceLevelService.ExistsByNameAsync(listingDTO.ExperienceLevel))
					{
						await _experienceLevelService.AddAsync(new ExperienceLevelDTO() { Name = listingDTO.ExperienceLevel });
					}
					entity.ExperienceLevel = await _experienceLevelRepository.GetAll().Where(e => e.Name == listingDTO.ExperienceLevel).FirstOrDefaultAsync();
				}
				return entity;
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return null;
			}
		}

		public async Task DeleteAsync(string id)
		{
			try
			{
				if (_listingRepository.GetAll().Any(e => e.Id == id))
				{
					await _listingRepository.DeleteAsync(id);
				}
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
			}
		}

		public async Task<List<ListingOverviewDTO>> GetAsync(IListingFilterModel? filter = null)
		{
			// Get entities from database
			if (filter is null)
			{
				filter = new ListingFilterModel();
			}

			var query = _listingRepository
				.GetAll()
				.Include(e => e.ExperienceLevel)
				.Include(e => e.Location)
				.Include(e => e.Employer)

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

			var entities = new List<Listing>();

			try
			{
				entities = await query.ToListAsync();
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
			}

			// Map entities to DTOs

			var listings = new List<ListingOverviewDTO>();

			foreach (var entity in entities)
			{
				listings.Add(new ListingOverviewDTO
				{
					Id = entity.Id,
					ListingTitle = entity.ListingTitle,
					SalaryMax = entity.SalaryMax,
					SalaryMin = entity.SalaryMin,
					FTE = entity.FTE,
					CreatedDate = entity.CreatedDate,
					JobTitle = entity.JobTitle,
					EmployerName = entity.Employer == null ? null : entity.Employer.Name,
					Location = entity.Location == null ? null : entity.Location.Name,
					ExperienceLevel = entity.ExperienceLevel == null ? null : entity.ExperienceLevel.Name
				});
			}

			return listings;
		}

		public async Task<ListingDTO?> GetByIdAsync(string id)
		{
			try
			{

				// Get entity
				var entity = await _listingRepository
					.GetAll()
					.Include(e => e.Employer)
					.Include(e => e.Location)
					.Include(e => e.ExperienceLevel)
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

				return listing;
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return null;
			}
		}

		public async Task UpdateAsync(ListingDTO listingDTO, string id)
		{
			try
			{
				if (listingDTO.Id is not null || id is not null)
				{
					if (await ExistsByIdAsync(id))
					{
						var updatedEntity = await GetUpdatedListingEntityWithSubEntities(listingDTO);
						await _listingRepository.UpdateAsync(updatedEntity, id);
					}
				}
				else
				{
					_logger.LogWarning($"Arguments cannot be null when using the method: {nameof(UpdateAsync)}.");
				}

			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
			}
		}

		public async Task<bool> ExistsByIdAsync(string id)
		{
			try
			{
				return await _listingRepository.GetAll().Where(e => e.Id == id).AnyAsync();
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return false;
			}
		}

		/// <summary>
		/// Maps a DTO to an existing entity entity and sends a request to the repository to add the entity to the database.
		/// If the listing entitys sub-entities do not exist the method creates new entitites and populates the database.
		/// </summary>
		private async Task<Listing?> GetUpdatedListingEntityWithSubEntities(ListingDTO listingDTO)
		{
			if (listingDTO is null)
			{
				var message = $"Attempted to pass a null object to {nameof(GetUpdatedListingEntityWithSubEntities)}.";
				_logger.LogError(message);
				throw new ArgumentNullException(nameof(listingDTO), message);
			}

			var oldEntity = await _listingRepository.FindAsync(listingDTO.Id);

			if (oldEntity is null)
			{
				_logger.LogWarning($"Could not find the listing when calling the repository in method {nameof(GetUpdatedListingEntityWithSubEntities)}.");
				return null;
			}

			oldEntity.Id = listingDTO.Id;
			oldEntity.ListingTitle = listingDTO.ListingTitle;
			oldEntity.Description = listingDTO.Description;
			oldEntity.SalaryMin = listingDTO.SalaryMin;
			oldEntity.SalaryMax = listingDTO.SalaryMax;
			oldEntity.JobTitle = listingDTO.JobTitle;
			oldEntity.FTE = listingDTO.FTE;
			var newEntity = await AddSubEntitiesToListing(listingDTO, oldEntity);

			return newEntity;
		}

		/// <summary>
		/// Maps a DTO to a new instance of an entity and sends a request to the repository to add the entity. 
		/// If the listings entitys sub-entities do not exist the method creates new entitites and populates the database.
		/// </summary>
		private async Task<Listing?> GetNewListingEntityWithSubEntities(ListingDTO listingDTO, string? signedInUserId = null)
		{

			if (listingDTO is null)
			{
				var message = $"Attempted to pass a null object to {nameof(GetNewListingEntityWithSubEntities)}.";
				_logger.LogError(message);
				throw new ArgumentNullException(nameof(listingDTO), message);
			}

			var newListingEntity = new Listing();

			newListingEntity.Id = Guid.NewGuid().ToString();
			newListingEntity.ListingTitle = listingDTO.ListingTitle;
			newListingEntity.Description = listingDTO.Description;
			newListingEntity.SalaryMin = listingDTO.SalaryMin;
			newListingEntity.SalaryMax = listingDTO.SalaryMax;
			newListingEntity.JobTitle = listingDTO.JobTitle;
			newListingEntity.FTE = listingDTO.FTE;
			newListingEntity.CreatedDate = listingDTO.CreatedDate;
			newListingEntity.CreatedById = signedInUserId;
			newListingEntity = await AddSubEntitiesToListing(listingDTO, newListingEntity);

			return newListingEntity;
		}
	}
}

