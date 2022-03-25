using GT.Core.FilterModels.Interfaces;
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
	}
}
