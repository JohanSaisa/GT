using GT.Core.FilterModels.Impl;
using GT.Core.FilterModels.Interfaces;
using GT.Core.Services.Interfaces;
using GT.Data.Data.GTAppDb.Entities;
using GT.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GT.Core.Services.Impl
{
	public class GTListingService : IGTService, IGTListingService
	{
		private readonly IGTGenericRepository<Listing> _listingRepository;
		private readonly IGTIdentityRepository _identityRepository;

		public GTListingService(IGTGenericRepository<Listing> listingRepository, IGTIdentityRepository identityRepository)
		{
			_listingRepository = listingRepository;
			_identityRepository = identityRepository;
		}
		public async Task<List<Listing>> GetAsync(IListingFilterModel? filter = null)
		{
			if (filter is null)
			{
				filter = new ListingFilterModel();
			}

			var query = _listingRepository
				.GetAll()
				.Include(e => e.ExperienceLevel)

				.Where(e => filter.ExperienceLevel == null
					|| filter.ExperienceLevel.Count <= 0
					|| filter.ExperienceLevel.Contains(e.ExperienceLevel.Name))

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

				.Where(e => filter.CreatedDate == null
					|| (e.CreatedDate != null
						&& filter.CreatedDate < e.CreatedDate))

						//.Where(e =>
						//	filter.FreeText == null
						//	|| filter.FreeText.Length <= 0
						//	|| filter.FreeText.All(ft =>
						//		(e.JobTitle != null
						//			&& e.JobTitle.Contains(ft, StringComparison.OrdinalIgnoreCase))
						//		|| (e.Description != null
						//			&& e.Description.Contains(ft, StringComparison.OrdinalIgnoreCase))
						//		|| (e.Location != null
						//			&& e.Location.Name != null
						//			&& e.Location.Name.Contains(ft, StringComparison.OrdinalIgnoreCase))))
						;

			return await query
				.ToListAsync();
		}
	}
}
