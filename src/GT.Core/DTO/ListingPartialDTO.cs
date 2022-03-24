namespace GT.Core.DTO
{
	/// <summary>
	/// Intended viewmodel for list of listings.
	/// </summary>
	public class ListingPartialDTO : IGTDataTransferObject
	{
		public string Id { get; set; }
		public string ListingTitle { get; set; }
		public int? SalaryMin { get; set; }
		public int? SalaryMax { get; set; }
		public string? JobTitle { get; set; }
		public string? Location { get; set; }
		public bool? FTE { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string? ExperienceLevel { get; set; }
	}
}
