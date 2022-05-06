using System.ComponentModel.DataAnnotations.Schema;

namespace GT.Data.Data.AppDb.Entities
{
	public class Listing : IAppEntity
	{
		[Column(TypeName = "nvarchar(450)")]
		public string Id { get; set; } = string.Empty;

		[Column(TypeName = "nvarchar(100)")]
		public string? ListingTitle { get; set; }

		[Column(TypeName = "nvarchar(2000)")]
		public string? Description { get; set; }

		[Column(TypeName = "nvarchar(450)")]
		public string? EmployerId { get; set; }

		public Company? Employer { get; set; }

		[Column(TypeName = "nvarchar(450)")]
		public string? LocationId { get; set; }

		public Location? Location { get; set; }

		[Column(TypeName = "nvarchar(450)")]
		public string? ExperienceLevelId { get; set; }

		public ExperienceLevel? ExperienceLevel { get; set; }

		public int? SalaryMin { get; set; }

		public int? SalaryMax { get; set; }

		public bool? FTE { get; set; }

		[Column(TypeName = "nvarchar(100)")]
		public string? JobTitle { get; set; }

		[Column(TypeName = "nvarchar(450)")]
		public string? CreatedById { get; set; }

		[Column(TypeName = "Date")]
		public DateTime? CreatedDate { get; set; }
		
		[Column(TypeName = "Date")]
		public DateTime? ApplicationDeadline { get; set; }

		public ICollection<Inquiry> Inquiries { get; set; } = new List<Inquiry>();
	}
}
