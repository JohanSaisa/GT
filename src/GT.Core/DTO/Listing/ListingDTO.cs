using GT.Core.DTO.Inquiry;
using System.ComponentModel.DataAnnotations;
using GT.Core.DTO.Impl;

namespace GT.Core.DTO.Listing
{
	public class ListingDTO
	{
		public string? Id { get; set; }

		public string? ListingTitle { get; set; }

		public string? Description { get; set; }

		public string? Employer { get; set; }

		public int? SalaryMin { get; set; }

		public int? SalaryMax { get; set; }

		public string? JobTitle { get; set; }

		public string? Location { get; set; }

		public bool? FTE { get; set; }
		
		public string? ExperienceLevel { get; set; }
		
		public DateTime? CreatedDate { get; set; }

		public DateTime? ApplicationDeadline { get; set; }
	}
}
