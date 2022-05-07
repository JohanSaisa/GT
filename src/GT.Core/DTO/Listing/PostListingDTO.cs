using System.ComponentModel.DataAnnotations;

namespace GT.Core.DTO.Listing
{
	public class PostListingDTO
	{
		[Required(ErrorMessage = "ListingTitle is required")]
		[StringLength(100)]
		public string? ListingTitle { get; set; }

		[StringLength(2000)]
		public string? Description { get; set; }

		[Required(ErrorMessage = "Employer is required")]
		[StringLength(200)]
		public string? Employer { get; set; }

		public int? SalaryMin { get; set; }

		public int? SalaryMax { get; set; }

		[StringLength(100)]
		public string? JobTitle { get; set; }

		[Required]
		[StringLength(200)]
		public string? Location { get; set; }

		public bool? FTE { get; set; }

		[StringLength(100)]
		public string? ExperienceLevel { get; set; }

		[Required(ErrorMessage = "ApplicationDeadline is required")]
		public DateTime? ApplicationDeadline { get; set; }
	}
}
