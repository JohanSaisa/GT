using GT.Core.DTO.Impl;
using GT.Core.FilterModels.Impl;
using GT.Core.Services.Interfaces;
using GT.Data.Data.GTIdentityDb.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GT.UI.Controllers
{
	[Route("[controller]")]
	public class ListingController : Controller
	{
		private readonly IGTListingService _listingService;
		private readonly UserManager<ApplicationUser> _userManager;

		public ListingController(IGTListingService listingService, UserManager<ApplicationUser> userManager)
		{
			_listingService = listingService;
			_userManager = userManager;
		}

		// GET: api/Listings
		[HttpGet]
		public async Task<ActionResult<IEnumerable<ListingOverviewDTO>>> GetListings(ListingFilterModel filterModel = null)
		{
			// TODO Populate and create a filtermodel

			var listingDTOs = await _listingService.GetAsync(filterModel);

			if (listingDTOs == null)
			{
				return NotFound();
			}

			return Ok(listingDTOs);
		}

		// GET:/GetListing/
		[HttpGet("{id}")]
		public async Task<ActionResult<ListingDTO>> GetListing(string id)
		{
			var listing = await _listingService.GetByIdAsync(id);

			if (listing == null)
			{
				return NotFound();
			}

			return View(listing);
		}
	}
}
