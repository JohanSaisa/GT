using GT.Core.DTO.Impl;
using GT.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GT.UI.Controllers
{
	public class InquiryController : Controller
	{
		private readonly IGTListingInquiryService _gtListingInquiryService;

		public InquiryController(IGTListingInquiryService gtListingInquiryService)
		{
			_gtListingInquiryService = gtListingInquiryService;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult CreateInquiry(ListingInquiryDTO inquiryDTO, string? signedInUserId = null)
		{
			_gtListingInquiryService.AddAsync(inquiryDTO, signedInUserId);
		}
	}
}
