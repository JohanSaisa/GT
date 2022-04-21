using GT.Core.DTO.Impl;
using GT.Core.FilterModels.Impl;
using GT.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
		[Route("Overview")]
		[HttpGet]
		public async Task<ActionResult<IEnumerable<string>>> GetListingsWithFilter()
		{
			ListingFilterModel filterModel = new ListingFilterModel();

			var listingDTOs = await _listingService
				.GetAsync(filterModel);

			if (listingDTOs == null)
			{
				return NotFound();
			}
			var result = JsonConvert.SerializeObject(listingDTOs);

			return Ok(result);
		}

		// GET:/Listing/
		[Route("Listing")]
		[HttpGet("{id}")]
		public async Task<ActionResult<string>> GetListing(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return BadRequest();
			}

			var listing = await _listingService
				.GetByIdAsync(id);

			if (listing == null)
			{
				return NotFound();
			}

			var result = JsonConvert.SerializeObject(listing);

			return Ok(result);
		}

		// GET: Listing/DeleteListing/5
		[Route("DeleteListing")]
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteListing(string? id)
		{
			if (string.IsNullOrEmpty(id))
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
			if (string.IsNullOrEmpty(id) || id != listing.Id)
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
		public async Task<ActionResult<string?>> PostListing(ListingDTO listing)
		{
			// TODO Need to learn how to get user id from jwt

			if (listing is null)
			{
				return BadRequest();
			}

			try
			{
				var objToReturn = await _listingService.AddAsync(listing, GetUserId());
				var result = JsonConvert.SerializeObject(objToReturn);
				return Ok(result);
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}

		private string GetUserId()
		{
			return User.Claims
				.First(i => i.Type == "UserId").Value;
		}
	}
}
