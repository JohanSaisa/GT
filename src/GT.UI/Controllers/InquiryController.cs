using GT.Core.DTO.Impl;
using GT.Core.Services.Impl;
using GT.Core.Services.Interfaces;
using GT.Data.Data.GTIdentityDb.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GT.UI.Controllers
{
	public class InquiryController : Controller
	{
		private readonly IGTListingInquiryService _gtListingInquiryService;
		private readonly IGTListingService _gtListingService;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly UserManager<ApplicationUser> _userManager;

		public InquiryController(
			IGTListingInquiryService gtListingInquiryService,
			IGTListingService gtListingService,
			SignInManager<ApplicationUser> signInManager,
			UserManager<ApplicationUser> userManager)
		{
			_gtListingInquiryService = gtListingInquiryService;
			_gtListingService = gtListingService;
			_signInManager = signInManager;
			_userManager = userManager;
		}

		[HttpGet]
		public async Task<IActionResult> GetInquiry(string? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var inquiry = await _gtListingInquiryService
				.GetByIdAsync(id);

			if (inquiry == null)
			{
				return NotFound();
			}

			return View("~/Views/Inquiries/Inquiry.cshtml", inquiry);
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

			var listing = await _gtListingService.GetByIdAsync(inquiryDTO.ListingId);
			ViewData["RequestResult"] = result;

			return View("~/Views/Listing/Listing.cshtml", listing);
		}

		// GET: Listing/DeleteListing/5
		[Authorize(Policy = "AdminPolicy")]
		public async Task<IActionResult> Delete(string? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var inquiry = await _gtListingInquiryService
					.GetByIdAsync(id);

			if (inquiry == null)
			{
				return NotFound();
			}

			return View("~/Views/Inquiries/Delete.cshtml", inquiry);
		}

		// Delete:/DeleteListing/
		[Authorize(Policy = "AdminPolicy")]
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(string id)
		{
			var inquiry = await _gtListingInquiryService
					.GetByIdAsync(id);

			var listingId = inquiry.ListingId;

			var listing = await _gtListingService.GetByIdAsync(listingId);
			if (listing == null)
			{
				return NotFound();
			}

			await _gtListingInquiryService.DeleteAsync(id);

			return View("~/Views/Listing/Listing.cshtml", listing);
		}
	}
}
