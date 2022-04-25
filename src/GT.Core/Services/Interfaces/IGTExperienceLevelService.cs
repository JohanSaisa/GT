using GT.Core.DTO.Impl;

namespace GT.Core.Services.Interfaces
{
	public interface IGTExperienceLevelService : 
		IGTService<ExperienceLevelDTO>,
		IHasGetAllAsyncWithoutParameters<ExperienceLevelDTO>,
		IHasAddAsyncWithoutUserId<ExperienceLevelDTO>,
		IHasExistsByNameAsync
	{
	}
}
