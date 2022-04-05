using GT.Core.DTO.Impl;
using GT.Core.FilterModels.Impl;
using GT.Core.Services.Interfaces;
using GT.Data.Data.GTIdentityDb.Entities;
using GT.UI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GT.UI.Controllers
{
	public class ListingController : Controller
	{
		private readonly IGTListingService _listingService;
		private readonly IGTExperienceLevelService _experienceService;
		private readonly UserManager<ApplicationUser> _userManager;

		public ListingController(
			IGTListingService listingService, 
			IGTExperienceLevelService experienceService, 
			UserManager<ApplicationUser> userManager)
		{
			_listingService = listingService;
			_experienceService = experienceService;
			_userManager = userManager;
		}

		// GET: Listings
		[Route("Annonser")]
		[HttpGet]
		public async Task<ActionResult<IEnumerable<ListingOverviewDTO>>> ListingOverview(ListingFilterViewModel? filterModel)
		{
			if(filterModel is not null 
				&& filterModel.ExperienceLevels.Any()
				&& filterModel.Filter is not null)
			{
			filterModel.Filter.ExperienceLevels = filterModel.ExperienceLevels
				.Where(el => el != null && el.IsSelected)
				.Select(el => el.Name)
				.ToList();
			}

			var listingDTOs = await _listingService
				.GetAsync(filterModel?.Filter);

			if (listingDTOs == null)
			{
				return NotFound();
			}

			if (filterModel is not null)
			{
				var experienceLevels = await _experienceService
					.GetAllAsync();

				ViewData["ExperienceLevels"] = experienceLevels is not null
					? experienceLevels.Select(el => new ExperienceLevelItem
					{
						Name = el.Name,
						IsSelected = false
					})
					.ToList()
					: new List<ExperienceLevelItem>();
			}
			else
			{
					ViewData["ExperienceLevels"] = filterModel?.ExperienceLevels;
			}

			return View(listingDTOs);
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

			return View("Listing", listing);
		}
	}
}
