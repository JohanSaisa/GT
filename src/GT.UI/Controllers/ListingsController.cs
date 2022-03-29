#nullable disable
using GT.Core.DTO.Impl;
using GT.Core.Services.Interfaces;
using GT.Data.Data.GTAppDb.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GT.UI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ListingsController : ControllerBase
	{
		private readonly IGTListingService _ListingService;
		private readonly UserManager<IdentityUser> _userManager;


		public ListingsController(IGTListingService iGTListingService, UserManager<IdentityUser> userManager)
{
			_ListingService = iGTListingService;
			_userManager = userManager;
		}

		// GET: api/Listings
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Listing>>> GetListings()
		{
			var listings = await _ListingService.GetAsync();

			if(listings == null)
			{
				return NotFound();
			}

			return Ok(listings);
		}

		// GET: api/Listings/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Listing>> GetListing(string id)
		{
			var listing = await _ListingService.GetByIdAsync(id);

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

			try
			{
				await _ListingService.UpdateAsync(listing, id);
			}

			catch (DbUpdateConcurrencyException)
			{
				if (!ListingExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();

		}

		// POST: api/Listings
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<Listing>> PostListing(ListingDTO listing)
		{
			await _ListingService.AddAsync(listing, _userManager.GetUserId(User));
			return CreatedAtAction("GetListing", new { id = listing.Id }, listing);
		}

		// DELETE: api/Listings/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteListing(string id)
		{
			var listing = await _ListingService.GetByIdAsync(id);
			if (listing == null)
			{
				return NotFound();
			}

			await _ListingService.DeleteAsync(id);

			return NoContent();
		}

		private bool ListingExists(string id)
		{
			return _ListingService.ExistsById(id).Result;
		}
	}
}
