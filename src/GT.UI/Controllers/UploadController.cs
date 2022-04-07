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
		public async Task<IActionResult> Index()
		{
			var model = await _companyService.GetCompanyLogo("3d0bb741-0f74-43b8-a30f-20f8c2f3fa77");
			var image = model.File;
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> UploadAsync(CompanyLogoDTO model)
		{
			await	_companyService.AddCompanyLogo(model);		
			return View("Index", model);
		}
	}
}
