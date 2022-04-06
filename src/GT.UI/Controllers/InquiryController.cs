using GT.Core.DTO.Impl;
using GT.Core.Services.Interfaces;
using GT.Data.Data.GTIdentityDb.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GT.UI.Controllers
{
	[Route("")]
	public class InquiryController : Controller
	{
		private readonly IGTListingInquiryService _gtListingInquiryService;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly UserManager<ApplicationUser> _userManager;

		public InquiryController(
			IGTListingInquiryService gtListingInquiryService,
			SignInManager<ApplicationUser> signInManager,
			UserManager<ApplicationUser> userManager)
		{
			_gtListingInquiryService = gtListingInquiryService;
			_signInManager = signInManager;
			_userManager = userManager;
		}

		[HttpPost]
		public async Task<IActionResult> CreateInquiry(ListingInquiryDTO inquiryDTO)
		{
			string? userIdValue = null;
			if (_signInManager.IsSignedIn(User))
			{
				var claimsIdentity = User.Identity as ClaimsIdentity;
				if (claimsIdentity != null)
				{
					var userIdClaim = claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

					if (userIdClaim != null)
					{
						userIdValue = userIdClaim.Value;
					}
				}
			}

			inquiryDTO.ListingId = Request.Form["ListingId"];

			var listingInquiry = await _gtListingInquiryService.AddAsync(inquiryDTO, userIdValue);

			var result = "Application was submitted successfully!";

			if (listingInquiry == null)
			{
				result = "Application was not submitted.";
			}

			ViewData["RequestResult"] = result;

			return Redirect(inquiryDTO.ListingId.ToString());
		}
	}
}
