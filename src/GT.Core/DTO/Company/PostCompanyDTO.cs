using GT.Core.DTO.Impl;
using System.ComponentModel.DataAnnotations;

namespace GT.Core.DTO.Company
{
	/// <summary>
	/// Represents a Company view and create model.
	/// </summary>
	public class PostCompanyDTO
	{
		[Required]
		public string? Name { get; set; }

		public List<LocationDTO>? Locations { get; set; }
	}
}
