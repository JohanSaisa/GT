using GT.Core.DTO.Impl;
using Microsoft.CodeAnalysis;

namespace GT.Core.Services.Interfaces
{
	public interface IGTLocationService : 
		IGTService<LocationDTO>, 
		IHasGetAllAsyncWithoutParameters<LocationDTO>,
		IHasAddAsyncWithoutUserId<LocationDTO>, 
		IHasExistsByNameAsync
	{
	}
}
