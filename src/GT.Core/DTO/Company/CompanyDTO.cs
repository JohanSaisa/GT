using GT.Core.DTO.Impl;
using System.ComponentModel.DataAnnotations;

namespace GT.Core.DTO.Company
{
	public class CompanyDTO
	{
		public string? Id { get; set; }
		public string? Name { get; set; }
		public List<string>? Locations { get; set; }
	}
}
