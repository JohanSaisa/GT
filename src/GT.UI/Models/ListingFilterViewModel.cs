using GT.Core.DTO.Impl;
using GT.Core.FilterModels.Impl;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GT.UI.Models
{
	public class ListingFilterViewModel
	{
		public ListingFilterModel? Filter { get; set; }
		public List<ExperienceLevelCheckbox> ExperienceLevels { get; set; } = new();
		public SelectList? Locations { get; set; }
		public bool ExcludeExpiredListings { get; set; }
	}
}
