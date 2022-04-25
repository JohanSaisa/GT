#nullable disable

using GT.Core.DTO.Impl;
using GT.Core.FilterModels.Impl;
using GT.Core.FilterModels.Interfaces;
using GT.Core.Services.Interfaces;
using GT.Data.Data.GTIdentityDb.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GT.UI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ListingAPIController : ControllerBase
	{
		private readonly IGTListingService _listingService;
		private readonly UserManager<ApplicationUser> _userManager;

		public ListingAPIController(IGTListingService listingService, UserManager<ApplicationUser> userManager)
		{
			_listingService = listingService ?? throw new ArgumentNullException(nameof(listingService));
			_userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
		}

		// GET: api/Listings
		[HttpGet]
		public async Task<ActionResult<IEnumerable<ListingOverviewDTO>>> GetListings(ListingFilterModel filterModel)
		{
			var listingDTOs = await _listingService.GetAllByFilterAsync(filterModel);

			if (listingDTOs == null || listingDTOs.Count <= 0)
			{
				return NotFound();
			}

			return Ok(listingDTOs);
		}

		// GET: api/Listings/5
		[HttpGet("{id}")]
		public async Task<ActionResult<ListingDTO>> GetListing(string id)
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
		[Authorize(Policy = "AdminPolicy")]
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
		[Authorize(Policy = "AdminPolicy")]
		public async Task<ActionResult<ListingDTO>> PostListing(ListingDTO listing)
		{
			var listingDTO = await _listingService.AddAsync(listing, _userManager.GetUserId(User));
			if (listingDTO == null)
			{
				return BadRequest();
			}
			return CreatedAtAction("GetListing", listingDTO);
		}

		// DELETE: api/Listings/5
		[HttpDelete("{id}")]
		[Authorize(Policy = "AdminPolicy")]
		public async Task<IActionResult> DeleteListing(string id)
		{
			if (!await _listingService.ExistsByIdAsync(id))
			{
				return NotFound();
			}

			await _listingService.DeleteAsync(id);

			return NoContent();
		}
	}
}