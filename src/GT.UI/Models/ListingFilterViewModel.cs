using GT.Core.DTO.Impl;
using GT.Core.FilterModels.Impl;

namespace GT.UI.Models
{
	public class ListingFilterViewModel
	{
		public ListingFilterModel Filter { get; set; } = new();
		public List<ExperienceLevelDTO> ExperienceLevels { get; set; } = new();
	}
}
