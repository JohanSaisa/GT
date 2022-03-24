namespace GT.Core.FilterModels
{
	public class ListingFilterModel : IListingFilterModel
	{
		// Includes listing title & description
		public string? FreeText { get; set; }
		public int? MinSalary { get; set; }
		public int? MaxSalary { get; set; }
		// Full Time Equivelent
		public bool? FTE { get; set; }
		public string? Location { get; set; }
		ICollection<string>? ExperienceLevel { get; set; }
	}
}
