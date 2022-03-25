using GT.Core.FilterModels.Interfaces;

namespace GT.Core.FilterModels.Impl
{
  public class ListingFilterModel : IListingFilterModel
  {
		string[]? ExperienceLevel { get; set; }

		string[]? FreeText { get; set; }

		bool? FTE { get; set; }

		string? Location { get; set; }

		int? MaxSalary { get; set; }

		int? MinSalary { get; set; }

		public DateTime? PostDate { get; set; }
  }
}
