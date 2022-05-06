using GT.Core.DTO.Listing;
using GT.Core.FilterModels.Impl;
using GT.Core.FilterModels.Interfaces;
using GT.Core.Services.Interfaces;
using GT.Data.Data.AppDb.Entities;
using GT.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GT.Core.Services.Impl
{
	/// <summary>
	/// Contains business logic for listing objects. Used to map and convert data transfer objects and entities.
	/// </summary>
	public class ListingService : IListingService
	{
		private readonly ILogger<ListingService> _logger;

		private readonly IGenericRepository<Listing> _listingRepository;
		private readonly IGenericRepository<Company> _companyRepository;
		private readonly IGenericRepository<Location> _locationRepository;
		private readonly IGenericRepository<ExperienceLevel> _experienceLevelRepository;
		private readonly IGenericRepository<Inquiry> _inquiryRepository;

		public ListingService(
			ILogger<ListingService> logger,

			IGenericRepository<Listing> listingRepository,
			IGenericRepository<Company> companyRepository,
			IGenericRepository<Location> locationRepository,
			IGenericRepository<ExperienceLevel> experienceLevelRepository,
			IGenericRepository<Inquiry> inquiryRepository)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_listingRepository = listingRepository ?? throw new ArgumentNullException(nameof(listingRepository));
			_companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
			_locationRepository = locationRepository ?? throw new ArgumentNullException(nameof(locationRepository));
			_experienceLevelRepository = experienceLevelRepository ?? throw new ArgumentNullException(nameof(experienceLevelRepository));
			_inquiryRepository = inquiryRepository ?? throw new ArgumentNullException(nameof(inquiryRepository));
		}

		/// <summary>
		/// Converts a DTO to entities and updates the database.
		/// Requires the signed in users Id for assignment of CreatedBy property.
		/// </summary>
		/// <returns>The input DTO with an updated Id.</returns>
		public async Task<ListingDTO?> AddAsync(ListingDTO dto, string signedInUserId)
		{
			try
			{
				if (dto is null)
				{
					_logger.LogWarning($"Attempted to add a null reference to the database.");
					return null;
				}
				if (signedInUserId is null)
				{
					_logger.LogWarning($"Attempted to add a new listing with a null reference on signedInUserId.");
					return null;
				}

				var newListing = new Listing()
				{
					Id = Guid.NewGuid().ToString(),
					CreatedById = signedInUserId,
					CreatedDate = DateTime.Now,
					ApplicationDeadline = dto.ApplicationDeadline,
					ListingTitle = dto.ListingTitle == null ? null : dto.ListingTitle.Trim(),
					Description = dto.Description == null ? null : dto.Description.Trim(),
					JobTitle = dto.JobTitle == null ? null : dto.JobTitle.Trim(),
					SalaryMin = dto.SalaryMin,
					SalaryMax = dto.SalaryMax,
					FTE = dto.FTE,
					Employer = await _companyRepository.Get().FirstOrDefaultAsync(e => e.Name == dto.Employer),
					Location = await _locationRepository.Get().FirstOrDefaultAsync(e => e.Name == dto.Location),
					ExperienceLevel = await _experienceLevelRepository.Get().FirstOrDefaultAsync(e => e.Name == dto.ExperienceLevel),
				};

				await _listingRepository.AddAsync(newListing);

				// Assigning the updated id to the DTO as it is the only property with a new value.
				dto.Id = newListing.Id;
				dto.CreatedDate = newListing.CreatedDate;
				return dto;
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
				var entity = await _listingRepository.Get()
					.Include(e => e.Inquiries)
					.Include(e => e.ExperienceLevel)
					.Include(e => e.Employer)
					.Include(e => e.Location)

					.FirstOrDefaultAsync(e => e.Id == id);
				if (entity is not null)
				{
					await _listingRepository.DeleteAsync(entity);
				}
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
			}
		}

		public async Task<List<ListingOverviewDTO>> GetAllByFilterAsync(IListingFilterModel? filter = null)
		{
			// TODO - Refactor code and split up into multiple methods which modifys an IQueryable.
			// Suggesting that the methods should be implemented in the FilterModel as static methods.

			if (filter is null)
			{
				filter = new PostListingFilterDTO();
			}

			var query = _listingRepository?
				.Get()?
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
					(filter.ExcludeExpiredListings == null || filter.ExcludeExpiredListings == false)
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
					.Get()
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

		public async Task UpdateAsync(ListingDTO dto, string id)
		{
			if (dto is null || dto.Id is null || id is null)
			{
				_logger.LogWarning($"Arguments cannot be null when using the method: {nameof(UpdateAsync)}.");
				return;
			}
			if (dto.Id != id)
			{
				_logger.LogWarning($"IDs are not matching in method: {nameof(UpdateAsync)}.");
				return;
			}

			try
			{
				if (await ExistsByIdAsync(id))
				{
					var entity = await _listingRepository
						.Get()
						.Include(e => e.Employer)
						.Include(e => e.Location)
						.Include(e => e.ExperienceLevel)
						.FirstOrDefaultAsync(e => e.Id == dto.Id);

					entity.ListingTitle = dto.ListingTitle == null ? null : dto.ListingTitle.Trim();
					entity.Description = dto.Description == null ? null : dto.Description.Trim();
					entity.SalaryMin = dto.SalaryMin;
					entity.SalaryMax = dto.SalaryMax;
					entity.JobTitle = dto.JobTitle == null ? null : dto.JobTitle.Trim();
					entity.FTE = dto.FTE;
					entity.ApplicationDeadline = dto.ApplicationDeadline;

					if (entity.Employer?.Name != dto.Employer)
					{
						entity.Employer = await _companyRepository.Get()
							.FirstOrDefaultAsync(e => e.Name == dto.Employer);
					}
					if (entity.Location?.Name != dto.Location)
					{
						entity.Location = await _locationRepository.Get()
							.FirstOrDefaultAsync(e => e.Name == dto.Location);
					}
					if (entity.ExperienceLevel?.Name != dto.ExperienceLevel)
					{
						entity.ExperienceLevel = await _experienceLevelRepository.Get()
							.FirstOrDefaultAsync(e => e.Name == dto.ExperienceLevel);
					}

					await _listingRepository.UpdateAsync(entity, id);
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
					.Get()
					.AnyAsync(e => e.Id == id);
			}
			catch (Exception e)
			{
				_logger.LogError(e.Message);
				return false;
			}
		}
	}
}
