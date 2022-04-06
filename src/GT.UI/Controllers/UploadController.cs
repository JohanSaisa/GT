using GT.Core.DTO.Impl;
using GT.Core.Services.Interfaces;
using GT.Data.Data.GTIdentityDb.Entities;
using Microsoft.AspNetCore.Identity;
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
		public IActionResult Index()
		{
			CompanyLogoDTO model = new CompanyLogoDTO();
			return View(model);
		}

		[HttpPost]
		public IActionResult Upload(CompanyLogoDTO model)
		{
				_companyService.AddCompanyLogo(model);		
			return View("Index", model);
		}
	}
}
