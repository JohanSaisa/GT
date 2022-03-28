using GT.Core.DTO;
using GT.Core.FilterModels.Impl;
using GT.Core.FilterModels.Interfaces;
using GT.Core.Services.Interfaces;
using GT.Data.Data.GTAppDb.Entities;
using GT.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GT.Core.Services.Impl
{
	public class GTListingService : IGTService, IGTListingService
	{
		private readonly ILogger<GTListingService> _logger;
		private readonly IGTGenericRepository<Listing> _listingRepository;
		private readonly IGTIdentityRepository _identityRepository;

		public GTListingService(ILogger<GTListingService> logger, IGTGenericRepository<Listing> listingRepository, IGTIdentityRepository identityRepository)
		{
			_logger = logger;
			_listingRepository = listingRepository;
			_identityRepository = identityRepository;
		}

		public Task<ListingDTO> AddAsync(CreateListingDTO listingDTO)
		{
			// Map DTO to entity
			var newListing = new Listing()
			{
				Id = Guid.NewGuid().ToString(),
				ListingTitle = listingDTO.ListingTitle,
				Description = listingDTO.Description,
				// Map to employer
				Employer = listingDTO.Employer?.Name,
				SalaryMin = listingDTO.SalaryMin,
				SalaryMax = listingDTO.SalaryMax,
				JobTitle = listingDTO.JobTitle,
				// Map to Location
				Location = listingDTO.Location?.Name,
				FTE = listingDTO.FTE,
				CreatedDate = listingDTO.CreatedDate,
				// Map to experience level
				ExperienceLevel = listingDTO.ExperienceLevel?.Name

				//Created by needs to get their name from Identity
			};

			// Update database

			_listingRepository.AddAsync(newListing);


			// Map model to DTO
			var newListingDTO = new ListingDTO

			// return the updated ListingDTO
			return;
		}

		public Task DeleteAsync(string id)
		{
			throw new NotImplementedException();
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
					|| (e.ExperienceLevel != null && (e.ExperienceLevel.Name != null || filter.ExperienceLevels.Contains(e.ExperienceLevel.Name))))

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

			var entities = await query
				.ToListAsync();

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

		public Task UpdateAsync(ListingDTO listingDTO, string id)
		{
			throw new NotImplementedException();
		}
	}
}
