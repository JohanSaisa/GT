using GT.Core.FilterModels;
using GT.Core.Services.Interfaces;
using GT.Data.Data.GTAppDb.Entities;
using GT.Data.Repositories;

namespace GT.Core.Services.Impl
{
	public class GTListingService : IGTService
	{
		private readonly IGTGenericRepository<Listing> _listingRepository;
		private readonly IGTIdentityRepository _identityRepository;

		public GTListingService(IGTGenericRepository<Listing> listingRepository, IGTIdentityRepository identityRepository)
		{
			_listingRepository = listingRepository;
			_identityRepository = identityRepository;
		}

		//TODO - Needs to be changed back to returntype IEnumerable<ListingPartialDTO>
		public IEnumerable<Listing> GetListingsByFilter(IListingFilterModel filter)
		{
			//Get entitites based on filter

			var query = _listingRepository.GetAll();

			var entities = query
				.Where(e => filter.FreeText == null || e.JobTitle.Contains(filter.FreeText))
				.Where(e => filter.FreeText == null || e.Description.Contains(filter.FreeText))
				.Where(e => filter.MinSalary == null || e.SalaryMin >= filter.MinSalary)
				.Where(e => filter.MaxSalary == null || e.SalaryMax <= filter.MaxSalary)
				.Where(e => filter.FTE == null || e.FTE == filter.FTE)
				.Where(e => filter.Location == null || e.Location.Name == filter.Location)
				.Where(e => filter.ExperienceLevel == null || filter.ExperienceLevel.Count == 0 || filter.ExperienceLevel.Contains(e.ExperienceLevel.Name))
				.ToList();

			//Map entities to DTOs

			//Return DTOs
			return entities;
		}
	}
}
