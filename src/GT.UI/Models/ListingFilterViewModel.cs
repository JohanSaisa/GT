using GT.Core.FilterModels.Impl;

namespace GT.UI.Models
{
  public class ListingFilterViewModel
  {
		public ListingFilterModel Filter { get; set; } 
		public List<ExperienceLevelItem> ExperienceLevels { get; set; } = new();
  }
}
