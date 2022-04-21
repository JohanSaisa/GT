using GT.Core.DTO.Impl;
using GT.Core.FilterModels.Impl;
using GT.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GT.API.Controllers
{
	[Route("api/v1/listings")]
	[ApiController]
	public class ListingController : ControllerBase
	{
		private readonly IGTListingService _listingService;
		private readonly IGTExperienceLevelService _experienceService;
		private readonly IGTLocationService _locationService;
		private readonly IGTListingInquiryService _listingInquiryService;

		public ListingController(
			IGTListingService listingService,
			IGTExperienceLevelService experienceService,
			IGTLocationService locationService,
			IGTListingInquiryService listingInquiryService)
		{
			_listingService = listingService ?? throw new ArgumentNullException(nameof(listingService));
			_experienceService = experienceService ?? throw new ArgumentNullException(nameof(experienceService));
			_locationService = locationService ?? throw new ArgumentNullException(nameof(locationService));
			_listingInquiryService = listingInquiryService ?? throw new ArgumentNullException(nameof(listingInquiryService));
		}

		// GET: Listing/ListingsWithFilter
		[Route("ListingsOverview")]
		[HttpGet]
		public async Task<ActionResult<IEnumerable<ListingOverviewDTO>>> GetListingsWithFilter(ListingFilterModel? filterModel)
		{
			var listingDTOs = await _listingService
				.GetAsync(filterModel);

			if (listingDTOs == null)
			{
				return NotFound();
			}

			return Ok(listingDTOs);
		}

		// GET:/Listing/
		[Route("Listing")]
		[HttpGet("{id}")]
		public async Task<ActionResult<ListingDTO>> GetListing(string id)
		{
			if (id == null)
			{
				return BadRequest();
			}

			var listing = await _listingService
				.GetByIdAsync(id);

			if (listing == null)
			{
				return NotFound();
			}

			return Ok(listing);
		}

		// GET: Listing/DeleteListing/5
		[Route("DeleteListing")]
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteListing(string? id)
		{
			if (id == null)
			{
				return BadRequest();
			}

			try
			{
				await _listingService.DeleteAsync(id);
			}
			catch (Exception)
			{
				return NotFound();
			}

			return Ok();
		}

		// PUT: Listing/UpdateListing
		[Route("UpdateListing")]
		[HttpPut("{id}")]
		public async Task<IActionResult> PutListing(string id, ListingDTO listing)
		{
			if (id != listing.Id)
			{
				return BadRequest();
			}

			try
			{
				await _listingService.UpdateAsync(listing, id);
			}
			catch
			{
				return StatusCode(500);
			}

			return Ok();
		}

		// POST: api/Listings
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[Route("CreateListing")]
		[HttpPost]
		public async Task<ActionResult<ListingDTO>> PostListing(ListingDTO listing)
{
			// TODO Need to learn how to 
		}
	}
}
