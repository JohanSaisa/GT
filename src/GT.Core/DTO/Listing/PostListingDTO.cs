using System.ComponentModel.DataAnnotations;

namespace GT.Core.DTO.Listing
{
	public class PostListingDTO
	{
		[Required]
		[StringLength(100)]
		public string? ListingTitle { get; set; }

		[StringLength(2000)]
		public string? Description { get; set; }

		[Required]
		[StringLength(200)]
		public string? Employer { get; set; }

		public int? SalaryMin { get; set; }

		public int? SalaryMax { get; set; }

		[StringLength(100)]
		public string? JobTitle { get; set; }

		[StringLength(200)]
		public string? Location { get; set; }

		public bool? FTE { get; set; }

		[StringLength(100)]
		public string? ExperienceLevel { get; set; }

		[Required]
		public DateTime? ApplicationDeadline { get; set; }
	}
}
