using Microsoft.AspNetCore.Http;

namespace GT.Core.DTO.Impl
{
	public class CompanyLogoDTO
	{
		public string FileName { get; set; }
		public IFormFile File { get; set; }
	}
}
