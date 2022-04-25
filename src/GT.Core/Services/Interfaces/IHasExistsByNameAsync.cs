using GT.Core.DTO.Interfaces;

namespace GT.Core.Services.Interfaces;

public interface IHasExistsByNameAsync
{
	Task<bool> ExistsByNameAsync(string name);
}
