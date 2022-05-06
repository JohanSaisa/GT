using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GT.Core.DTO.Company
{
	public class CompanyLogoDTO
	{
		public string? CompanyId { get; set; }
		public IFormFile? Logo { get; set; }
	}
}
