using GT.Core.DTO.Impl;
using GT.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GT.UI.Controllers
{
	public class UploadController : Controller
	{
		private readonly IGTCompanyService _companyService;

		public UploadController(IGTCompanyService companyService)
		{
			_companyService = companyService;
		}

		public async Task<IActionResult> Index()
		{
			CompanyLogoDTO model = new CompanyLogoDTO();
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> UploadAsync(CompanyLogoDTO model)
		{
			await _companyService.AddCompanyLogo(model);
			return View("Index", model);
		}
	}
}
