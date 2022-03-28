using GT.Core.FilterModels.Interfaces;

namespace GT.Core.FilterModels.Impl
{
	public class ListingFilterModel : IListingFilterModel
	{
		public List<string>? ExperienceLevel { get; set; }

		public List<string>? FreeText { get; set; }

		public bool? FTE { get; set; }

		public string? Location { get; set; }

		public int? SalaryMax { get; set; }

		public int? SalaryMin { get; set; }

		public DateTime? CreatedDate { get; set; }
	}
}
