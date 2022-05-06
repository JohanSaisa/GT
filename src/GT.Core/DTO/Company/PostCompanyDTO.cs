using System.ComponentModel.DataAnnotations;

namespace GT.Core.DTO.Company
{
	public class PostCompanyDTO
	{
		[Required(ErrorMessage = "Name is required")]
		public string? Name { get; set; }

		public List<string>? Locations { get; set; } = new List<string>();
	}
}
