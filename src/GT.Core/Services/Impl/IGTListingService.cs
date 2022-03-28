using GT.Core.FilterModels.Interfaces;
using GT.Data.Data.GTAppDb.Entities;

namespace GT.Core.Services.Impl
{
	public interface IGTListingService
	{
		Task<List<Listing>> GetAsync(IListingFilterModel? filter = null);
	}
}
