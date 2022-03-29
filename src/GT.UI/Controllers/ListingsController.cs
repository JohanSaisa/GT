#nullable disable
using GT.Data.Data.GTAppDb;
using GT.Data.Data.GTAppDb.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GT.UI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ListingsController : ControllerBase
	{
		private readonly GTAppContext _context;

		public ListingsController(GTAppContext context)
		{
			_context = context;
		}

		// GET: api/Listings
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Listing>>> GetListings()
		{
			return await _context.Listings.ToListAsync();
		}

		// GET: api/Listings/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Listing>> GetListing(string id)
		{
			var listing = await _context.Listings.FindAsync(id);

			if (listing == null)
			{
				return NotFound();
			}

			return listing;
		}

		// PUT: api/Listings/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		public async Task<IActionResult> PutListing(string id, Listing listing)
		{
			if (id != listing.Id)
			{
				return BadRequest();
			}

			_context.Entry(listing).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
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
		public async Task<ActionResult<Listing>> PostListing(Listing listing)
		{
			_context.Listings.Add(listing);
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateException)
			{
				if (ListingExists(listing.Id))
				{
					return Conflict();
				}
				else
				{
					throw;
				}
			}

			return CreatedAtAction("GetListing", new { id = listing.Id }, listing);
		}

		// DELETE: api/Listings/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteListing(string id)
		{
			var listing = await _context.Listings.FindAsync(id);
			if (listing == null)
			{
				return NotFound();
			}

			_context.Listings.Remove(listing);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool ListingExists(string id)
		{
			return _context.Listings.Any(e => e.Id == id);
		}
	}
}
