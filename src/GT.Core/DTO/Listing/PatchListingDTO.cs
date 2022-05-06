using System.ComponentModel.DataAnnotations;

namespace GT.Core.DTO.Listing
{
	public class PatchListingDTO
	{
		[StringLength(2000)]
		public string? Description { get; set; }

		public int? SalaryMin { get; set; }

		public int? SalaryMax { get; set; }

		[StringLength(100)]
		public string? JobTitle { get; set; }

		[StringLength(200)]
		public string? Location { get; set; }

		public bool? FTE { get; set; }

		[StringLength(100)]
		public string? ExperienceLevel { get; set; }
	}
}
