using GT.Core.DTO;
using GT.Core.Services.Interfaces;
using GT.Data.Data.GTAppDb.Entities;
using GT.Data.Repositories;

namespace GT.Core.Services.Impl
{
	public class GTLocationService : GTGenericService<Location, LocationDTO>, IGTLocationService
	{
		public GTLocationService(IGTGenericRepository<Location> repository) : base(repository)
		{
		}

		protected override Location DTOToEntity(LocationDTO dataTransferObject)
		{
			throw new NotImplementedException();
		}

		protected override LocationDTO EntityToDTO(Location entity)
		{
			throw new NotImplementedException();
		}
	}
}
