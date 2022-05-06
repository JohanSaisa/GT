using GT.Core.DTO.Company;
using System.ComponentModel.DataAnnotations;

namespace GT.Core.DTO.Impl
{
	public class LocationDTO
	{
		public string? Id { get; set; }

		public string? Name { get; set; }

		public List<CompanyDTO> Companies { get; set; }
	}
}
