#nullable disable

using GT.Core.DTO.Impl;
using GT.Core.Services.Interfaces;
using GT.Data.Data.GTAppDb.Entities;
using GT.Data.Data.GTIdentityDb.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GT.UI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ListingsController : ControllerBase
	{
		private readonly IGTListingService _listingService;
		private readonly UserManager<ApplicationUser> _userManager;

		public ListingsController(IGTListingService listingService, UserManager<ApplicationUser> userManager)
		{
			_listingService = listingService;
			_userManager = userManager;
		}

		// GET: api/Listings
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Listing>>> GetListings()
		{
			var listings = await _listingService.GetAsync();

			if (listings == null)
			{
				return NotFound();
			}

			return Ok(listings);
		}

		// GET: api/Listings/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Listing>> GetListing(string id)
		{
			var listing = await _listingService.GetByIdAsync(id);

			if (listing == null)
			{
				return NotFound();
			}

			return Ok(listing);
		}

		// PUT: api/Listings/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		public async Task<IActionResult> PutListing(string id, ListingDTO listing)
		{
			if (id != listing.Id)
			{
				return BadRequest();
			}

			await _listingService.UpdateAsync(listing, id);

			return NoContent();
		}

		// POST: api/Listings
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<Listing>> PostListing(ListingDTO listing)
		{
			await _listingService.AddAsync(listing, _userManager.GetUserId(User));
			return CreatedAtAction("GetListing", new { id = listing.Id }, listing);
		}

		// DELETE: api/Listings/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteListing(string id)
		{
			if (!_listingService.ExistsById(id).Result)
			{
				return NotFound();
			}

			await _listingService.DeleteAsync(id);

			return NoContent();
		}

	}
}
