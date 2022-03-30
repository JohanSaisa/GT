using GT.Core.DTO.Interfaces;

namespace GT.Core.DTO.Impl
{
	/// <summary>
	/// Intended view model for a listing which is part of a list of other listings.
	/// Should not be used as a create model as the ListingPartialDTO does not contain
	/// all necessary properties needed for the creation of the listing entity.
	/// Instead use the ListingDTO.
	/// </summary>
	public class ListingOverviewDTO : IGTDataTransferObject
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
	}
}
