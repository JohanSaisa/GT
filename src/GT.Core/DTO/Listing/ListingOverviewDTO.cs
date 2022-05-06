namespace GT.Core.DTO.Listing
{
	/// <summary>
	/// Intended view model for a listing which is part of a list of other listings.
	/// </summary>
	public class ListingOverviewDTO
	{
		public string? Id { get; set; }
		public string? ListingTitle { get; set; }
		public string? EmployerName { get; set; }
		public int? SalaryMin { get; set; }
		public int? SalaryMax { get; set; }
		public string? JobTitle { get; set; }
		public string? Location { get; set; }
		public bool? FTE { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string? ExperienceLevel { get; set; }
		public DateTime? ApplicationDeadline { get; set; }
	}
}
