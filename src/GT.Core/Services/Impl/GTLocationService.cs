using GT.Core.DTO;
using GT.Core.Services.Interfaces;
using GT.Data.Data.GTAppDb.Entities;
using GT.Data.Repositories;

namespace GT.Core.Services.Impl
{
	public class GTLocationService : GTGenericService<City, LocationDTO>, IGTLocationService
	{
		public GTLocationService(IGTGenericRepository<City> repository) : base(repository)
		{
		}

		protected override City DTOToEntity(LocationDTO dataTransferObject)
		{
			throw new NotImplementedException();
		}

		protected override LocationDTO EntityToDTO(City entity)
		{
			throw new NotImplementedException();
		}
	}
}
