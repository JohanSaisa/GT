using Microsoft.AspNetCore.Http;

namespace GT.Core.DTO.Impl
{
	public class CompanyLogoDTO
	{
		public string CompanyId { get; set; }
		public IFormFile File { get; set; }
	}
}
