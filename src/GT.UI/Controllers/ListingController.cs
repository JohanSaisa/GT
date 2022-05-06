using GT.Core.DTO.Listing;
using GT.Core.FilterModels.Impl;
using GT.Core.Services.Interfaces;
using GT.Data.Data.GTIdentityDb.Entities;
using GT.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GT.UI.Controllers
{
	public class ListingController : Controller
	{
		private readonly IGTListingService _listingService;
		private readonly IGTExperienceLevelService _experienceService;
		private readonly IGTLocationService _locationService;
		private readonly IGTListingInquiryService _listingInquiryService;
		private readonly UserManager<ApplicationUser> _userManager;

		public ListingController(
			IGTListingService listingService,
			IGTExperienceLevelService experienceService,
			IGTLocationService locationService,
			IGTListingInquiryService listingInquiryService,
			UserManager<ApplicationUser> userManager)
		{
			_listingService = listingService ?? throw new ArgumentNullException(nameof(listingService));
			_experienceService = experienceService ?? throw new ArgumentNullException(nameof(experienceService));
			_locationService = locationService ?? throw new ArgumentNullException(nameof(locationService));
			_listingInquiryService = listingInquiryService ?? throw new ArgumentNullException(nameof(listingInquiryService));
			_userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
		}

		// GET: Listings
		[HttpGet]
		public async Task<ActionResult<IEnumerable<ListingOverviewDTO>>> ListingOverview(ListingFilterViewModel? filterModel)
		{
			if (filterModel is not null
				&& filterModel.Filter is not null)
			{
				filterModel.Filter.ExcludeExpiredListings = filterModel.ExcludeExpiredListings;

				if (filterModel.ExperienceLevels is not null
					&& filterModel.ExperienceLevels.Any())
				{
					filterModel.Filter.ExperienceLevels = filterModel.ExperienceLevels
						.Where(el => el != null && el.IsSelected)
						.Select(el => el.Name)
						.ToList()!;
				}
			}

			var listingDTOs = await _listingService
				.GetAllByFilterAsync(filterModel?.Filter);

			if (listingDTOs == null)
			{
				return NotFound();
			}

			if (filterModel is not null)
			{
				var experienceLevels = await _experienceService
					.GetAllAsync();

				var locations = await _locationService
					.GetAllAsync();

				ViewData["ExperienceLevels"] = experienceLevels is not null
					? experienceLevels.Select(el => new ExperienceLevelCheckbox
					{
						Name = el.Name,
						IsSelected = false
					})
					.ToList()
					: new List<ExperienceLevelCheckbox>();

				ViewData["Locations"] = new SelectList(locations?
					.Select(l => new SelectListItem
					{
						Selected = false,
						Text = l.Name,
						Value = l.Name
					})
					.OrderBy(l => l.Text),
					"Value",
					"Text");
			}

			return View(listingDTOs);
		}

		// GET:/GetListing/
		[HttpGet("{id}")]
		public async Task<ActionResult<ListingDTO>> GetListing(string id)
		{
			var listing = await _listingService
				.GetByIdAsync(id);

			if (listing == null)
			{
				return NotFound();
			}

			listing.Inquiries = await _listingInquiryService
				.GetByListingIdAsync(listing.Id!);

			return View("Listing", listing);
		}

		// GET: Listing/DeleteListing/5
		[Authorize(Policy = "AdminPolicy")]
		public async Task<IActionResult> Delete(string? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var listing = await _listingService
					.GetByIdAsync(id);

			if (listing == null)
			{
				return NotFound();
			}

			return View(listing);
		}

		// Delete:/DeleteListing/
		[Authorize(Policy = "AdminPolicy")]
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(string id)
		{
			await _listingService.DeleteAsync(id);
			return RedirectToAction("ListingOverview");
		}
	}
}