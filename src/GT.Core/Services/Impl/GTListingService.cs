using GT.Core.DTO.Impl;
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
		private readonly IGTGenericRepository<ListingInquiry> _inquiryRepository;

		public GTListingService(
			ILogger<GTListingService> logger,
			IGTCompanyService companyService,
			IGTExperienceLevelService experienceLevelService,
			IGTLocationService locationService,
			IGTGenericRepository<Listing> listingRepository,
			IGTGenericRepository<Company> companyRepository,
			IGTGenericRepository<Location> locationRepository,
			IGTGenericRepository<ExperienceLevel> experienceLevelRepository,
			IGTGenericRepository<ListingInquiry> inquiryRepository)
		{
			_logger = logger;
			_companyService = companyService;
			_experienceLevelService = experienceLevelService;
			_locationService = locationService;
			_listingRepository = listingRepository;
			_companyRepository = companyRepository;
			_locationRepository = locationRepository;
			_experienceLevelRepository = experienceLevelRepository;
			_inquiryRepository = inquiryRepository;
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
					var employerName = listingDTO.Employer.Trim();
					if (!await _companyService.ExistsByNameAsync(employerName))
					{
						await _companyService
							.AddAsync(new CompanyDTO() { Name = employerName });
					}

					entity.Employer = await _companyRepository
						.GetAll()
						.FirstOrDefaultAsync(e => e.Name == employerName);
				}

				// Add sub entity Location
				if (listingDTO.Location is not null)
				{
					var locationName = listingDTO.Location.Trim();
					if (!await _locationService.ExistsByNameAsync(locationName))
					{
						await _locationService
							.AddAsync(new LocationDTO() { Name = locationName });
					}

					entity.Location = await _locationRepository
						.GetAll()
						.FirstOrDefaultAsync(e => e.Name == locationName);
				}

				// Add sub entity ExperienceLevel
				if (listingDTO.ExperienceLevel is not null)
				{
					var experienceLevelName = listingDTO.ExperienceLevel.Trim();
					if (!await _experienceLevelService.ExistsByNameAsync(experienceLevelName))
					{
						await _experienceLevelService
							.AddAsync(new ExperienceLevelDTO() { Name = experienceLevelName });
					}

					entity.ExperienceLevel = await _experienceLevelRepository
						.GetAll()
						.FirstOrDefaultAsync(e => e.Name == experienceLevelName);
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
					var linkedInquiries = _inquiryRepository
						.GetAll()
						.Include(e => e.Listing)
						.Where(e => e.Listing.Id == id)
						.Select(e => e.Id).ToArray();

					foreach (var inquiryId in linkedInquiries)
					{
						await _inquiryRepository.DeleteAsync(inquiryId);
					}

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
			// TODO - Refactor code and split up into multiple methods which modifys an IQueryable.
			// Suggesting that the methods should be implemented in the FilterModel as static methods.

			if (filter is null)
			{
				filter = new ListingFilterModel();
			}

			var query = _listingRepository?
				.GetAll()?
				.Include(e => e.ExperienceLevel)
				.Include(e => e.Location)
				.Include(e => e.Employer)

				.Where(e => 
					filter.ExperienceLevels == null
					|| filter.ExperienceLevels.Count <= 0
					|| (e.ExperienceLevel != null && e.ExperienceLevel.Name != null
						&& filter.ExperienceLevels.Any(el => string.Equals(e.ExperienceLevel.Name, el))))

				.Where(e =>
					filter.FTE == null
					|| e.FTE == filter.FTE)

				.Where(e =>
					filter.Location == null
					|| (e.Location != null
						&& e.Location.Name != null
						&& EF.Functions.Like(e.Location.Name, filter.Location)))

				.Where(e =>
					filter.SalaryMin == null
					|| (e.SalaryMin != null
						&& filter.SalaryMin <= e.SalaryMax))

				.Where(e =>
					filter.SalaryMax == null
					|| (e.SalaryMax != null
						&& filter.SalaryMax >= e.SalaryMin))

				.Where(e => 
					filter.IncludeListingsFromDate == null
					|| (e.CreatedDate != null
						&& filter.IncludeListingsFromDate < e.CreatedDate))

				.Where(e => 
					(filter.ExcludeExpiredListings == null
						|| filter.ExcludeExpiredListings == false)
					|| (filter.ExcludeExpiredListings == true
						&& e.ApplicationDeadline != null
						&& e.ApplicationDeadline > DateTime.Now));

			// TODO: Refactor to split at user defined character
			var keywords = filter?.KeywordsRawText?
				.Split(' ')
				.Where(k => k != null)
				.ToArray();

			if (keywords is not null
				&& query is not null
				&& keywords.Any())
			{
				for (int i = 0; i < keywords.Length; i++)
				{
					var keyword = keywords[i].Replace('_', ' ');

					query = query.Where(e =>
						(e.Employer != null && e.Employer.Name != null && EF.Functions.Like(e.Employer.Name, $"%{keyword}%"))
							|| (e.ListingTitle != null && EF.Functions.Like(e.ListingTitle, $"%{keyword}%"))
							|| (e.JobTitle != null && EF.Functions.Like(e.JobTitle, $"%{keyword}%"))
							|| (e.Description != null && EF.Functions.Like(e.Description, $"%{keyword}%"))
							|| (e.Location != null && e.Location.Name != null && EF.Functions.Like(e.Location.Name, $"%{keyword}%")));
				}
			}

			try
			{
				return await query?
					.Select(entity => new ListingOverviewDTO
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
						ExperienceLevel = entity.ExperienceLevel == null ? null : entity.ExperienceLevel.Name,
						ApplicationDeadline = entity.ApplicationDeadline == null ? null : entity.ApplicationDeadline
					})
					.ToListAsync()!;
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return null!;
			}
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
					ExperienceLevel = entity.ExperienceLevel?.Name,
					ApplicationDeadline = entity.ApplicationDeadline
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
				if (listingDTO.Id is not null && id is not null)
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
				return await _listingRepository
					.GetAll()
					.AnyAsync(e => e.Id == id);
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

			var oldEntity = await _listingRepository
				.GetAll()
				.Include(e => e.Location)
				.Include(e => e.Employer)
				.Include(e => e.ExperienceLevel)
				.FirstOrDefaultAsync(e => e.Id == listingDTO.Id);

			if (oldEntity is null)
			{
				_logger.LogWarning($"Could not find the listing when calling the repository in method {nameof(GetUpdatedListingEntityWithSubEntities)}.");
				return null;
			}

			oldEntity.ListingTitle = listingDTO.ListingTitle == null ? null : listingDTO.ListingTitle.Trim();
			oldEntity.Description = listingDTO.Description == null ? null : listingDTO.Description.Trim();
			oldEntity.SalaryMin = listingDTO.SalaryMin;
			oldEntity.SalaryMax = listingDTO.SalaryMax;
			oldEntity.JobTitle = listingDTO.JobTitle == null ? null : listingDTO.JobTitle.Trim();
			oldEntity.FTE = listingDTO.FTE;

			return await AddSubEntitiesToListing(listingDTO, oldEntity);
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
			newListingEntity.ListingTitle = listingDTO.ListingTitle! == null ? null : listingDTO.ListingTitle.Trim();
			newListingEntity.Description = listingDTO.Description == null ? null : listingDTO.Description.Trim();
			newListingEntity.SalaryMin = listingDTO.SalaryMin;
			newListingEntity.SalaryMax = listingDTO.SalaryMax;
			newListingEntity.JobTitle = listingDTO.JobTitle == null ? null : listingDTO.JobTitle.Trim();
			newListingEntity.FTE = listingDTO.FTE;
			newListingEntity.CreatedDate = DateTime.Now;
			newListingEntity.CreatedById = signedInUserId;

			newListingEntity = await AddSubEntitiesToListing(listingDTO, newListingEntity);

			return newListingEntity;
		}
	}
}
