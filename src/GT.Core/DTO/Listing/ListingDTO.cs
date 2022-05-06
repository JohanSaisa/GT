using System.ComponentModel.DataAnnotations;

namespace GT.Core.DTO.Listing
{
	/// <summary>
	/// Represents a listing view and create model.
	/// </summary>
	public class ListingDTO
	{
		[StringLength(450)]
		public string? Id { get; set; }

		[StringLength(100)]
		public string? ListingTitle { get; set; }

		[StringLength(2000)]
		public string? Description { get; set; }

		/// <summary>
		/// Maps to a company name in the database.
		/// </summary>
		[StringLength(200)]
		public string? Employer { get; set; }

		public int? SalaryMin { get; set; }

		public int? SalaryMax { get; set; }

		[StringLength(100)]
		public string? JobTitle { get; set; }

		/// <summary>
		/// Maps to a location name in the database.
		/// </summary>
		[StringLength(200)]
		public string? Location { get; set; }

		//Full Time Equivelent
		public bool? FTE { get; set; }

		[DataType(DataType.Date)]
		public DateTime? CreatedDate { get; set; }

		/// <summary>
		/// Maps to an ExperienceLevel name in the database.
		/// </summary>
		[StringLength(100)]
		public string? ExperienceLevel { get; set; }

		public List<InquiryDTO>? Inquiries { get; set; }

		/// <summary>
		/// Maps to an ApplicationDeadline name in the database.
		/// </summary>
		public DateTime? ApplicationDeadline { get; set; }
	}
}
