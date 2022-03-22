using GT.Core.DTO;
using GT.Core.Services.Interfaces;
using GT.Data.Data.GTAppDb.Entities;
using GT.Data.Repositories;

namespace GT.Core.Services.Impl
{
	public class GTAddressService : GTGenericService<Address, AddressDTO>, IGTAddressService
	{
		public GTAddressService(IGTGenericRepository<Address> repository) : base(repository)
		{
		}

		protected override Address DTOToEntity(AddressDTO dataTransferObject)
		{
			throw new NotImplementedException();
		}

		protected override AddressDTO EntityToDTO(Address entity)
		{
			throw new NotImplementedException();
		}
	}
}
